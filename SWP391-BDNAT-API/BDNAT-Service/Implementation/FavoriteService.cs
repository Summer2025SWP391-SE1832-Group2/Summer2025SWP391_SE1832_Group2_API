using AutoMapper;
using BDNAT_Repository.Entities;
using BDNAT_Repository.Implementation;
using BDNAT_Repository.DTO;
using BDNAT_Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDNAT_Service.Implementation
{
    public class FavoriteService : IFavoriteService
    {
        private readonly IMapper _mapper;

        public FavoriteService(IMapper mapper)
        {
            _mapper = mapper;
        }

        public async Task<bool> CreateOrToggleFavoriteAsync(FavoriteDTO favorite)
        {
            var allFavorites = await FavoriteRepo.Instance.GetAllAsync();
            var existingFavorite = allFavorites
                .FirstOrDefault(f => f.UserId == favorite.UserId && f.BlogId == favorite.BlogId);

            if (existingFavorite != null)
            {
                // Nếu đã tồn tại, xóa
                return await FavoriteRepo.Instance.DeleteAsync(existingFavorite.FavoriteId);
            }
            else
            {
                // Nếu chưa có, thêm mới
                var newFavorite = _mapper.Map<Favorite>(favorite);
                return await FavoriteRepo.Instance.InsertAsync(newFavorite);
            }
        }


        public async Task<List<FavoriteDTO>> GetFavoritesByBlogAsync(int blogId)
        {
            var favorites = await FavoriteRepo.Instance.GetFavoritesByBlogIdAsync(blogId);
            return favorites.Select(f => new FavoriteDTO
            {
                FavoriteId = f.FavoriteId,
                UserId = f.UserId,
                BlogId = f.BlogId
            }).ToList();
        }
        public async Task<bool> DeleteFavoriteAsync(int id)
        {
            return await FavoriteRepo.Instance.DeleteAsync(id);
        }

        public async Task<List<FavoriteDTO>> GetAllFavoritesAsync()
        {
            var list = await FavoriteRepo.Instance.GetAllAsync();
            return list.Select(x => _mapper.Map<FavoriteDTO>(x)).ToList();
        }

        public async Task<FavoriteDTO> GetFavoriteByIdAsync(int id)
        {
            return _mapper.Map<FavoriteDTO>(await FavoriteRepo.Instance.GetByIdAsync(id));
        }

        public async Task<bool> UpdateFavoriteAsync(FavoriteDTO favorite)
        {
            var map = _mapper.Map<Favorite>(favorite);
            return await FavoriteRepo.Instance.UpdateAsync(map);
        }
    }

}
