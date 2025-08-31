using Microsoft.EntityFrameworkCore;
using ToDoList.Core.Interfaces;
using ToDoList.Core.Models;
using ToDoList.Infrastructure.Data;

namespace ToDoList.Infrastructure.Repositories
{
    public class CategoryRepository(AppDbContext dbContext) : ICategoryRepository
    {
        public async Task<IEnumerable<CategoryEntity>> GetAllCategories()
        {
            return await dbContext.ToDoCategories
                           .AsNoTracking()
                           .OrderBy(c => c.Name)
                           .ToListAsync();
        }

        public async Task<CategoryEntity> GetCategoryById(Guid id)
        {
            return await dbContext.ToDoCategories
                           .AsNoTracking()
                           .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<CategoryEntity> AddAsync(CategoryEntity category, CancellationToken ct = default)
        {
            dbContext.ToDoCategories.Add(category);
            await dbContext.SaveChangesAsync(ct);
            return category;
        }
        public async Task<CategoryEntity> UpdateCategory(Guid CategoryId, CategoryEntity category)
        {
            var categoryFound = await dbContext.ToDoCategories.FirstOrDefaultAsync(t => t.Id == CategoryId);

            if (categoryFound is not null)
            {
                categoryFound.Name = category.Name;

                await dbContext.SaveChangesAsync();

                return categoryFound;
            }

            return category;
        }

        public async Task<bool> DeleteCategory(Guid categoryId)
        {
            var category = await dbContext.ToDoCategories.FirstOrDefaultAsync(t => t.Id == categoryId);

            if (category is not null)
            {
                dbContext.ToDoCategories.Remove(category);

                return await dbContext.SaveChangesAsync() > 0;

            }

            return false;
        }

        public Task<bool> ExistsByNameAsync(string name, CancellationToken ct = default)
        {
            // Adjust for case-insensitivity if you store normalized names
            return dbContext.ToDoCategories.AnyAsync(c => c.Name == name, ct);
        }

        public IQueryable<CategoryEntity> Query() => dbContext.ToDoCategories.AsNoTracking().AsQueryable();
    }
}
