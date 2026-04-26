using KampusRitim.Application.Features.Dtos;
using KampusRitim.Application.Interfaces.Repositories;
using MediatR;

namespace KampusRitim.Application.UseCases.User.GetUserById
{
    public class GetUserByIdHandler : IRequestHandler<GetUserByIdRequest, GetUserByIdResponse>
    {
        private readonly IUserRepository _userRepo;

        public GetUserByIdHandler(IUserRepository userRepo)
        {
            _userRepo = userRepo;
        }

        public async Task<GetUserByIdResponse> Handle(GetUserByIdRequest request, CancellationToken cancellationToken)
        {
            // 1. Repository'den kullanıcıyı ID ile çek
            var user = await _userRepo.GetByIdAsync(request.Id);

            // 2. Kontrol: Kullanıcı var mı?
            if (user == null)
            {
                return new GetUserByIdResponse
                {
                    IsSuccess = false,
                    Message = $"Aranan {request.Id} numaralı kullanıcı bulunamadı.",
                    User = null
                };
            }

            // 3. Mapping: Entity -> UserDto (Record)
            // Entity'deki verileri DTO'ya aktarıyoruz.
            var userDto = new UserDto(
                user.Id,
                user.Name,
                user.Surname,
                user.Email,
                user.Faculty,
                user.Department,
                user.ClassLevel // Enum tipi korunarak aktarılıyor (DTO'daki JsonConverter bunu string yapacak)
            );

            // 4. Başarılı Dönüş
            return new GetUserByIdResponse
            {
                IsSuccess = true,
                Message = "Kullanıcı bilgileri getirildi.",
                User = userDto
            };
        }
    }
}
