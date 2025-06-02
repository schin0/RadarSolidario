using Infra.Entities;

namespace Infra.Interfaces;

public interface IInfoRepository
{
    Task IncrementQuantityHelpAsync();
    Task IncrementQuantityVolunteersAsync();
    Task<Info?> GetAsync();
}
