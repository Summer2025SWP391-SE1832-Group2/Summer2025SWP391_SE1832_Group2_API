using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using BDNAT_Repository.DTO;
using BDNAT_Repository.Entities;
using BDNAT_Repository.Implementation;
using BDNAT_Repository.Interface;
using BDNAT_Service.Interface;

namespace BDNAT_Service.Implementation
{
    public class WritingService : IWritingService
    {
        private readonly IMapper _mapper;

        public WritingService(IMapper mapper)
        {
            _mapper = mapper;
        }

        public async Task<List<WritingDTO>> GetAllAsync()
        {
            var list = await IWritingRepo.Instance.GetAllAsync();
            return list.Select(x => _mapper.Map<WritingDTO>(x)).ToList();
        }

        public async Task<WritingDTO?> GetByIdAsync(int id)
        {
            var entity = await IWritingRepo.Instance.GetByIdAsync(id);
            return entity == null ? null : _mapper.Map<WritingDTO>(entity);
        }

        public async Task<bool> CreateAsync(WritingDTO dto)
        {
            var entity = _mapper.Map<Writing>(dto);
            return await IWritingRepo.Instance.CreateAsync(entity);
        }

        public async Task<bool> UpdateAsync(WritingDTO dto)
        {
            var entity = _mapper.Map<Writing>(dto);
            return await IWritingRepo.Instance.UpdateAsync(entity);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            return await WritingRepo.Instance.DeleteAsync(id);
        }
    }
}
