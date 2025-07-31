using AutoMapper;
using BDNAT_Repository.DTO;
using BDNAT_Repository.Entities;
using BDNAT_Repository.Implementation;
using BDNAT_Repository.Interface;
using BDNAT_Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDNAT_Service.Implementation
{
    public class NotificationService : INotificationService
    {
        private readonly IMapper _mapper;

        public NotificationService(IMapper mapper)
        {
            _mapper = mapper;
        }

        public async Task<bool> CreateNotificationAsync(NotificationDTO Notification)
        {
            var mapNotification = _mapper.Map<Notification>(Notification);
            return await NotificationRepo.Instance.InsertAsync(mapNotification);
        }

        public Task<bool> DeleteNotificationAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<List<BlogDTO>> GetAllNotificationsAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<List<NotificationDTO>> GetNotificationsByUserId(int userId)
        {
            var List = await NotificationRepo.Instance.GetNotificationsByUserID(userId);
            return List.Select(log => _mapper.Map<NotificationDTO>(log)).ToList();
        }

        public Task<NotificationDTO> GetNotificationByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<List<NotificationDTO>> GetNotificationsByNotificationTypeIdAsync(int NotificationTypeId)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateNotificationAsync(NotificationDTO Notification)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> MarkNotificationAsReadAsync(int notificationId)
        {
            return await NotificationRepo.Instance.MarkAsReadAsync(notificationId);
        }
    }
}
