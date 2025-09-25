using AutoMapper;
using CurrencyExchange.Application.DTOs;
using CurrencyExchange.Domain.Entities;

namespace CurrencyExchange.Application.Profiles
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Currency, CurrencyDto>().ReverseMap();
            CreateMap<Wallet, WalletDto>().ReverseMap();
            CreateMap<Funds, FundsDto>()
                .ForMember(dest => dest.CurrencyCode, opt => opt.MapFrom(src => src.Currency.Code))
                .ReverseMap();
        }
    }
}
