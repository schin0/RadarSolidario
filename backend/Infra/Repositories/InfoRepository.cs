using Azure.Data.Tables;
using Infra.Entities;
using Infra.Interfaces;

namespace Infra.Repositories;

public class InfoRepository : IInfoRepository
{
    private readonly TableClient _tableClient;
    private const string PartitionKey = "Info";
    private const string RowKey = "1";

    public InfoRepository(TableServiceClient serviceClient)
    {
        _tableClient = serviceClient.GetTableClient("Info");
        _tableClient.CreateIfNotExists();
    }

    public async Task<Info?> GetAsync()
    {
        var response = await _tableClient.GetEntityIfExistsAsync<Info>(PartitionKey, RowKey);
        return response.HasValue ? response.Value : null;
    }

    public async Task IncrementQuantityHelpAsync()
    {
        await IncrementFieldAsync(nameof(Info.QuantityHelp));
    }

    public async Task IncrementQuantityVolunteersAsync()
    {
        await IncrementFieldAsync(nameof(Info.QuantityVolunteers));
    }

    private async Task IncrementFieldAsync(string fieldName)
    {
        var response = await _tableClient.GetEntityIfExistsAsync<Info>(PartitionKey, RowKey);

        Info entity;
        if (response.HasValue)
            entity = response.Value!;
        else
            entity = new Info { PartitionKey = PartitionKey, RowKey = RowKey };

        switch (fieldName)
        {
            case nameof(Info.QuantityHelp):
                entity.QuantityHelp += 1;
                break;
            case nameof(Info.QuantityVolunteers):
                entity.QuantityVolunteers += 1;
                break;
        }

        await _tableClient.UpsertEntityAsync(entity);
    }
}
