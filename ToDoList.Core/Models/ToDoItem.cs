using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToDoList.Core.Models
{
    public class ToDoItem
    {
        public Guid Id { get; set; }
        public required String Title { get; set; }
        public String? Description { get; set; }
        public bool IsCompleted { get; set; } = false;
        public DateOnly DueDate { get; set; }
        public DateTime CreatedAt { get; set; }

        public Guid ToDoListId { get; set; }
        public Guid? CategoryId { get; set; }

    }
}
