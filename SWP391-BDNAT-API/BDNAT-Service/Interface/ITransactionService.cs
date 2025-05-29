using BDNAT_Repository.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDNAT_Service.Interface
{
    public interface ITransactionService
    {
        Task<List<TransactionDTO>> GetAllTransactionsAsync();
        Task<TransactionDTO> GetTransactionByIdAsync(int id);
        Task<bool> CreateTransactionAsync(TransactionDTO transaction);
        Task<bool> UpdateTransactionAsync(TransactionDTO transaction);
        Task<bool> DeleteTransactionAsync(int id);
    }
}
