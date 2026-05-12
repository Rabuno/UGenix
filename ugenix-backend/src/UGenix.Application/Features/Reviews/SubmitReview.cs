using MediatR;
using UGenix.Domain.Entities;
using UGenix.Shared.Abstractions;

namespace UGenix.Application.Features.Reviews;

public record SubmitReviewCommand(
    Guid RestaurantId,
    int Rating,
    string Comment,
    string IpAddress,
    string UserAgent) : IRequest<Result<Guid>>;

// Handler

public class SubmitReviewHandler : IRequestHandler<SubmitReviewCommand, Result<Guid>>
{
    private readonly IRepository<Review> _reviewRepository;
    private readonly ICurrentUser _currentUser;

    public SubmitReviewHandler(IRepository<Review> reviewRepository, ICurrentUser currentUser)
    {
        _reviewRepository = reviewRepository;
        _currentUser = currentUser;
    }

    public async Task<Result<Guid>> Handle(SubmitReviewCommand request, CancellationToken ct)
    {
        var review = Review.Create(
            request.RestaurantId,
            _currentUser.UserId,
            request.Rating,
            request.Comment,
            request.UserAgent,
            request.IpAddress
        );

        await _reviewRepository.AddAsync(review, ct);
        
        return Result<Guid>.Success(review.Id);
    }
}

