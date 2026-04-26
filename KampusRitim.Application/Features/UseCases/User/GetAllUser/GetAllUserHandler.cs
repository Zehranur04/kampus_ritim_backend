using KampusRitim.Application.Features.Dtos;
using KampusRitim.Application.Interfaces.Repositories;
using MediatR;

namespace KampusRitim.Application.UseCases.User.GetAllUsers
{
    public class GetAllUserHandler : IRequestHandler<GetAllUserRequest, GetAllUserResponse>
    {
        private readonly IUserRepository _userRepo;

        public GetAllUserHandler(IUserRepository userRepo)
        {
            _userRepo = userRepo;
        }

        public async Task<GetAllUserResponse> Handle(GetAllUserRequest request, CancellationToken cancellationToken)
        {
            // 1. Repository'den tüm kullanıcıları çek
            // (IUserRepository içinde GetAllAsync olduğunu varsayıyoruz)
            var users = await _userRepo.GetAllAsync();

            // 2. Mapping: Entity -> UserDto (Record)
            var userDtos = users.Select(u => new UserDto(
                u.Id,
                u.Name,
                u.Surname,
                u.Email,
                u.Faculty,
                u.Department,
                u.ClassLevel // Enum direkt taşınıyor, DTO'daki attribute JSON'a çevirirken halledecek
            )).ToList();

            // 3. Response Dön
            return new GetAllUserResponse
            {
                IsSuccess = true,
                Message = "Kullanıcılar başarıyla listelendi.",
                Users = userDtos
            };
        }
    }
}