using Cells.WebApi.Grains.Cell;
using Orleans.Runtime;
using Sylvan.Data.Csv;

namespace Cells.WebApi.Grains.Dataset;

public class DatasetGrain(
    [PersistentState("dataset", "dataset")]
    IPersistentState<DatasetState> state
) : Grain, IDatasetGrain
{
    public async Task CreateAsync(string content)
    {
        var sr = new StringReader(content);
        await using var reader = await CsvDataReader.CreateAsync(sr);

        state.State.ColNames = reader.GetColumnSchema().Select(c => c.ColumnName).ToList();

        var row = 0;

        while (await reader.ReadAsync())
        {
            foreach (var col in state.State.ColNames)
            {
                var index = reader.GetOrdinal(col);
                await GrainFactory.GetGrain<ICellGrain>($"{this.GetPrimaryKeyString()}-{col}-{row}")
                    .CreateAsync(reader.GetString(index));
            }

            row++;
        }

        state.State.Rows = row;
        await state.WriteStateAsync();
    }

    public Task<DatasetState> GetAsync()
    {
        return Task.FromResult(state.State);
    }
}