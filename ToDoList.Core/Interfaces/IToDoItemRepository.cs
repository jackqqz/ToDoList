using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDoList.Core.Models;

namespace ToDoList.Core.Interfaces
{
    public interface IToDoItemRepository
    {
        Task<IEnumerable<ToDoItem>> GetToDoItems();
        Task<ToDoItem> GetToDoItemsById(Guid id);
        Task<ToDoItem> AddToDoItem(ToDoItem item);
        Task<ToDoItem> UpdateToDoItem(Guid ToDoItemId, ToDoItem item);
        Task<bool> DeleteToDoItemAsync(Guid itemId);

    }
}
