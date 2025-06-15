using BDNAT_Repository.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDNAT_Service.Interface
{
    public interface IWorkScheduleService
    {
        Task<List<WorkScheduleDTO>> GetAllWorkSchedulesAsync();
        Task<WorkScheduleDTO> GetWorkScheduleByIdAsync(int id);
        Task<bool> CreateWorkScheduleAsync(WorkScheduleDTO WorkSchedule);

        Task<bool> CreateListWorkScheduleAsync(List<WorkScheduleDTO> WorkSchedule);

        Task<bool> UpdateWorkScheduleAsync(WorkScheduleDTO WorkSchedule);
        Task<bool> DeleteWorkScheduleAsync(int id);
    }
}
