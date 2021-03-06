# Getting Started

## What is Cactus.Blade.Repository

A Generic Unit of Work and Repository pattern implementation framework for Entity Framework Core, to assist developers to implementing the [Generic Repository Pattern .net core](https://garywoodfine.com/generic-repository-pattern-net-core).

### Dependency Injection

ASP.NET Core supports the dependency injection (DI) software design pattern, which is a technique for achieving Inversion of Control (IoC) between classes and their dependencies.

### What is Dependency Injection

Dependency injection is basically a mechanism of providing objects that an object needs (its dependencies) instead of having it construct them itself. It's a very useful technique for testing, since it allows dependencies to be mocked or stubbed out.

Dependencies can be injected into objects by many means (such as constructor injection or setter injection). One can even use specialized dependency injection frameworks (e.g. SimpleInjector, Autofac, StructureMap) to do that, but they certainly aren't required.

The .net core framework comes with a built in DI container. Cactus.Blade.Repository is able to be used as an extension of the DI container and you can use this to configure and inject Cactus.Blade.Repository and its Unit of Work interface as a dependency.

Dependency injection addresses a few issues in software development

- The use of an interface or base class to abstract the dependency implementation.
- Registration of the dependency in a service container. ASP.NET Core provides a built-in service container, `IServiceProvider`. Services are registered in the applications `Startup.ConfigureServices` method.
- Injection of the service into the constructor of the class where it's used. The framework takes on the responsibility of creating an instance of the dependency and disposing of it when it's no longer needed.

### How to use Cactus.Blade.Repository.DependencyInjection

Once you have added the Nuget Package to your project, you can edit your `Startup.cs` and import `using Cactus.Blade.Repository.DependencyInjection;`

In the example we are just going to use a Connection String that we have declared in our `appsettings.json` file which we have simply called `SampleDB`.

We have also made use of Microsoft SQL Server (MS SQL) for the example, but this can be any Relational Database Management System (RDBMS) of your choice i.e. mySQL, Postgres SQL, oracle etc.

We have simply used MS SQL for ease of illustration.

```c#
public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Use the Cactus.Blade.Repository Dependency Injection to set up the Unit of Work
            services.AddDbContext<SampleContext>(options =>
                options.UseNpgsql(Configuration.GetConnectionString("SampleDB")))
                .AddUnitOfWork<SampleContext>();

            services.AddMvc();
        }
```

We can use any of the Entity Framework Core supported database drivers, in the example about we made use of PostgreSql.
Once the Dependency Injection has been configured. You can now simply make use of the Unit of Work to access your
repositories via Dependency Injection.

```c#

 public class AddressService : IService<Address>
    {
        private readonly IUnitOfWork _unitOfWork;
        public AddressService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public Address GetAddressDetail(int id)
        {
           return _unitOfWork.GetReadOnlyRepository<Address>().SingleOrDefault(x => x.Id == id);

        }
    }

```
