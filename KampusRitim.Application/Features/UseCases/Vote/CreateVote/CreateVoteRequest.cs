using MediatR;

namespace KampusRitim.Application.Features.UseCases.Vote.CreateVote
{
    public class CreateVoteRequest : IRequest<CreateVoteResponse>
    {
        public int EventId { get; set; }
        public int Score { get; set; } // 1-5 arası puan
    }
 
}
