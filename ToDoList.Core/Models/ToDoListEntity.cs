namespace ToDoList.Core.Models
{
    public class ToDoListEntity
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = "";
        public ICollection<ToDoItem> Items { get; set; } = new List<ToDoItem>();
    }
}
