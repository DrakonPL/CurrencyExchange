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

            CreateMap<Transaction, TransactionDto>()
                .ForMember(d => d.CurrencyCode, o => o.MapFrom(s => s.Currency!.Code))
                .ForMember(d => d.Type, o => o.MapFrom(s => s.Type.ToString()))
                .ForMember(d => d.Direction, o => o.MapFrom(s => s.Direction.ToString()));
        }
    }
}
