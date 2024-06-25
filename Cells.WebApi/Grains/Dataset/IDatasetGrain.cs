namespace Cells.WebApi.Grains.Dataset;

public interface IDatasetGrain : IGrainWithGuidKey
{
    public Task CreateAsync(string content);
    public Task<DatasetState> GetAsync();
}