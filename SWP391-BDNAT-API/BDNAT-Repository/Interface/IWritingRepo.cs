using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BDNAT_Repository.Entities;

namespace BDNAT_Repository.Interface
{
    internal interface IWritingRepo
    {
            Task<List<Writing>> GetAllAsync();
            Task<Writing?> GetByIdAsync(int id);
            Task<Writing> CreateAsync(Writing writing);
            Task<bool> UpdateAsync(Writing writing);
            Task<bool> DeleteAsync(int id);
     
    }
}
