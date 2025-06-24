using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BDNAT_Repository.DTO;

namespace BDNAT_Service.Interface
{
   
    public interface IWritingService
    {
        Task<List<WritingDTO>> GetAllAsync();
        Task<WritingDTO> GetByIdAsync(int id);
        Task<bool> CreateAsync(WritingDTO dto);
        Task<bool> UpdateAsync(WritingDTO dto);
        Task<bool> DeleteAsync(int id);
    }

}
