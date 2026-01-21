var builder = DistributedApplication.CreateBuilder(args);


var postgres = builder.AddPostgres("postgres", port: 54320);
var postgresdb = postgres.AddDatabase("VisitDb");

var api = builder.AddProject<Projects.Project_Baw>("Api")
    .WaitFor(postgresdb)
    .WithReference(postgresdb);

builder.AddNpmApp("frontend", "../Project.Baw.Web", "dev")
    .WithReference(api)
    .WaitFor(api)
    .WithExternalHttpEndpoints()
    .PublishAsDockerFile();

builder.Build().Run();