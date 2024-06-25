namespace Cells.WebApi.Grains.Cell;

public interface ICellGrain : IGrainWithStringKey
{
    public Task CreateAsync(string content);
    public Task<string> GetAsync();
}