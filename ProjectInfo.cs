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
        public List<string> ToDos { get; set; }

        public ProjectInfo(string Name)
        {
            this.Name = Name;
            this.Description = "Description not specified.";
            this.Tags = "Tags not specified";
            ToDos = new List<string>();
        }
    }
}
