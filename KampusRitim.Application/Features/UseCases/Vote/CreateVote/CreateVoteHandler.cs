using KampusRitim.Application.Interfaces;
using KampusRitim.Application.Interfaces.Repositories;
using MediatR;

namespace KampusRitim.Application.Features.UseCases.Vote.CreateVote
{
    public class CreateVoteHandler : IRequestHandler<CreateVoteRequest, CreateVoteResponse>
    {
        // Artık DbContext yerine Repository kullanıyoruz
        private readonly IVoteRepository _voteRepository;
        private readonly ICurrentUserService _currentUserService;

        public CreateVoteHandler(IVoteRepository voteRepository, ICurrentUserService currentUserService)
        {
            _voteRepository = voteRepository;
            _currentUserService = currentUserService;
        }

        public async Task<CreateVoteResponse> Handle(CreateVoteRequest request, CancellationToken cancellationToken)
        {
            // 1. SİSTEMDEKİ KULLANICIYI BUL (Token'dan Zehranur'un ID'sini çeker)
            var currentUserId = _currentUserService.UserId;

            if (_currentUserService.UserId == null)
            {
                return new CreateVoteResponse { IsSuccess = false, Message = "Oy vermek için giriş yapmalısınız." };
            }

            int userId = _currentUserService.UserId.Value;

            // REPOSITORY KULLANIMI 1: Kontrol
            bool hasVoted = await _voteRepository.HasUserVotedAsync(userId, request.EventId);

            if (hasVoted)
            {
                return new CreateVoteResponse { IsSuccess = false, Message = "Zaten oy kullandınız." };
            }

            var newVote = new KampusRitim.Domain.Entity.Vote
            {
                EventId = request.EventId,
                UserId = userId,
                Score = request.Score,
                VotedAt = DateTime.UtcNow
            };

            // REPOSITORY KULLANIMI 2: Ekleme
            await _voteRepository.AddAsync(newVote);

            return new CreateVoteResponse
            {
                IsSuccess = true,
                Message = "Puanınız başarıyla kaydedildi."
            };
        }
    }
}
