using MediatR;
using KampusRitim.Application.Interfaces.Repositories;

namespace KampusRitim.Application.UseCases.Club.DeleteClub
{
    public class DeleteClubHandler : IRequestHandler<DeleteClubRequest, DeleteClubResponse>
    {
        private readonly IClubRepository _clubRepo;

        public DeleteClubHandler(IClubRepository clubRepo)
        {
            _clubRepo = clubRepo;
        }

        public async Task<DeleteClubResponse> Handle(DeleteClubRequest request, CancellationToken cancellationToken)
        {
            // 1. ÖNCE BUL: Silinecek kulübü ID ile çağırıyoruz.
            var existingClub = await _clubRepo.GetByIdAsync(request.Id);

            // 2. KONTROL: Eğer kulüp yoksa işlem burada biter, Repository'ye hiç gitmeyiz.
            if (existingClub == null)
            {
                return new DeleteClubResponse
                {
                    IsSuccess = false,
                    Message = $"Silinmek istenen {request.Id} numaralı kulüp bulunamadı."
                };
            }

            // 3. SİL: Kulüp elimizde, şimdi Repository'ye veriyoruz.
            await _clubRepo.DeleteAsync(existingClub);

            return new DeleteClubResponse
            {
                IsSuccess = true,
                Message = "Kulüp başarıyla silindi."
            };
        }
    }
}