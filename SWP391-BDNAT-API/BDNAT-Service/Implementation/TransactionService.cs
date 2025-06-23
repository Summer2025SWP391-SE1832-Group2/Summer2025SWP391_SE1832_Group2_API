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
using System.Text.Json;

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

        public async Task<TransactionDTO> GetByOrderCodeAsync(long orderCode)
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

        public async Task<bool> HandleWebhookAsync(JsonElement payload)
        {
            try
            {
                if (!payload.TryGetProperty("data", out JsonElement dataElement))
                {
                    Console.WriteLine("[ERROR] Missing 'data' in payload");
                    return false;
                }

                if (!dataElement.TryGetProperty("orderCode", out JsonElement orderCodeElement))
                {
                    Console.WriteLine("[ERROR] Missing 'orderCode' in data");
                    return false;
                }

                long orderCode = orderCodeElement.GetInt64();
                Console.WriteLine($"[Webhook] orderCode: {orderCode}");

                var transaction = await GetByOrderCodeAsync(orderCode);
                if (transaction == null)
                {
                    Console.WriteLine("[ERROR] Transaction not found.");
                    return false;
                }

                transaction.Price = dataElement.GetProperty("amount").GetDecimal();
                transaction.Description = dataElement.GetProperty("description").GetString();
                transaction.TransactionCode = dataElement.GetProperty("reference").GetString();
                transaction.Status = "Đã thanh toán";
                transaction.UpdatedAt = DateTime.Parse(dataElement.GetProperty("transactionDateTime").GetString());

                var updateTran = await UpdateTransactionAsync(transaction);
                Console.WriteLine(updateTran ? "[INFO] Transaction updated successfully." : "[ERROR] Failed to update transaction.");

                var booking = await BookingRepo.Instance.GetBookingByOrderCodeAsync(orderCode);
                booking.PaymentStatus = "Đã thanh toán";
                var updatebooking = await BookingRepo.Instance.UpdateAsync(booking);
                Console.WriteLine(updateTran ? "[INFO] Booking updated successfully." : "[ERROR] Failed to update booking.");
                if (updateTran != true || updatebooking != true) { 
                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[EXCEPTION] {ex.Message}");
                return false;
            }
        }

    }

}
