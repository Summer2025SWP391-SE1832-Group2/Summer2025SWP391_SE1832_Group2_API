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
    public class BookingService : IBookingService
    {
        private readonly IMapper _mapper;

        public BookingService(IMapper mapper)
        {
            _mapper = mapper;
        }

        public async Task<bool> CreateBookingAsync(BookingDTO booking)
        {
            var map = _mapper.Map<Booking>(booking);
            return await BookingRepo.Instance.InsertAsync(map);
        }

        public async Task<bool> DeleteBookingAsync(int id)
        {
            return await BookingRepo.Instance.DeleteAsync(id);
        }

        public async Task<List<BookingDTO>> GetAllBookingsAsync()
        {
            var list = await BookingRepo.Instance.GetAllAsync();
            return list.Select(x => _mapper.Map<BookingDTO>(x)).ToList();
        }

        public async Task<BookingDTO> GetBookingByIdAsync(int id)
        {
            return _mapper.Map<BookingDTO>(await BookingRepo.Instance.GetByIdAsync(id));
        }

        public async Task<bool> UpdateBookingAsync(BookingDTO booking)
        {
            var map = _mapper.Map<Booking>(booking);
            return await BookingRepo.Instance.UpdateAsync(map);
        }
    }

}
