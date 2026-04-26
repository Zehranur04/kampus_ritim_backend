using KampusRitim.Application.Features.Dtos;
using KampusRitim.Application.Interfaces.Repositories;
using MediatR;

namespace KampusRitim.Application.UseCases.Event.GetEventById
{
    public class GetEventByIdHandler : IRequestHandler<GetEventByIdRequest, GetEventByIdResponse>
    {
        private readonly IEventRepository _eventRepo;

        public GetEventByIdHandler(IEventRepository eventRepo)
        {
            _eventRepo = eventRepo;
        }

        public async Task<GetEventByIdResponse> Handle(GetEventByIdRequest request, CancellationToken cancellationToken)
        {
            // 1. Repository'den ID ve Speaker detaylı veriyi çekiyoruz
            // (Repository'de bu metodun tanımlı olduğundan emin ol)
            var existingEvent = await _eventRepo.GetByIdWithSpeakerAsync(request.Id);

            // 2. Kontrol: Etkinlik var mı?
            if (existingEvent == null)
            {
                return new GetEventByIdResponse
                {
                    IsSuccess = false,
                    Message = $"Aradığınız {request.Id} numaralı etkinlik bulunamadı.",
                    Event = null
                };
            }

            // 3. Mapping: Entity -> Sade EventDto
            var eventDto = new EventDto
            {
                Id = existingEvent.Id,
                Title = existingEvent.Title,
                Description = existingEvent.Description,
                Time = existingEvent.Time,
                Location = existingEvent.Location,
                Quota = existingEvent.Quota,
                CertificateDetails = existingEvent.CertificateDetails,

                // Konuşmacı bilgisini sadeleştirerek isme çeviriyoruz
                SpeakerName = existingEvent.Speaker != null
                    ? $"{existingEvent.Speaker.Name} {existingEvent.Speaker.Surname}"
                    : "Belirtilmemiş"
            };

            return new GetEventByIdResponse
            {
                IsSuccess = true,
                Message = "Etkinlik detayları getirildi.",
                Event = eventDto
            };
        }
    }
}
