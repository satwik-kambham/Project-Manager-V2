using System;
using System.IO;

namespace Project_Manager_V2
{
    public class Preferences
    {
        public bool darkMode { get; set; }
        public string projectPath { get; set; }

        // Set default values for Preferences
        public Preferences()
        {
            darkMode = false;
            projectPath = Path.Combine(Environment.GetFolderPath(
                Environment.SpecialFolder.DesktopDirectory), 
                "Projects");
        }
    }
}
