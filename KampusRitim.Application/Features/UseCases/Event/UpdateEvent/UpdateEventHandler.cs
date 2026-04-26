using KampusRitim.Application.Interfaces.Repositories;
using KampusRitim.Application.UseCases.Event.UpdateEvent;
using KampusRitim.Domain.Entity; // Tam yol kullanmasak da olur, aşağıda dikkat edeceğiz
using MediatR;

namespace KampusRitim.Application.UseCases.Events.UpdateEvent
{
    public class UpdateEventHandler : IRequestHandler<UpdateEventRequest, UpdateEventResponse>
    {
        private readonly IEventRepository _eventRepo;
        private readonly ISpeakerRepository _speakerRepo;

        public UpdateEventHandler(IEventRepository eventRepo, ISpeakerRepository speakerRepo)
        {
            _eventRepo = eventRepo;
            _speakerRepo = speakerRepo;
        }

        public async Task<UpdateEventResponse> Handle(UpdateEventRequest request, CancellationToken cancellationToken)
        {
            // 1. Etkinliği Bul
            var existingEvent = await _eventRepo.GetByIdAsync(request.Id);
            if (existingEvent == null)
            {
                return new UpdateEventResponse { IsSuccess = false, Message = "Etkinlik bulunamadı." };
            }

            // 2. KONUŞMACI KARAR MEKANİZMASI
            // Eğer kullanıcı "Yeni Konuşmacı Adı" girdiyse, önce onu yaratacağız.
            if (!string.IsNullOrEmpty(request.NewSpeakerName))
            {
                // A) Yeni Speaker Oluştur
                var newSpeaker = new KampusRitim.Domain.Entity.Speaker
                {
                    Name = request.NewSpeakerName,
                    Surname = request.NewSpeakerSurname ?? "", // Zorunlu alan null olmasın
                    Title = request.NewSpeakerTitle,
                    Bio = request.NewSpeakerBio,
                    ProfileImageUrl = request.NewSpeakerImageUrl
                };

                // B) Kaydet ve ID al
                var createdSpeaker = await _speakerRepo.AddAsync(newSpeaker);

                // C) Etkinliği bu YENİ konuşmacıya bağla
                existingEvent.SpeakerId = createdSpeaker.Id;
            }
            else if (request.SpeakerId.HasValue && request.SpeakerId.Value > 0)
            {
                // D) Var olan bir ID seçildiyse onu bağla
                // (Burada o ID var mı diye kontrol etmek iyi olur)
                var speakerCheck = await _speakerRepo.GetByIdAsync(request.SpeakerId.Value);
                if (speakerCheck == null)
                {
                    return new UpdateEventResponse { IsSuccess = false, Message = "Seçilen konuşmacı sistemde bulunamadı." };
                }

                existingEvent.SpeakerId = request.SpeakerId.Value;
            }

            // 3. Diğer Etkinlik Bilgilerini Güncelle
            existingEvent.Title = request.Title;
            existingEvent.Description = request.Description;
            existingEvent.Time = request.Time;
            existingEvent.Location = request.Location;
            existingEvent.Quota = request.Quota;
            existingEvent.CertificateDetails = request.CertificateDetails;

            // 4. Kaydet
            await _eventRepo.UpdateAsync(existingEvent);

            return new UpdateEventResponse
            {
                IsSuccess = true,
                Message = "Etkinlik (ve gerekiyorsa yeni konuşmacı) başarıyla güncellendi.",
                EventId = existingEvent.Id
            };
        }
    }
}