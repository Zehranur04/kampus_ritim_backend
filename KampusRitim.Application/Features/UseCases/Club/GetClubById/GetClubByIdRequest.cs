using MediatR;

namespace KampusRitim.Application.UseCases.Club.GetClubById
{
    // Bu sorgu, işlem sonucunda "GetClubByIdQueryResponse" dönecek.
    public class GetClubByIdRequest : IRequest<GetClubByIdResponse>
    {
        public int ClubId { get; set; }
    }
}