using MediatR;

namespace KampusRitim.Application.UseCases.Profile.DeleteProfile
{
    public class DeleteProfileRequest : IRequest<DeleteProfileResponse>
    {
        public int Id { get; set; }
    }
}