using KampusRitim.Application.Interfaces;
using KampusRitim.Application.Interfaces.Repositories;
using DomainUser = KampusRitim.Domain.Entity.User;
using KampusRitim.Domain.Enums;
using KampusRitim.Domain.Events;
using MediatR;

namespace KampusRitim.Application.UseCases.Event.CreateEvent
{
    public class CreateEventHandler : IRequestHandler<CreateEventRequest, CreateEventResponse>
    {
        private readonly IEventRepository _eventRepo;
        private readonly ISpeakerRepository _speakerRepo;
        private readonly IUserRepository _userRepo;
        private readonly ICurrentUserService _currentUser;
        private readonly IMediator _mediator;

        public CreateEventHandler(IEventRepository eventRepo, ISpeakerRepository speakerRepo, IUserRepository userRepo, ICurrentUserService currentUser, IMediator mediator)
        {
            _eventRepo = eventRepo;
            _speakerRepo = speakerRepo;
            _userRepo = userRepo;
            _currentUser = currentUser;
            _mediator = mediator;
        }

        public async Task<CreateEventResponse> Handle(CreateEventRequest request, CancellationToken cancellationToken)
        {
            var response = new CreateEventResponse();

            DomainUser? user = null;
            if (_currentUser.UserId.HasValue)
            {
                user = await _userRepo.GetByIdAsync(_currentUser.UserId.Value);
            }
            else if (!string.IsNullOrWhiteSpace(request.UserEmail))
            {
                user = await _userRepo.GetByEmailAsync(request.UserEmail);
            }

            if (user == null)
            {
                response.IsSuccess = false;
                response.Message = "Kullanıcı bulunamadı (giriş yapmanız gerekiyor olabilir).";
                return response;
            }

            if (user.Role != UserSystemRole.SystemManager)
            {
                response.IsSuccess = false;
                response.Message = "YETKİSİZ İŞLEM! Etkinlik oluşturma yetkisine sahip değilsiniz.";
                return response;
            }

            // 1. ADIM: Önce Konuşmacıyı Oluştur
            var newSpeaker = new KampusRitim.Domain.Entity.Speaker
            {
                Name = request.SpeakerName,
                Surname = request.SpeakerSurname,
                Bio = request.SpeakerBio,
                Title = request.SpeakerTitle,
                ProfileImageUrl = request.SpeakerImageUrl
            };

            var createdSpeaker = await _speakerRepo.AddAsync(newSpeaker);

            // 2. ADIM: Etkinliği Oluştur
            var newEvent = new KampusRitim.Domain.Entity.Event
            {
                Title = request.Title,
                Description = request.Description,
                Time = request.Time,
                Location = request.Location,
                Quota = request.Quota,
                CertificateDetails = request.CertificateDetails,
                SpeakerId = createdSpeaker.Id,
                CategoryId = request.CategoryId,

                // DİKKAT: Bildirim sisteminin çalışması için bu Etkinliğin
                // hangi kulübe ait olduğunu bilmemiz şart. Request'ten alıyoruz.
                ClubId = request.ClubId
            };

            // 3. ADIM: Etkinliği Kaydet
            var createdEvent = await _eventRepo.AddAsync(newEvent);

            // ---------------------------------------------------------------------
            // 4. ADIM (YENİ): Bildirim Sistemini Tetikle! 🔔
            // ---------------------------------------------------------------------
            // Kayıt başarılı oldu, şimdi "EventCreatedDomainEvent" paketini fırlatıyoruz.
            // Bunu yakalayan diğer Handler (EventCreatedEventHandler) üyelere bildirim atacak.

            await _mediator.Publish(new EventCreatedDomainEvent(
                createdEvent.Id,
                createdEvent.Title,
                request.ClubId,     // Hangi kulüp?
                request.ClubName ?? string.Empty   // Kulüp Adı (yoksa boş)
            ), cancellationToken);

            response.IsSuccess = true;
            response.Message = "Etkinlik oluşturuldu ve üyelere bildirim gönderildi.";
            response.EventId = createdEvent.Id;
            return response;
        }
    }
}