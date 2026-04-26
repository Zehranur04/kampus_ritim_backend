namespace KampusRitim.Application.Features.UseCases.Vote.CreateVote
{
    public class CreateVoteResponse
    {
        public int VoteId { get; set; }
        public bool IsSuccess { get; set; }
        public string Message { get; set; } = string.Empty;
    }
}
