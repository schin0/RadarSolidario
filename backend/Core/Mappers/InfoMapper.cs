using Core.DTOs;
using Infra.Entities;

namespace Core.Mappers;

public static class InfoMapper
{
    public static InfoDto ToDto(this Info entity)
    {
        return new InfoDto(
            entity.QuantityHelp,
            entity.QuantityVolunteers,
            entity.Connections,
            entity.CommunitiesServed
        );
    }
}
