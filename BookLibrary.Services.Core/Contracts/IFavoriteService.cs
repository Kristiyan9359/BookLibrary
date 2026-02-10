namespace BookLibrary.Services.Core.Contracts;

using BookLibrary.ViewModels.Favorites;
public interface IFavoriteService
{
    Task<bool?> ToggleAsync(int bookId, string userId);
    Task<IEnumerable<FavoriteBookViewModel>> GetUserFavoritesAsync(string userId);
}
