using Projects;

var builder = DistributedApplication.CreateBuilder(args);

var storage = builder.AddAzureStorage("storage");
var clusteringTable = storage.AddTables("clustering");

var orleans = builder.AddOrleans("default")
    .WithClustering(clusteringTable);

var api = builder.AddProject<Cells_WebApi>("api")
    .WithReference(orleans)
    .WithReplicas(3);

builder.Build().Run();