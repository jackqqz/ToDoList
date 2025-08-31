using Microsoft.EntityFrameworkCore;
using ToDoList.Core.Interfaces;
using ToDoList.Core.Models;
using ToDoList.Infrastructure.Data;

namespace ToDoList.Infrastructure.Repositories;

public class ToDoListRepository(AppDbContext db) : IToDoListRepository
{
    public Task<ToDoListEntity?> GetAsync(Guid id, CancellationToken ct = default) =>
        db.ToDoLists.FirstOrDefaultAsync(l => l.Id == id, ct);

    public Task<ToDoListEntity?> GetWithItemsAsync(Guid id, CancellationToken ct = default) =>
        db.ToDoLists.Include(l => l.Items).FirstOrDefaultAsync(l => l.Id == id, ct);

    public async Task<ToDoListEntity> AddAsync(ToDoListEntity list, CancellationToken ct = default)
    {
        db.ToDoLists.Add(list);
        await db.SaveChangesAsync(ct);
        return list;
    }

    public async Task AddItemAsync(ToDoItem item, CancellationToken ct = default)
    {
        db.ToDoItems.Add(item);
        await db.SaveChangesAsync(ct);
    }
}
