namespace BookLibrary.Services.Core.Contracts;

using BookLibrary.ViewModels.Books;

public interface IReviewService
{
    Task<bool> AddAsync(ReviewCreateViewModel model);
}
