namespace Cells.WebApi.Grains.Dataset;

[GenerateSerializer]
[Alias("Cells.WebApi.Grains.Dataset.DatasetState")]
public class DatasetState
{
    [Id(0)] public Guid Id { get; set; }
    [Id(1)] public List<string> ColNames { get; set; }
    [Id(2)] public int Rows { get; set; }
}