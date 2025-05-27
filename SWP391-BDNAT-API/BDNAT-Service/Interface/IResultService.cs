using BDNAT_Service.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDNAT_Service.Interface
{
    public interface IResultService
    {
        Task<List<ResultDTO>> GetAllResultsAsync();
        Task<ResultDTO> GetResultByIdAsync(int id);
        Task<bool> CreateResultAsync(ResultDTO result);
        Task<bool> UpdateResultAsync(ResultDTO result);
        Task<bool> DeleteResultAsync(int id);
    }
}
