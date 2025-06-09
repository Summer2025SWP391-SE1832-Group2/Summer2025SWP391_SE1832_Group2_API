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
    public class TransactionService : ITransactionService
    {
        private readonly IMapper _mapper;

        public TransactionService(IMapper mapper)
        {
            _mapper = mapper;
        }

        public async Task<bool> CreateTransactionAsync(TransactionDTO transaction)
        {
            var map = _mapper.Map<Transaction>(transaction);
            return await TransactionRepo.Instance.InsertAsync(map);
        }

        public async Task<bool> DeleteTransactionAsync(int id)
        {
            return await TransactionRepo.Instance.DeleteAsync(id);
        }

        public async Task<List<TransactionDTO>> GetAllTransactionsAsync()
        {
            var list = await TransactionRepo.Instance.GetAllAsync();
            return list.Select(x => _mapper.Map<TransactionDTO>(x)).ToList();
        }

        public async Task<TransactionDTO> GetByOrderCodeAsync(string orderCode)
        {
            return _mapper.Map<TransactionDTO>(await TransactionRepo.Instance.GetByOrderCodeAsync(orderCode));
        }

        public async Task<TransactionDTO> GetTransactionByIdAsync(int id)
        {
            return _mapper.Map<TransactionDTO>(await TransactionRepo.Instance.GetByIdAsync(id));
        }

        public async Task<bool> UpdateTransactionAsync(TransactionDTO transaction)
        {
            var map = _mapper.Map<Transaction>(transaction);
            return await TransactionRepo.Instance.UpdateAsync(map);
        }
    }

}
