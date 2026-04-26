using System.Globalization;
using System.Text.RegularExpressions;
using KampusRitim.Application.Interfaces;
using KampusRitim.Domain.Enums;
using KampusRitim.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace KampusRitim.Infrastructure.Services
{
    public sealed class ChatDataQueryService : IChatDataQueryService
    {
        private readonly AppDbContext _context;
        private readonly TimeZoneInfo _appTimeZone;

        public ChatDataQueryService(AppDbContext context, IConfiguration configuration)
        {
            _context = context;
            _appTimeZone = ResolveTimeZone(configuration["App:TimeZone"]);
        }

        public async Task<string?> TryResolveAsync(string message, int? userId, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(message))
                return null;

            var text = Normalize(message);

            if (LooksLikeEventQuery(text))
            {
                var nowInTimeZone = GetNowInAppTimeZone();
                var (rangeStartLocal, rangeEndLocal, rangeLabel, needsClarification) = ParseDateTimeRange(text, nowInTimeZone);
                var rangeStartUtc = ToUtc(rangeStartLocal);
                var rangeEndUtc = ToUtc(rangeEndLocal);
                var nowUtc = ToUtc(nowInTimeZone);

                if (needsClarification)
                    return "Etkinlikleri hangi gün/saat için arıyorsun? Örn: 'bugün saat 15:00' veya 'yarın 10:00'.";

                // For week queries, avoid listing past events from earlier in the same week.
                var effectiveStartUtc = rangeLabel == "bu hafta" && rangeStartUtc < nowUtc ? nowUtc : rangeStartUtc;

                var query = _context.Events
                    .AsNoTracking()
                    .Include(e => e.Club)
                    .Where(e => e.Time >= effectiveStartUtc && e.Time < rangeEndUtc)
                    .OrderBy(e => e.Time)
                    .Take(10);

                var events = await query.ToListAsync(cancellationToken);

                if (events.Count == 0)
                {
                    // As a helpful fallback for "today" queries, show next upcoming events.
                    if (rangeLabel == "bugün")
                    {
                        var upcoming = await _context.Events
                            .AsNoTracking()
                            .Include(e => e.Club)
                            .Where(e => e.Time >= nowUtc)
                            .OrderBy(e => e.Time)
                            .Take(5)
                            .ToListAsync(cancellationToken);

                        if (upcoming.Count > 0)
                            return FormatEvents("Bugün için etkinlik yok. Yaklaşan etkinlikler:", upcoming);
                    }

                    return $"{rangeLabel} için uygun bir etkinlik bulamadım.";
                }

                var header = rangeLabel switch
                {
                    "bugün" => "Bugünkü etkinlikler:",
                    "yarın" => "Yarınki etkinlikler:",
                    _ => $"{rangeLabel} etkinlikleri:"
                };

                // If this was a specific time query, adjust header for clarity.
                if (rangeLabel.Contains("saat", StringComparison.OrdinalIgnoreCase) || rangeLabel.Contains(":", StringComparison.OrdinalIgnoreCase))
                    header = $"{rangeLabel} civarı etkinlikler:";

                return FormatEvents(header, events);
            }

            if (LooksLikeAvailableRoomsQuery(text))
            {
                var nowInTimeZone = GetNowInAppTimeZone();
                var (startLocal, endLocal, label, needsClarification) = ParseRoomAvailabilityRange(text, nowInTimeZone);
                var startUtc = ToUtc(startLocal);
                var endUtc = ToUtc(endLocal);

                if (needsClarification)
                    return "Hangi saat aralığında oda arıyorsun? Örn: 'bugün 14:00-16:00 boş oda var mı?'";

                var requireProjector = text.Contains("projeksiyon", StringComparison.OrdinalIgnoreCase) || text.Contains("projector", StringComparison.OrdinalIgnoreCase);
                var minCapacity = TryParseMinCapacity(text);

                var roomsQuery = _context.Rooms.AsNoTracking().AsQueryable();
                if (requireProjector)
                    roomsQuery = roomsQuery.Where(r => r.HasProjector);
                if (minCapacity.HasValue)
                    roomsQuery = roomsQuery.Where(r => r.Capacity >= minCapacity.Value);

                var rooms = await roomsQuery
                    .OrderBy(r => r.Name)
                    .Take(50)
                    .ToListAsync(cancellationToken);

                if (rooms.Count == 0)
                    return "Kriterlere uyan oda bulamadım.";

                // Availability check - small dataset expected.
                var available = new List<string>();
                foreach (var room in rooms)
                {
                    var isOccupied = await _context.Reservations
                        .AsNoTracking()
                        .AnyAsync(r =>
                            r.RoomId == room.Id &&
                            r.Status != ReservationStatus.Rejected &&
                            (r.StartTime < endUtc && r.EndTime > startUtc),
                            cancellationToken);

                    if (!isOccupied)
                        available.Add($"- {room.Name} (Kapasite: {room.Capacity}, Konum: {room.Location})");

                    if (available.Count >= 10)
                        break;
                }

                if (available.Count == 0)
                    return $"{label} aralığında müsait oda bulamadım.";

                return $"{label} aralığında müsait odalar:\n" + string.Join("\n", available);
            }

            if (LooksLikeReservationQuery(text))
            {
                if (LooksLikeMyReservationsQuery(text))
                {
                    if (!userId.HasValue)
                        return "Rezervasyonlarını görmek için giriş yapman gerekiyor.";

                    var nowInTimeZone = GetNowInAppTimeZone();
                    var (rangeStartLocal, rangeEndLocal, rangeLabel, _) = ParseDateTimeRange(text, nowInTimeZone);
                    var rangeStartUtc = ToUtc(rangeStartLocal);
                    var rangeEndUtc = ToUtc(rangeEndLocal);
                    var nowUtc = ToUtc(nowInTimeZone);
                    // If user didn't specify a time, ParseDateTimeRange defaults to a whole-day range for today.
                    // For "rezervasyonlarım" with no day keyword, we prefer upcoming reservations.
                    var hasExplicitDay = text.Contains("bugün") || text.Contains("yarın") || Regex.IsMatch(text, @"\b\d{1,2}[./-]\d{1,2}");

                    IQueryable<Domain.Entity.Reservation> query = _context.Reservations
                        .AsNoTracking()
                        .Include(r => r.Room)
                        .Where(r => r.UserId == userId.Value);

                    if (hasExplicitDay)
                        query = query.Where(r => r.StartTime >= rangeStartUtc && r.StartTime < rangeEndUtc);
                    else
                        query = query.Where(r => r.EndTime >= nowUtc);

                    var items = await query
                        .OrderBy(r => r.StartTime)
                        .Take(10)
                        .ToListAsync(cancellationToken);

                    if (items.Count == 0)
                        return hasExplicitDay ? $"{rangeLabel} için rezervasyonun görünmüyor." : "Yaklaşan rezervasyonun görünmüyor.";

                    var header = hasExplicitDay ? $"{rangeLabel} rezervasyonların:" : "Yaklaşan rezervasyonların:";
                    var lines = items.Select(r => $"- {r.Room?.Name ?? "Oda"} | {r.StartTime:dd.MM HH:mm}-{r.EndTime:HH:mm} | Durum: {r.Status}");
                    return header + "\n" + string.Join("\n", lines);
                }

                if (text.Contains("onay bekleyen", StringComparison.OrdinalIgnoreCase) || text.Contains("pending", StringComparison.OrdinalIgnoreCase))
                {
                    var pending = await _context.Reservations
                        .AsNoTracking()
                        .Include(r => r.Room)
                        .Where(r => r.Status == ReservationStatus.Pending)
                        .OrderBy(r => r.StartTime)
                        .Take(10)
                        .ToListAsync(cancellationToken);

                    if (pending.Count == 0)
                        return "Onay bekleyen rezervasyon görünmüyor.";

                    var lines = pending.Select(r => $"- {r.Room?.Name ?? "Oda"} | {r.StartTime:dd.MM HH:mm}-{r.EndTime:HH:mm}");
                    return "Onay bekleyen rezervasyonlar:\n" + string.Join("\n", lines);
                }

                // For generic reservation questions, ask for more detail.
                return "Rezervasyon için hangi oda ve hangi saat aralığı? Örn: 'Konferans Salonu A bugün 14:00-16:00 dolu mu?'";
            }

            return null;
        }

        private async Task<string?> RecommendClubFromDatabaseAsync(string normalizedMessage, int? userId, CancellationToken cancellationToken)
        {
            var clubs = await _context.Clubs
                .AsNoTracking()
                .Include(c => c.Category)
                .Select(c => new
                {
                    c.Id,
                    c.Name,
                    c.Description,
                    CategoryName = c.Category != null ? c.Category.Name : string.Empty
                })
                .OrderBy(c => c.Name)
                .ToListAsync(cancellationToken);

            if (clubs.Count == 0)
                return "Şu an sistemde kayıtlı kulüp görünmüyor.";

            var requestedArchitecture = normalizedMessage.Contains("mimarl", StringComparison.OrdinalIgnoreCase) ||
                                        normalizedMessage.Contains("architecture", StringComparison.OrdinalIgnoreCase);

            var architectureKeywords = new[]
            {
                "mimarl", "tasarım", "tasarim", "iç mimar", "ic mimar", "şehir", "sehir", "plan", "maket", "3d", "render"
            };

            bool AnyMatch(string? value)
            {
                if (string.IsNullOrWhiteSpace(value)) return false;
                var v = Normalize(value);
                return architectureKeywords.Any(k => v.Contains(k, StringComparison.OrdinalIgnoreCase));
            }

            var hasAnyArchitectureClub = clubs.Any(c =>
                AnyMatch(c.Name) || AnyMatch(c.Description) || AnyMatch(c.CategoryName));

            string? department = null;
            if (userId.HasValue)
            {
                department = await _context.Users
                    .AsNoTracking()
                    .Where(u => u.Id == userId.Value)
                    .Select(u => u.Department)
                    .FirstOrDefaultAsync(cancellationToken);
            }

            var keywords = BuildRecommendationKeywords(normalizedMessage, department);

            int Score(string? value, int weight)
            {
                if (string.IsNullOrWhiteSpace(value))
                    return 0;

                var v = Normalize(value);
                var score = 0;
                foreach (var k in keywords)
                {
                    if (k.Length < 3)
                        continue;

                    if (v.Contains(k, StringComparison.OrdinalIgnoreCase))
                        score += weight;
                }

                return score;
            }

            var ranked = clubs
                .Select(c => new
                {
                    Club = c,
                    Score = Score(c.Name, 5) + Score(c.CategoryName, 3) + Score(c.Description, 2)
                })
                .OrderByDescending(x => x.Score)
                .ThenBy(x => x.Club.Name)
                .ToList();

            var best = ranked[0];
            var clubName = best.Club.Name;
            var categoryPart = string.IsNullOrWhiteSpace(best.Club.CategoryName) ? "" : $" (Kategori: {best.Club.CategoryName})";

            if (requestedArchitecture && !hasAnyArchitectureClub)
            {
                return $"Not: Şu an veritabanında 'mimarlık/tasarım' ile doğrudan eşleşen bir kulüp göremedim.\n\nMevcut kulüpler arasından önerim: {clubName}{categoryPart}.\n\nİstersen ilgi alanını daha da aç (örn: 'tasarım', 'şehir planlama', '3D modelleme') ya da yöneticiden ilgili kulübün sisteme eklenmesini isteyebilirsin.";
            }

            // Low scores are often caused by generic words (e.g., "öğrenci") in descriptions.
            // In that case, still suggest an existing club but ask for a clearer interest area.
            if (best.Score < 5)
            {
                return $"Veritabanındaki kulüpler arasından önerim: {clubName}{categoryPart}.\n\nİstersen ilgi alanını biraz daha net yaz (örn: 'tasarım', 'şehir planlama', '3D modelleme') daha iyi eşleştireyim.";
            }

            return $"Veritabanındaki kulüpler arasından önerim: {clubName}{categoryPart}.\n\nKısa not: Mesajındaki ilgi alanlarına en çok uyan kulüp bu göründü.";
        }

        private static bool LooksLikeClubRecommendationQuery(string text)
        {
            // Examples: "kulüp öner", "bana bir kulüp tavsiye et", "hangi kulübe katılayım"
            if (!(text.Contains("kulüp") || text.Contains("kulup") || text.Contains("topluluk") || text.Contains("club")))
                return false;

            return text.Contains("öner") || text.Contains("oner") || text.Contains("tavsiye") || text.Contains("hangi") || text.Contains("katılay") || text.Contains("katil");
        }

        private static IReadOnlyList<string> BuildRecommendationKeywords(string normalizedMessage, string? department)
        {
            // Keep it simple and deterministic; we only use these for matching against DB fields.
            var stopWords = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
            {
                "bir","biri","kulüp","kulup","topluluk","club","öner","oner","öneri","tavsiye",
                "bana","ben","benim","için","icin","olan","bu","şu","su","okul","okulda","bulunan",
                "mısın","misin","lütfen","lutfen","hangi","katılayım","katilayim","katılmak","katilmak",
                "öğrenci","ogrenci","öğrencisi","ogrencisi","öğrenciler","ogrenciler",
                "üniversite","universite","kampüs","kampus","sistem","kayıtlı","kayitli"
            };

            var raw = normalizedMessage;
            if (!string.IsNullOrWhiteSpace(department))
                raw += " " + Normalize(department);

            var words = Regex
                .Split(raw, @"[^\p{L}\p{N}]+")
                .Where(w => !string.IsNullOrWhiteSpace(w))
                .Select(w => w.Trim())
                .Where(w => w.Length >= 2)
                .Where(w => !stopWords.Contains(w))
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .ToList();

            // Domain-specific boosts
            if (raw.Contains("mimarl", StringComparison.OrdinalIgnoreCase) || raw.Contains("architecture", StringComparison.OrdinalIgnoreCase))
            {
                words.AddRange(new[] { "mimarl", "tasarım", "tasarim", "iç mimar", "ic mimar", "şehir", "sehir", "plan", "maket", "3d", "render" });
            }

            return words
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .OrderBy(w => w, StringComparer.OrdinalIgnoreCase)
                .ToList();
        }

        private static string Normalize(string input)
        {
            // Turkish-aware lowercase to make matching more reliable.
            return input.Trim().ToLower(new CultureInfo("tr-TR"));
        }

        private static bool LooksLikeEventQuery(string text)
        {
            // If user asks for recommendations ("etkinlik öner"), do not treat it as a date/list query.
            if (LooksLikeRecommendationIntent(text))
                return false;

            // "Bu hafta ne var?" gibi, etkinlik kelimesi geçmese bile kampüs haftalık aktiviteleri soruları.
            if (text.Contains("bu hafta") && (text.Contains("ne var") || text.Contains("neler var") || text.Contains("hangi") || text.Contains("kampüs") || text.Contains("kampus")))
                return true;

            return text.Contains("etkinlik") || text.Contains("event") || text.Contains("semin") || text.Contains("konfer") || text.Contains("hangi etkinlik");
        }

        private static bool LooksLikeRecommendationIntent(string text)
        {
            // Covers: "öner", "tavsiye", "recommend" etc.
            return text.Contains("öner") || text.Contains("oner") || text.Contains("tavsiye") || text.Contains("recommend") || text.Contains("önerir") || text.Contains("onerir");
        }

        private static bool LooksLikeAvailableRoomsQuery(string text)
        {
            if (!(text.Contains("oda") || text.Contains("salon") || text.Contains("room")))
                return false;

            return text.Contains("boş") || text.Contains("müsait") || text.Contains("uygun") || text.Contains("available") || text.Contains("dolu mu");
        }

        private static bool LooksLikeReservationQuery(string text)
        {
            return text.Contains("rezerv") || text.Contains("reservation");
        }

        private static bool LooksLikeMyReservationsQuery(string text)
        {
            // Handles: "rezervasyonlarım", "rezervasyonlarim", "benim rezervasyonlar".
            if (!text.Contains("rezervasyon"))
                return false;

            if (text.Contains("benim"))
                return true;

            if (text.Contains("rezervasyonlarım") || text.Contains("rezervasyonlarim"))
                return true;

            return text.Contains("rezervasyonlar");
        }

        private static int? TryParseMinCapacity(string text)
        {
            // Very small heuristic: "en az 50" or "50 kişi".
            var match = Regex.Match(text, @"\b(\d{2,4})\b");
            if (!match.Success)
                return null;

            if (!int.TryParse(match.Groups[1].Value, out var number))
                return null;

            if (text.Contains("kişi") || text.Contains("kapas") || text.Contains("en az"))
                return number;

            return null;
        }

        private static (DateTime rangeStart, DateTime rangeEnd, string label, bool needsClarification) ParseDateTimeRange(string text, DateTime now)
        {
            // Default: today.
            var day = now.Date;
            var labelDay = "bugün";

            // Week queries
            if (text.Contains("bu hafta"))
            {
                var startOfWeek = StartOfWeek(now.Date, DayOfWeek.Monday);
                var endOfWeek = startOfWeek.AddDays(7);
                return (startOfWeek, endOfWeek, "bu hafta", false);
            }

            if (text.Contains("yarın"))
            {
                day = now.Date.AddDays(1);
                labelDay = "yarın";
            }
            else if (TryParseDate(text, now, out var parsedDay, out var parsedLabel))
            {
                day = parsedDay;
                labelDay = parsedLabel;
            }

            var times = ExtractTimes(text).ToList();

            if (times.Count == 0)
            {
                // No time: whole day.
                var start = day;
                var end = day.AddDays(1);
                return (start, end, labelDay, false);
            }

            var startTime = ResolveHourAmbiguity(times[0], text, now, day);
            var startDt = day.Add(startTime);

            // For single time queries, use a small window.
            var endDt = startDt.AddHours(2);
            if (endDt.Date != startDt.Date)
                endDt = startDt.Date.AddDays(1);

            var label = $"{labelDay} saat {startDt:HH:mm}";
            return (startDt, endDt, label, false);
        }

        private static DateTime StartOfWeek(DateTime date, DayOfWeek startOfWeek)
        {
            var diff = (7 + (date.DayOfWeek - startOfWeek)) % 7;
            return date.AddDays(-diff).Date;
        }

        private static (DateTime start, DateTime end, string label, bool needsClarification) ParseRoomAvailabilityRange(string text, DateTime now)
        {
            var day = now.Date;
            var labelDay = "bugün";

            if (text.Contains("yarın"))
            {
                day = now.Date.AddDays(1);
                labelDay = "yarın";
            }
            else if (TryParseDate(text, now, out var parsedDay, out var parsedLabel))
            {
                day = parsedDay;
                labelDay = parsedLabel;
            }

            var times = ExtractTimes(text).ToList();
            if (times.Count == 0)
                return (default, default, string.Empty, true);

            var startTime = ResolveHourAmbiguity(times[0], text, now, day);
            var start = day.Add(startTime);

            DateTime end;
            if (times.Count >= 2)
            {
                var endTime = ResolveHourAmbiguity(times[1], text, now, day);
                end = day.Add(endTime);
                if (end <= start)
                    end = start.AddHours(1);
            }
            else
            {
                end = start.AddHours(2);
            }

            var label = $"{labelDay} {start:HH:mm}-{end:HH:mm}";
            return (start, end, label, false);
        }

        private static bool TryParseDate(string text, DateTime now, out DateTime day, out string label)
        {
            day = default;
            label = string.Empty;

            // 1) Numeric dates: 27.12, 27/12, 27-12-2025
            var numericMatch = Regex.Match(text, @"\b(\d{1,2})[./-](\d{1,2})(?:[./-](\d{2,4}))?\b");
            if (numericMatch.Success)
            {
                if (!int.TryParse(numericMatch.Groups[1].Value, out var dd) || !int.TryParse(numericMatch.Groups[2].Value, out var mm))
                    return false;

                var hasYear = false;
                var yyyy = now.Year;
                if (numericMatch.Groups[3].Success && int.TryParse(numericMatch.Groups[3].Value, out var y))
                {
                    hasYear = true;
                    yyyy = y < 100 ? 2000 + y : y;
                }

                return TryBuildDate(now, dd, mm, hasYear, yyyy, out day, out label);
            }

            // 2) Turkish month names: "27 aralık", "27 aralik 2025".
            // Note: input is already normalized with tr-TR lowercase.
            var monthMatch = Regex.Match(
                text,
                @"\b(\d{1,2})\s*(ocak|şubat|subat|mart|nisan|mayıs|mayis|haziran|temmuz|ağustos|agustos|eylül|eylul|ekim|kasım|kasim|aralık|aralik)(?:['’]?(?:ta|te|da|de))?\b(?:\s*(\d{4}))?\b");

            if (!monthMatch.Success)
                return false;

            if (!int.TryParse(monthMatch.Groups[1].Value, out var dayOfMonth))
                return false;

            var monthText = monthMatch.Groups[2].Value;
            var monthNumber = monthText switch
            {
                "ocak" => 1,
                "şubat" or "subat" => 2,
                "mart" => 3,
                "nisan" => 4,
                "mayıs" or "mayis" => 5,
                "haziran" => 6,
                "temmuz" => 7,
                "ağustos" or "agustos" => 8,
                "eylül" or "eylul" => 9,
                "ekim" => 10,
                "kasım" or "kasim" => 11,
                "aralık" or "aralik" => 12,
                _ => 0
            };

            if (monthNumber == 0)
                return false;

            var hasExplicitYear = false;
            var year = now.Year;
            if (monthMatch.Groups[3].Success && int.TryParse(monthMatch.Groups[3].Value, out var explicitYear))
            {
                hasExplicitYear = true;
                year = explicitYear;
            }

            return TryBuildDate(now, dayOfMonth, monthNumber, hasExplicitYear, year, out day, out label);
        }

        private static bool TryBuildDate(DateTime now, int dd, int mm, bool hasExplicitYear, int yyyy, out DateTime day, out string label)
        {
            day = default;
            label = string.Empty;

            try
            {
                var candidate = new DateTime(yyyy, mm, dd);

                // If year is omitted and date already passed, assume next year.
                if (!hasExplicitYear && candidate.Date < now.Date)
                    candidate = candidate.AddYears(1);

                day = candidate;
                label = day.ToString("dd.MM.yyyy", CultureInfo.InvariantCulture);
                return true;
            }
            catch
            {
                return false;
            }
        }

        private DateTime GetNowInAppTimeZone()
        {
            // Return an Unspecified-kind DateTime representing "now" in the app time zone.
            // We keep it Unspecified so downstream parsing via .Date/.Add(...) stays consistent.
            var nowInTimeZone = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, _appTimeZone);
            return DateTime.SpecifyKind(nowInTimeZone, DateTimeKind.Unspecified);
        }

        private DateTime ToUtc(DateTime value)
        {
            if (value.Kind == DateTimeKind.Utc)
                return value;

            if (value.Kind == DateTimeKind.Local)
                return value.ToUniversalTime();

            // Treat "unspecified" as wall-clock time in the configured app time zone.
            return TimeZoneInfo.ConvertTimeToUtc(value, _appTimeZone);
        }

        private static TimeZoneInfo ResolveTimeZone(string? timeZoneId)
        {
            // Prefer a stable app timezone so "bugün/yarın" queries don't depend on server OS timezone.
            // Cross-platform IDs: Linux/macOS uses IANA (e.g. "Europe/Istanbul"), Windows uses "Turkey Standard Time".
            var candidates = new List<string>();

            if (!string.IsNullOrWhiteSpace(timeZoneId))
                candidates.Add(timeZoneId);

            candidates.Add("Europe/Istanbul");
            candidates.Add("Turkey Standard Time");

            foreach (var candidate in candidates)
            {
                try
                {
                    return TimeZoneInfo.FindSystemTimeZoneById(candidate);
                }
                catch
                {
                    // ignore and try next
                }
            }

            return TimeZoneInfo.Local;
        }

        private static IEnumerable<TimeSpan> ExtractTimes(string text)
        {
            // Matches HH:mm or "saat 15" style.
            foreach (Match match in Regex.Matches(text, @"\b(\d{1,2})[:.](\d{2})\b"))
            {
                if (int.TryParse(match.Groups[1].Value, out var h) && int.TryParse(match.Groups[2].Value, out var m) && h is >= 0 and <= 23 && m is >= 0 and <= 59)
                    yield return new TimeSpan(h, m, 0);
            }

            foreach (Match match in Regex.Matches(text, @"\bsaat\s*(\d{1,2})\b"))
            {
                if (int.TryParse(match.Groups[1].Value, out var h) && h is >= 0 and <= 23)
                    yield return new TimeSpan(h, 0, 0);
            }
        }

        private static TimeSpan ResolveHourAmbiguity(TimeSpan time, string text, DateTime now, DateTime day)
        {
            // If we already have minutes, assume it's unambiguous.
            if (time.Minutes != 0)
                return time;

            var hour = time.Hours;

            if (text.Contains("akşam") || text.Contains("öğleden sonra") || text.Contains("gece"))
            {
                if (hour < 12)
                    hour += 12;
                return new TimeSpan(hour, 0, 0);
            }

            if (text.Contains("sabah"))
                return new TimeSpan(hour, 0, 0);

            // Heuristic: for today, choose the closest future between H and H+12.
            if (day.Date == now.Date && hour <= 12)
            {
                var candidate1 = day.AddHours(hour);
                var candidate2 = day.AddHours((hour + 12) % 24);

                var c1Future = candidate1 >= now;
                var c2Future = candidate2 >= now;

                if (c1Future && c2Future)
                    return (candidate1 - now) <= (candidate2 - now) ? new TimeSpan(candidate1.Hour, 0, 0) : new TimeSpan(candidate2.Hour, 0, 0);

                if (c2Future)
                    return new TimeSpan(candidate2.Hour, 0, 0);

                return new TimeSpan(candidate1.Hour, 0, 0);
            }

            // Otherwise, assume afternoon for small hours (common "saat 3" => 15:00)
            if (hour is >= 1 and <= 7)
                hour += 12;

            return new TimeSpan(hour, 0, 0);
        }

        private static string FormatEvents(string header, IEnumerable<dynamic> events)
        {
            var lines = new List<string> { header };

            foreach (var e in events)
            {
                var clubName = "";
                try
                {
                    clubName = e.Club?.Name ?? "";
                }
                catch
                {
                    // ignore
                }

                var clubSuffix = string.IsNullOrWhiteSpace(clubName) ? "" : $" | Kulüp: {clubName}";
                lines.Add($"- {e.Time:dd.MM HH:mm} | {e.Title} | {e.Location}{clubSuffix}");
            }

            return string.Join("\n", lines);
        }
    }
}
