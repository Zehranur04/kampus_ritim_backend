using KampusRitim.Application.Interfaces.Repositories;
using MediatR;

namespace KampusRitim.Application.Features.UseCases.Vote.GetEventRating
{
    public class GetEventRatingQueryHandler : IRequestHandler<GetEventRatingRequest, GetEventRatingResponse>
    {
        private readonly IVoteRepository _voteRepository;

        public GetEventRatingQueryHandler(IVoteRepository voteRepository)
        {
            _voteRepository = voteRepository;
        }

        public async Task<GetEventRatingResponse> Handle(GetEventRatingRequest request, CancellationToken cancellationToken)
        {
            // 1. Olayla ilgili tüm oyları getir
            var votes = await _voteRepository.GetVotesByEventIdAsync(request.EventId);

            // 2. Hiç oy yoksa 0 dön
            if (votes == null || !votes.Any())
            {
                return new GetEventRatingResponse { AverageScore = 0, VoteCount = 0 };
            }

            // 3. Ortalamayı hesapla (Linq kullanarak)
            double average = votes.Average(v => v.Score);

            // 4. Cevabı hazırla (Virgülden sonra 1 basamak yuvarla: 4.666 -> 4.7)
            return new GetEventRatingResponse
            {
                AverageScore = Math.Round(average, 1),
                VoteCount = votes.Count()
            };
        }
    }

}
