using ToDoList.Core.Models;

namespace ToDoList.Core.Interfaces
{
    public interface ICategoryRepository
    {
        Task<IEnumerable<CategoryEntity>> GetAllCategories();
        Task<CategoryEntity> GetCategoryById(Guid id);
        Task<CategoryEntity> AddAsync(CategoryEntity list, CancellationToken ct = default);
        Task<CategoryEntity> UpdateCategory(Guid categoryId, CategoryEntity category);
        Task<bool> DeleteCategory(Guid categoryId);
        Task<bool> ExistsByNameAsync(string name, CancellationToken ct = default);
        IQueryable<CategoryEntity> Query();
    }
}
