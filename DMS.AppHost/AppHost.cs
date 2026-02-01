var builder = DistributedApplication.CreateBuilder(args);

var username = builder.AddParameter("PostgresUsername", secret: true);
var password = builder.AddParameter("PostgresPassword", secret: true);

var postgres = builder.AddPostgres("postgres", username, password)
    .WithPgAdmin(pgAdmin => pgAdmin.WithHostPort(5050))
    .WithDataBindMount(
        source: "./.containers/db",
        isReadOnly: false);

var dmsDb = postgres.AddDatabase("dms");

var cache = builder.AddRedis("cache")
    .WithRedisInsight();

builder.AddProject<Projects.DMS_WebApi>("webapi")
    .WaitFor(dmsDb)
    .WithReference(dmsDb)
    .WithReference(cache)
    .WithHttpHealthCheck("/health");

builder.Build().Run();
