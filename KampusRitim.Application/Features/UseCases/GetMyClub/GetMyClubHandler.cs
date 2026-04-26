using KampusRitim.Application.Features.Dtos;
using KampusRitim.Application.Interfaces;
using KampusRitim.Application.Interfaces.Repositories;
using MediatR;

namespace KampusRitim.Application.Features.UseCases.GetMyClub
{
    public class GetMyClubHandler : IRequestHandler<GetMyClubRequest, GetMyClubResponse>
    {
        private readonly IUserClubRepository _userClubRepo;
        private readonly ICurrentUserService _currentUserService;

        public GetMyClubHandler(IUserClubRepository userClubRepo, ICurrentUserService currentUserService)
        {
            _userClubRepo = userClubRepo;
            _currentUserService = currentUserService;
        }

        public async Task<GetMyClubResponse> Handle(GetMyClubRequest request, CancellationToken cancellationToken)
        {
            var userId = _currentUserService.UserId;

            if (userId == null)
                return new GetMyClubResponse { IsSuccess = false, Message = "Oturum bulunamadı." };

            // Repository'den veriyi çekiyoruz
            var memberships = await _userClubRepo.GetClubsForUserAsync(userId.Value);

            // DTO'ya map'leme işlemi
            var clubList = memberships.Select(m => new MyClubDto
            {
                ClubId = m.ClubId,
                ClubName = m.Club.Name, // Include sayesinde dolu gelir
                ClubDescription = m.Club.Description,
                ClubProfileImageUrl = m.Club.ProfileImageUrl,
                ClubRole = m.ClubRole,
                JoinDate = m.JoinDate
            }).ToList();

            return new GetMyClubResponse
            {
                IsSuccess = true,
                Message = "Kulüpleriniz başarıyla listelendi.",
                Clubs = clubList
            };
        }
    }
}
