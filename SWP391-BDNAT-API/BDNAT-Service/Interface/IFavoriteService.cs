using BDNAT_Repository.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDNAT_Service.Interface
{
    public interface IFavoriteService
    {
        Task<List<FavoriteDTO>> GetAllFavoritesAsync();
        Task<FavoriteDTO> GetFavoriteByIdAsync(int id);
        Task<bool> CreateFavoriteAsync(FavoriteDTO favorite);
        Task<bool> UpdateFavoriteAsync(FavoriteDTO favorite);
        Task<bool> DeleteFavoriteAsync(int id);
    }
}
