using Projects;

var builder = DistributedApplication.CreateBuilder(args);

var storage = builder.AddAzureStorage("storage")
    .RunAsEmulator(options =>
    {
        options.WithBlobPort(10000);
        options.WithQueuePort(10001);
        options.WithTablePort(10002);
    });

var clusteringTable = storage.AddTables("clustering");
var datasetStorage = storage.AddBlobs("dataset");
var columnStorage = storage.AddBlobs("column");
var cellStorage = storage.AddBlobs("cell");

var orleans = builder.AddOrleans("default")
    .WithGrainStorage("dataset", datasetStorage)
    .WithGrainStorage("column", columnStorage)
    .WithGrainStorage("cell", cellStorage)
    .WithClustering(clusteringTable);

var api = builder.AddProject<Cells_WebApi>("api")
    .WithReference(orleans)
    .WithReplicas(3);

builder.Build().Run();