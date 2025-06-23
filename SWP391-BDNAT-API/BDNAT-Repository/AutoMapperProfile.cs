using AutoMapper;
using BDNAT_Repository.DTO;
using BDNAT_Repository.DTO.Auth;
using BDNAT_Repository.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace BDNAT_Repository
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Blog, BlogDTO>().ReverseMap();
            CreateMap<BlogsType, BlogsTypeDTO>().ReverseMap();
            CreateMap<Booking, BookingRequestDTO>().ReverseMap();

            CreateMap<Booking, BookingDisplayDTO>().ReverseMap();
            CreateMap<Booking, BookingSampleDTO>().ReverseMap();
            CreateMap<Booking, BookingScheduleDTO>().ReverseMap();
            CreateMap<Booking, BookingDisplayDetailDTO>().ReverseMap();

            CreateMap<SampleCollectionSchedule, SampleCollectionScheduleDTO>().ReverseMap();

            CreateMap<Comment, CommentDTO>().ReverseMap();
            CreateMap<Favorite, FavoriteDTO>().ReverseMap();
            CreateMap<Feedback, FeedbackDTO>().ReverseMap();
            CreateMap<KitOrder, KitOrderDTO>().ReverseMap();
            CreateMap<Parameter, ParameterDTO>().ReverseMap();
            CreateMap<Rating, RatingDTO>().ReverseMap();
            CreateMap<ResultDetail, ResultDetailDTO>()
                .ForMember(dest => dest.ParameterName,opt => opt.MapFrom(src => src.TestParameter.Parameter.Name));

            CreateMap<Sample, SampleDTO>().ReverseMap();
            CreateMap<Service, ServiceDTO>().ReverseMap();
            CreateMap<ServiceType, ServiceTypeDTO>().ReverseMap();
            CreateMap<ShippingOrder, ShippingOrderDTO>().ReverseMap();
            CreateMap<TestKit, TestKitDTO>().ReverseMap();
            CreateMap<TestParameter, TestParameterDTO>().ReverseMap();
            CreateMap<TestParameter, TestParameterResultDTO>().ReverseMap();
            CreateMap<Transaction, TransactionDTO>().ReverseMap();
            CreateMap<User, UserDTO>().ReverseMap();
            CreateMap<User, RegisterRequest>().ReverseMap();
            CreateMap<SampleCollectionSchedule, SampleCollectionScheduleDTO>().ReverseMap();
            CreateMap<Team, TeamDTO>().ReverseMap();
            CreateMap<WorkSchedule, WorkScheduleDTO>().ReverseMap();
            CreateMap<UserWorkSchedule, UserWorkScheduleDTO>().ReverseMap();
        }
    }
}
