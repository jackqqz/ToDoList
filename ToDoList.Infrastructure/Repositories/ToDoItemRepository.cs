using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDoList.Core.Interfaces;
using ToDoList.Core.Models;
using ToDoList.Infrastructure.Data;

namespace ToDoList.Infrastructure.Repositories
{
    public class ToDoItemRepository(AppDbContext dbContext) : IToDoItemRepository
    {
        public async Task<IEnumerable<ToDoItem>> GetToDoItems()
        {
            return await dbContext.ToDoItems.ToListAsync();
        }

        public async Task<ToDoItem> GetToDoItemsById(Guid id)
        {
            return await dbContext.ToDoItems.FirstOrDefaultAsync(t => t.Id == id);
        }

        public async Task<ToDoItem> AddToDoItem(ToDoItem item)
        {
            item.Id = Guid.NewGuid();
            dbContext.ToDoItems.Add(item);

            await dbContext.SaveChangesAsync();

            return item;
        }
        public async Task<ToDoItem> UpdateToDoItem(Guid ToDoItemId, ToDoItem item)
        {
            var toDoItem = await dbContext.ToDoItems.FirstOrDefaultAsync(t => t.Id == ToDoItemId);

            if (toDoItem is not null)
            {
                toDoItem.Title = item.Title;
                toDoItem.Description = item.Description;
                toDoItem.IsCompleted = item.IsCompleted;

                await dbContext.SaveChangesAsync();

                return toDoItem;
            }

            return item;
        }

        public async Task<bool> DeleteToDoItemAsync(Guid itemId)
        {
            var toDoItem = await dbContext.ToDoItems.FirstOrDefaultAsync(t => t.Id == itemId);

            if (toDoItem is not null)
            {
                dbContext.ToDoItems.Remove(toDoItem);

                return await dbContext.SaveChangesAsync() > 0;

            }

            return false;
        }
    }
}
