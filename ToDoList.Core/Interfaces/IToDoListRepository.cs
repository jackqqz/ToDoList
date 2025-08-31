using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDoList.Core.Models;

namespace ToDoList.Core.Interfaces
{
    public interface IToDoListRepository
    {
        Task<ToDoListEntity?> GetAsync(Guid id, CancellationToken ct = default);
        Task<ToDoListEntity?> GetWithItemsAsync(Guid id, CancellationToken ct = default);
        Task<ToDoListEntity> AddAsync(ToDoListEntity list, CancellationToken ct = default);
        Task AddItemAsync(ToDoItem item, CancellationToken ct = default);

    }
}
