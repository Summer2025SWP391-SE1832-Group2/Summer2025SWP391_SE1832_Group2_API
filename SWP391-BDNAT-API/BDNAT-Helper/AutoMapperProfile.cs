using AutoMapper;
using BDNAT_Repository.Entities;
using BDNAT_Service.DTO;

namespace BDNAT_Helper
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Blog, BlogDTO>().ReverseMap();
            CreateMap<BlogsType, BlogsTypeDTO>().ReverseMap();
            CreateMap<Booking, BookingDTO>().ReverseMap();
            CreateMap<Comment, CommentDTO>().ReverseMap();
            CreateMap<Favorite, FavoriteDTO>().ReverseMap();
            CreateMap<Feedback, FeedbackDTO>().ReverseMap();
            CreateMap<KitOrder, KitOrderDTO>().ReverseMap();
            CreateMap<Parameter, ParameterDTO>().ReverseMap();
            CreateMap<Rating, RatingDTO>().ReverseMap();
            CreateMap<Result, ResultDTO>().ReverseMap();
            CreateMap<Sample, SampleDTO>().ReverseMap();
            CreateMap<Service, ServiceDTO>().ReverseMap();
            CreateMap<ServiceType, ServiceTypeDTO>().ReverseMap();
            CreateMap<ShippingOrder, ShippingOrderDTO>().ReverseMap();
            CreateMap<TestKit, TestKitDTO>().ReverseMap();
            CreateMap<TestParameter, TestParameterDTO>().ReverseMap();
            CreateMap<Transaction, TransactionDTO>().ReverseMap();
            CreateMap<User, UserDTO>().ReverseMap();
        }
    }

}