using BDNAT_Repository.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDNAT_Repository.Interface
{
    public interface ITechnicalServiceRepo : IGenericRepository<TechnicalService>
    {
        Task<List<TechnicalService>> GetTechnicalServicesByTechnicalIdAsync(int technicalId);
        Task<List<TechnicalService>> GetTechnicalServicesByServiceIdAsync(int serviceId);
    }
} 