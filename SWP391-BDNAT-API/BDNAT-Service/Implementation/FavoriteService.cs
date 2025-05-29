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

        public async Task<bool> CreateFavoriteAsync(FavoriteDTO favorite)
        {
            var map = _mapper.Map<Favorite>(favorite);
            return await FavoriteRepo.Instance.InsertAsync(map);
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
