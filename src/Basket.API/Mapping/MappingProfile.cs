using AutoMapper;
using Basket.Domain.Entities;
using EventBus.Messages.Events;

namespace Basket.API.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<BasketCheckout, BasketCheckoutEvent>().ReverseMap();
        }
    }
}
