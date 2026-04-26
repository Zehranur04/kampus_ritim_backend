using KampusRitim.Application.Interfaces.Repositories;
using MediatR;

namespace KampusRitim.Application.UseCases.User.DeleteUser
{
    public class DeleteUserHandler : IRequestHandler<DeleteUserRequest, DeleteUserResponse>
    {
        private readonly IUserRepository _userRepo;

        public DeleteUserHandler(IUserRepository userRepo)
        {
            _userRepo = userRepo;
        }

        public async Task<DeleteUserResponse> Handle(DeleteUserRequest request, CancellationToken cancellationToken)
        {
            // 1. ADIM: Önce silinecek kullanıcıyı bul
            var userToDelete = await _userRepo.GetByIdAsync(request.Id);

            // 2. ADIM: Kullanıcı var mı kontrolü (Unhappy Path)
            if (userToDelete == null)
            {
                // Olumsuz Senaryo
                return new DeleteUserResponse
                {
                    IsSuccess = false,
                    Message = $"Silinmek istenen {request.Id} numaralı kullanıcı bulunamadı."
                };
            }

            // 3. ADIM: Silme İşlemi
            // Repository'ye bulduğumuz entity'yi teslim ediyoruz.
            await _userRepo.DeleteAsync(userToDelete);

            // Olumlu Senaryo
            return new DeleteUserResponse
            {
                IsSuccess = true,
                Message = "Kullanıcı başarıyla silindi."
            };
        }
    }
}