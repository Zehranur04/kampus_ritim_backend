using FluentValidation;

namespace KampusRitim.Application.UseCase.Club.GetAllClub
{
    public class GetAllClubValidator : AbstractValidator<GetAllClubRequest>
    {
        public GetAllClubValidator()
        {
            // Doğrulanacak bir input olmadığı için burası boş.
        }
    }
}