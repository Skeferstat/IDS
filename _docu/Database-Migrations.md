

#### <span style="color:orange">Database migrations</span>

```
cd Server\
```
```
 dotnet ef migrations add InitialCreate --project IdsServer.Database --startup-project .\IdsServer --context AppDbContext

```

```
 dotnet ef migrations add InitialCreate --project SupportCenter.Database --startup-project .\SupportCenter.Web --context IdentityDbContext
```

The web application starts the missing migrations automatically. However, if you want to update a specific migration yourself, use the command:
```
  dotnet ef database update
```

#### <span style="color:orange">Miniprofiler</span>

Go to (or where your web api is running)

```
https://localhost:7025/profiler/results-index
```
and see the profiling results.


