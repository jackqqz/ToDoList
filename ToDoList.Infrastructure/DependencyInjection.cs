using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ToDoList.Infrastructure.Data;

namespace Todo.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration cfg)
    {
        // DbContext (SQLite)
        services.AddDbContext<AppDbContext>(opt =>
            opt.UseSqlite(cfg.GetConnectionString("Sqlite") ?? "Data Source=todo.db"));

        return services;
    }
}
