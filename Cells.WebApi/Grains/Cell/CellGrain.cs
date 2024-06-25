using Orleans.Concurrency;
using Orleans.Placement;
using Orleans.Runtime;

namespace Cells.WebApi.Grains.Cell;

[Reentrant]
[RandomPlacement]
public class CellGrain(
    [PersistentState("cell", "cell")] IPersistentState<CellState> state
) : Grain, ICellGrain
{
    public async Task CreateAsync(string content)
    {
        state.State.Content = content;
        await state.WriteStateAsync();
        DeactivateOnIdle();
    }

    public Task<string> GetAsync()
    {
        return Task.FromResult(state.State.Content);
    }
}