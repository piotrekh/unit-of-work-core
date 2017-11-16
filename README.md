# UnitOfWorkCore
A standalone Unit of Work component for Entity Framework Core 2.0 and ASP.NET Core 2.0 which handles transactions automatically (in ASP.NET request scope) by committing (and rolling back in case of an error) them at the end of HTTP request. Supports multiple DbContexts.

## Repository contents
This repository contains several C# projects:

- **_UnitOfWorkCore_** - core library with Unit of Work pattern implementation
- **_UnitOfWorkCore.AspNetCore_** - UnitOfWorkCore configuration (transaction filters and isolation level attributes) for ASP.NET Core 2.0
- Sample projects:
  - _UnitOfWorkCore.Samples.ReleasesDatabase_ - sample database migration scripts (console app using [DbUp](http://dbup.github.io/))
  - _UnitOfWorkCore.Samples.IssuesDatabase_ - sample database migration scripts (console app using [DbUp](http://dbup.github.io/))
  - _UnitOfWorkCore.Samples.SingleContextApi_ - sample ASP.NET Core 2.0 project using UnitOfWorkCore with only one DbContext
  - _UnitOfWorkCore.Samples.MultiContextApi_ - sample ASP.NET Core 2.0 project using UnitOfWorkCore with two DbContexts

## Nuget packages
UnitOfWorkCore component is distributed as two separate NuGet packages

- [_UnitOfWorkCore_](https://www.nuget.org/packages/UnitOfWorkCore/) - for use in projects that don't depend on ASP.NET Core (e.g. services layer in your solution)
- [_UnitOfWorkCore.AspNetCore_](https://www.nuget.org/packages/UnitOfWorkCore.AspNetCore/) - for use in ASP.NET Core projects with REST API

To install `UnitOfWorkCore` and `UnitOfWorkCore.AspNetCore` in your project(s), use the Visual Studio's built-in NuGet GUI (_Manage NuGet packages_ option in project's context menu) or run the following commands in Package Manager Console:

    Install-Package UnitOfWorkCore    
    Install-Package UnitOfWorkCore.AspNetCore

## Single DbContext usage

Register your DbContext in `ConfigureServices` method in `Startup.cs`

```csharp
services.AddDbContext<ReleasesDbContext>(options => options.UseSqlServer(Configuration.GetConnectionString("ReleasesDb")));
```

Register UnitOfWorkCore for your DbContext in services

```csharp
services.AddUnitOfWork<ReleasesDbContext>();
```

Add **`UnitOfWorkTransactionFilter`** to global filters

```csharp
services.AddMvcCore(options =>
{      
    options.Filters.Add(typeof(UnitOfWorkTransactionFilter));
});
```

Inject **`IUnitOfWork`** into your services where you need to access DbSets or save database changes (like EF's DbContext.SaveChanges) to obtain inserted entity's id etc.

```csharp
public ReleasesService(IUnitOfWork uow) 
{ 
    this._uow = uow;
}
```

Use the `IUnitOfWork` in your service

```csharp
//add entity to the database
_uow.Set<ReleaseEntity>().Add(entity);

//save changes to obtain the entity id
_uow.SaveChanges();
```

---

**PRO TIP**: If you don't want to  write __uow.Set\<MyEntity\>()_ with the exact name of the entity's class because you never remember it, or would like Intellisense to help you with selecting the right entities set, you can either:

A) Use helper extension methods:

```csharp
public static DbSet<ReleaseEntity> Releases(this IUnitOfWork uow)
{
    return uow.Set<ReleaseEntity>();
}

//and then:
_uow.Releases().Add(entity);
```

B) Utilize decorator pattern described in the [next section](#multiple-dbcontexts-usage) with Multiple DbContexts usage (**this is a more elegant solution for multiple DbContexts, but it also works for a single one**)

---

## Multiple DbContexts usage

Register your DbContexts in `ConfigureServices` method in `Startup.cs`

```csharp
services.AddDbContext<ReleasesDbContext>(options => options.UseSqlServer(Configuration.GetConnectionString("ReleasesDb")));

services.AddDbContext<IssuesDbContext>(options => options.UseSqlServer(Configuration.GetConnectionString("IssuesDb")));
```

Register multiple UoWs, providing a unique key (string) for each of them

```csharp
services.AddUnitOfWorkPool(optionsBuilder =>
{
    optionsBuilder.AddUnitOfWork<ReleasesDbContext>("Releases");
    optionsBuilder.AddUnitOfWork<IssuesDbContext>("Issues");
});
```

Add **`UnitOfWorkPoolTransactionFilter`** (please notice the _Pool_ word) to global filters

```csharp
services.AddMvcCore(options =>
{      
    options.Filters.Add(typeof(UnitOfWorkPoolTransactionFilter));
});
```

Inject **`IUnitOfWorkPool`** into your services and retrieve `IUnitOfWork` for a specific DbContext by its key

```csharp
private readonly IUnitOfWork _releasesUow;
private readonly IUnitOfWork _issuesUow;

public MyService(IUnitOfWorkPool uowPool)
{
    _releasesUow = uowPool.Get("Releases");
    _issuesUow = uowPool.Get("Issues");
}
```

---

**PRO TIP**: Injecting `IUnitOfWorkPool` into your services and retrieving the required `IUnitOfWork` by key may be a bit tedious and unelegant - it would be much easier if you had a separate interface for each of your UoWs, e.g _IReleasesUoW_ and _IIssuesUoW_, which you could inject instead of the whole `IUnitOfWorkPool`. You can easily do this by using the [adapter pattern](http://www.oodesign.com/decorator-pattern.html) that will also allow you to extend UoW with additional methods and properties (e.g. DbSet<TEntity> properties for easier access to data with Intellisense, methods for bulk insert, delete etc.) See [the sample code](https://github.com/piotrekh/unit-of-work-core/tree/master/UnitOfWorkCore.Samples.MultiContextApi/DataAccess/ReleasesDb) in _UnitOfWorkCore.Samples.MultiContextApi_ project.

The general steps for the adapter are:
1. Create an interface that inherits from `IUnitOfWork<TDbContext>` and add your own methods and properties

```csharp
public interface IReleasesUoW : IUnitOfWork<ReleasesDbContext>
{
    DbSet<ReleaseEntity> Releases { get; }
}
```

2. Implement your custom interface, inject `IUnitOfWorkPool` and use the `IUnitOfWork` retrieved by key in your methods

```csharp
public class ReleasesUoW : IReleasesUoW
{
    private readonly IUnitOfWork _uow;

    public DbSet<ReleaseEntity> Releases => _uow.Set<ReleaseEntity>();

    public ReleasesUoW(IUnitOfWorkPool uowPool)
    {
        _uow = uowPool.Get("Releases");
    }

    //IUnitOfWork<ReleasesDbContext> methods
    public void CommitTransaction()
    {
        _uow.CommitTransaction();
    }

    //(...)
}
```

3. Register your custom UoW in `Startup.ConfigureServices` and inject it into your other classes

```csharp
//register in DI container
services.AddScoped<IReleasesUoW, ReleasesUoW>();

//use as a dependency in other classes instead of IUnitOfWorkPool
public MyService(IReleasesUoW uow) {}
```

---