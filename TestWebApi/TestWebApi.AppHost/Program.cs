var builder = DistributedApplication.CreateBuilder(args);

builder.AddProject<Projects.TestWebApi>("testwebapi");

builder.Build().Run();
