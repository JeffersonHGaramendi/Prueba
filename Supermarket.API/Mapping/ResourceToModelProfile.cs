using AutoMapper;
using Supermarket.API.Domain.Models;
using Supermarket.API.Resources;

namespace Supermarket.API.Mapping
{
    public class ResourceToModelProfile : Profile
    {
        public ResourceToModelProfile()
        {
            CreateMap<SaveCategoryResource, Category>();

            CreateMap<SaveProductResource, Product>()
                .ForMember(
                    target => target.UnitOfMeasurement,
                    options
                        => options.MapFrom(source
                            => (EUnitOfMeasurement) source.UnitOfMeasurement));
        }
    }
}