namespace ToDoList.Core.Models
{
    public class CategoryEntity
    {
        public Guid Id{ get; set; }

        public required string Name { get; set; }
    }
}
