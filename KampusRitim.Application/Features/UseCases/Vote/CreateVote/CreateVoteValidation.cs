using FluentValidation;

namespace KampusRitim.Application.Features.UseCases.Vote.CreateVote
{
    public class CreateVoteValidation : AbstractValidator<CreateVoteRequest>
    {
        public CreateVoteValidation()
        {
            RuleFor(x => x.EventId)
                .GreaterThan(0).WithMessage("Geçerli bir etkinlik seçmelisiniz.");

            RuleFor(x => x.Score)
                .InclusiveBetween(1, 5).WithMessage("Puan 1 ile 5 arasında olmalıdır.");
        }
    }
}
