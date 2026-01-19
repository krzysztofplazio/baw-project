var builder = DistributedApplication.CreateBuilder(args);


var postgres = builder.AddPostgres("postgres");
var postgresdb = postgres.AddDatabase("VisitDb");

var api = builder.AddProject<Projects.Project_Baw>("Api")
    .WithHealthCheck("/health")
    .WaitFor(postgresdb)
    .WithReference(postgresdb);

builder.Build().Run();