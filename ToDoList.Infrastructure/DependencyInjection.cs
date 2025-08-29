using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ToDoList.Core.Interfaces;
using ToDoList.Infrastructure.Data;
using ToDoList.Infrastructure.Repositories;

namespace TodoList.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration cfg)
    {
        // DbContext (SQLite)
        services.AddDbContext<AppDbContext>(opt =>
            opt.UseSqlite(cfg.GetConnectionString("Sqlite") ?? "Data Source=todo.db"));

        services.AddScoped<IToDoItemRepository, ToDoItemRepository>();
        return services;
    }
}
