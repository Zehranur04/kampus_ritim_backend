using KampusRitim.Application.Features.Dtos;
using KampusRitim.Application.Interfaces.Repositories;
using MediatR;

namespace KampusRitim.Application.UseCases.Event.GetAllEvent
{
    public class GetAllEventHandler : IRequestHandler<GetAllEventRequest, GetAllEventResponse>
    {
        private readonly IEventRepository _eventRepo;

        public GetAllEventHandler(IEventRepository eventRepo)
        {
            _eventRepo = eventRepo;
        }

        public async Task<GetAllEventResponse> Handle(GetAllEventRequest request, CancellationToken cancellationToken)
        {
            // 1. Veritabanından etkinlikleri çek -- var events = await _eventRepo.GetAllAsync();

            var events = await _eventRepo.GetAllAsync(request.SearchTerm, request.CategoryId);

            // 2. Mapping: Entity -> EventDto
            var eventDtos = events.Select(e => new EventDto
            {
                Id = e.Id,
                Title = e.Title,
                Description = e.Description,
                Time = e.Time,
                Location = e.Location,
                Quota = e.Quota,
                CertificateDetails = e.CertificateDetails,

                // Konuşmacı Adı: Eğer Speaker null değilse adını al, yoksa "Belirtilmemiş" veya null dön.
                SpeakerName = e.Speaker != null ? $"{e.Speaker.Name} {e.Speaker.Surname}" : null
            }).ToList();

            // 3. Response Dön
            return new GetAllEventResponse
            {
                IsSuccess = true,
                Message = "Etkinlikler başarıyla listelendi.",
                Events = eventDtos
            };
        }
    }
}