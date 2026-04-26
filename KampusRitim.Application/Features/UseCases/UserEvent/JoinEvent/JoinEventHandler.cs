using KampusRitim.Application.Interfaces; // ICurrentUserService
using KampusRitim.Application.Interfaces.Repositories;
using KampusRitim.Application.UseCases.UserEvent.JoinEvent;
using KampusRitim.Domain.Entity;
using MediatR;

namespace KampusRitim.Application.UseCases.UserEvent.JoinEvent
{
    public class JoinEventHandler : IRequestHandler<JoinEventRequest, JoinEventResponse>
    {
        private readonly IUserEventRepository _userEventRepo;
        private readonly IEventRepository _eventRepo;
        private readonly ICurrentUserService _currentUserService;

        public JoinEventHandler(IUserEventRepository userEventRepo, IEventRepository eventRepo, ICurrentUserService currentUserService)
        {
            _userEventRepo = userEventRepo;
            _eventRepo = eventRepo;
            _currentUserService = currentUserService;
        }

        public async Task<JoinEventResponse> Handle(JoinEventRequest request, CancellationToken cancellationToken)
        {
            // 1. Kullanıcı Kim?
            var userId = _currentUserService.UserId;
            if (userId == null) return new JoinEventResponse { IsSuccess = false, Message = "Oturum bulunamadı." };

            // 2. Etkinlik Var mı? (Kontenjan kontrolü için katılımcılarla beraber çekiyoruz)
            var eventEntity = await _eventRepo.GetByIdWithParticipantsAsync(request.EventId);

            if (eventEntity == null)
            {
                return new JoinEventResponse { IsSuccess = false, Message = "Etkinlik bulunamadı." };
            }

            // 3. Tarih Kontrolü
            if (eventEntity.Time < DateTime.Now)
            {
                return new JoinEventResponse { IsSuccess = false, Message = "Geçmiş tarihli bir etkinliğe katılamazsınız." };
            }

            // 4. Kontenjan Kontrolü
            if (eventEntity.UserEvents != null && eventEntity.UserEvents.Count >= eventEntity.Quota)
            {
                return new JoinEventResponse { IsSuccess = false, Message = "Etkinlik kontenjanı dolmuştur." };
            }

            // 5. Mükerrer Kayıt Kontrolü
            var existing = await _userEventRepo.GetAsync(userId.Value, request.EventId);
            if (existing != null)
            {
                return new JoinEventResponse { IsSuccess = false, Message = "Zaten bu etkinliğe kayıtlısınız." };
            }

            // 6. KAYIT (JoinedAt Dahil)
            var userEvent = new KampusRitim.Domain.Entity.UserEvent
            {
                UserId = userId.Value,
                EventId = request.EventId,
                JoinedAt = DateTime.UtcNow // Şu anki zamanı kaydediyoruz
            };

            await _userEventRepo.AddAsync(userEvent);

            return new JoinEventResponse
            { 
                IsSuccess = true, 
                Message = "Etkinliğe başarıyla katıldınız."
            };
        }
    }
}