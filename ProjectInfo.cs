using System.Collections.Generic;

namespace Project_Manager_V2
{
    public class ProjectInfo
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Tags { get; set; }
        public List<string[]> ScreenshotPaths { get; set; }
        public List<string[]> ToDos { get; set; }
    }
}
