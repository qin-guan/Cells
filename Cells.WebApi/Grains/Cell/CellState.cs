namespace Cells.WebApi.Grains.Cell;

[GenerateSerializer]
[Alias("Cells.WebApi.Grains.Cell.CellState")]
public class CellState
{
    [Id(0)] public string Content { get; set; }
}