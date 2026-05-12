using MediatR;
using UGem.Domain.Entities;
using UGem.Shared.Abstractions;

namespace UGem.Application.Features.Reviews;

public record SubmitReviewCommand(
    Guid RestaurantId,
    int Rating,
    string Comment,
    string IpAddress,
    string UserAgent) : IRequest<Result<Guid>>;

public class SubmitReviewHandler : IRequestHandler<SubmitReviewCommand, Result<Guid>>
{
    private readonly IRepository<Review> _reviewRepository;
    private readonly IUserContext _userContext;

    public SubmitReviewHandler(IRepository<Review> reviewRepository, IUserContext userContext)
    {
        _reviewRepository = reviewRepository;
        _userContext = userContext;
    }

    public async Task<Result<Guid>> Handle(SubmitReviewCommand request, CancellationToken ct)
    {
        // 1. Check if user already reviewed this restaurant (Business Invariant)
        // Note: Simplified for this phase
        
        // 2. Create Review with Anti-Fraud Metadata
        var review = Review.Create(
            request.RestaurantId,
            _userContext.UserId,
            request.Rating,
            request.Comment,
            request.IpAddress,
            request.UserAgent,
            isVerified: false // Default to unverified until check-in link confirmed
        );

        await _reviewRepository.AddAsync(review, ct);
        
        return Result<Guid>.Success(review.Id);
    }
}
