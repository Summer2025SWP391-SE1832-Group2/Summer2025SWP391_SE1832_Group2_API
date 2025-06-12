using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BDNAT_Repository.DTO;

namespace BDNAT_Service.Interface
{
    public interface ITechnicalService
    {
        Task<List<TechnicalDTO>> GetAllTechnicalAsync();
        Task<TechnicalDTO> GetTechnicalByIdAsync(int id);
        Task<bool> CreateTechnicalAsync(TechnicalDTO Technical);
        Task<bool> UpdateTechnicalAsync(TechnicalDTO Technical);
        Task<bool> DeleteTechnicalAsync(int id);
    }
}
