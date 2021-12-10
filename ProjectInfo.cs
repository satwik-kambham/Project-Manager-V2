using System.Collections.Generic;

namespace Project_Manager_V2
{
    public class ProjectInfo
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Tags { get; set; }
        public string ScreenshotPath { get; set; }
        public string ExecutablePath { get; set; }
        public List<ToDoItem> ToDos { get; set; }

        public ProjectInfo(string Name)
        {
            this.Name = Name;
            this.Description = "Description not specified.";
            this.Tags = "Tags not specified";
            ToDos = new List<ToDoItem>();
        }

        public void AddToDoItem(string task) =>
            ToDos.Add(new ToDoItem() { Task = task });

        public bool RemoveToDoItem(string task)
        {
            return ToDos.Remove(ToDos.Find(x => x.Task == task));
        }

        public void ToggleToDoItem(string task)
        {
            ToDoItem toDoItem = ToDos.Find(x => x.Task == task);
            toDoItem.Done = !toDoItem.Done;
        }
    }
}
