using KampusRitim.Application.Features.Dtos;
using KampusRitim.Application.Interfaces.Repositories;
using MediatR;

namespace KampusRitim.Application.UseCases.User.GetUserByEmail
{
    public class GetUserByEmailHandler : IRequestHandler<GetUserByEmailRequest, GetUserByEmailResponse>
    {
        private readonly IUserRepository _userRepo;

        public GetUserByEmailHandler(IUserRepository userRepo)
        {
            _userRepo = userRepo;
        }

        public async Task<GetUserByEmailResponse> Handle(GetUserByEmailRequest request, CancellationToken cancellationToken)
        {
            // 1. Repository'den email ile sorgula
            var user = await _userRepo.GetByEmailAsync(request.Email);

            // 2. Kontrol: Kullanıcı var mı?
            if (user == null)
            {
                // Olumsuz Senaryo
                return new GetUserByEmailResponse
                {
                    IsSuccess = false,
                    Message = $"'{request.Email}' adresine sahip kullanıcı bulunamadı.",
                    User = null
                };
            }

            // 3. Mapping: Entity -> UserDto (Record)
            var userDto = new UserDto(
                user.Id,
                user.Name,
                user.Surname,
                user.Email,
                user.Faculty,
                user.Department,
                user.ClassLevel
            );

            // Olumlu Senaryo
            return new GetUserByEmailResponse
            {
                IsSuccess = true,
                Message = "Kullanıcı bulundu.",
                User = userDto
            };
        }
    }
}
