using HandyControl.Controls;
using HandyControl.Themes;
using HandyControl.Tools;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.IO;
using System;

namespace Project_Manager_V2
{
    // Main window
    public partial class MainWindow
    {
        // Path to the directory contaning the project folders
        public string ProjectsDirectoryPath { get; set; }

        // Helper class for interaction with database
        public DBHelper DBHelper;

        // The current selected project
        public ProjectInfo SelectedProject;

        // Helper class for getting and setting app preferences 
        public PreferencesHelper PreferencesHelper;

        public MainWindow()
        {
            InitializeComponent();

            // Apply preferences and get all projects in directory
            PreferencesHelper = new PreferencesHelper();
            Preferences preferences = PreferencesHelper.getPreferences();
            if (preferences.darkMode)
                ThemeManager.Current.ApplicationTheme = ApplicationTheme.Dark;

            ProjectsDirectoryPath = preferences.projectPath;
            if (!Directory.Exists(ProjectsDirectoryPath))
                try
                {
                    Directory.CreateDirectory(ProjectsDirectoryPath);
                }
                catch (Exception e)
                {
                    Console.WriteLine("Project directory does not exist. Unable to create directory!");
                    Console.WriteLine("Error: ");
                    Console.WriteLine(e);
                }
            InitializeProjectsList();

            DBHelper = new DBHelper();
        }

        #region Change Theme
        private void ButtonConfig_OnClick(object sender, RoutedEventArgs e) => PopupConfig.IsOpen = true;

        private void ButtonSkins_OnClick(object sender, RoutedEventArgs e)
        {
            if (e.OriginalSource is Button button)
            {
                PopupConfig.IsOpen = false;
                if (button.Tag is ApplicationTheme tag)
                {
                    ((App)Application.Current).UpdateTheme(tag);
                    Preferences preferences = PreferencesHelper.getPreferences();
                    if (tag == ApplicationTheme.Dark) preferences.darkMode = true;
                    else preferences.darkMode = false;
                    PreferencesHelper.setPreferences(preferences);
                }
                else if (button.Tag is Brush accentTag)
                {
                    ((App)Application.Current).UpdateAccent(accentTag);
                }
                else if (button.Tag is "Picker")
                {
                    var picker = SingleOpenHelper.CreateControl<ColorPicker>();
                    var window = new PopupWindow
                    {
                        PopupElement = picker,
                        WindowStartupLocation = WindowStartupLocation.CenterScreen,
                        AllowsTransparency = true,
                        WindowStyle = WindowStyle.None,
                        MinWidth = 0,
                        MinHeight = 0,
                        Title = "Select Accent Color"
                    };

                    picker.SelectedColorChanged += delegate
                    {
                        ((App)Application.Current).UpdateAccent(picker.SelectedBrush);
                        window.Close();
                    };
                    picker.Canceled += delegate { window.Close(); };
                    window.Show();
                }
            }
        }
        #endregion

        // Finds projects in folder and adds them to projects list
        public void InitializeProjectsList()
        {
            ProjectsList.Items.Clear();
            try
            {
                foreach (var project in Directory.GetDirectories(ProjectsDirectoryPath))
                {
                    ProjectsList.Items.Add(Path.GetRelativePath(ProjectsDirectoryPath, project));
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Error: ");
                Console.WriteLine(e);
            }
        }

        // Open edit project window
        private void EditClicked(object sender, RoutedEventArgs e)
        {
            if (ProjectsList.SelectedIndex != -1)
            {
                var editProjectWindow = new EditProject(SelectedProject);
                editProjectWindow.Owner = this;
                editProjectWindow.Closed += (s, e) =>
                {
                    DBHelper.SetProjectInfo(SelectedProject);
                    setText();
                };
                editProjectWindow.ShowDialog();
            }
        }

        // Update window with newly selected project
        private void ProjectSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SelectedProject = DBHelper.GetProjectInfo(ProjectsList.SelectedItem.ToString());
            if (SelectedProject == null)
            {
                SelectedProject = new ProjectInfo(ProjectsList.SelectedItem.ToString());
                DBHelper.SetProjectInfo(SelectedProject);
            }
            setText();
        }

        // Update window with selected project's details
        private void setText()
        {
            if (SelectedProject != null)
            {
                ProjectName.Text = SelectedProject.Name;
                ProjectDescription.Text = SelectedProject.Description;
                ProjectTags.Content = SelectedProject.Tags;
                if (SelectedProject.ScreenshotPath != null && SelectedProject.ScreenshotPath != String.Empty)
                {
                    Image_1.Visibility = Visibility.Visible;
                    BitmapImage image = new BitmapImage();
                    image.BeginInit();
                    image.UriSource = new Uri(SelectedProject.ScreenshotPath);
                    image.EndInit();
                    Image_1.Source = image;
                }
                else
                {
                    Image_1.Visibility = Visibility.Hidden;
                }
                ToDoList.Items.Clear();
                var x = DBHelper.GetProjectInfo(SelectedProject.Name);
                var toDoItems = DBHelper.GetProjectInfo(SelectedProject.Name).ToDos;
                foreach (var item in toDoItems)
                {
                    var checkBoxItem = new CheckBox();
                    checkBoxItem.Content = item.Task;
                    checkBoxItem.IsChecked = item.Done;
                    checkBoxItem.Checked += new RoutedEventHandler((s, e) =>
                    {
                        SelectedProject.ToggleToDoItem(item.Task);
                        DBHelper.SetProjectInfo(SelectedProject);
                    });
                    checkBoxItem.Unchecked += new RoutedEventHandler((s, e) =>
                    {
                        SelectedProject.ToggleToDoItem(item.Task);
                        DBHelper.SetProjectInfo(SelectedProject);
                    });
                    ToDoList.Items.Add(checkBoxItem);
                }
            }
        }

        // Backup the database and erase all data
        private void BackupAndReset(object sender, RoutedEventArgs e)
        {
            DBHelper.backupDB();
            Dialog.Show(new RestartWindow());
        }

        // Open selected project in Visual Studio Code
        private void OpenInVSCode(object sender, RoutedEventArgs e)
        {
            if (SelectedProject != null)
                System.Diagnostics.Process.Start("cmd.exe",
                    "-Command code.cmd \'" +
                    Path.Combine(ProjectsDirectoryPath, SelectedProject.Name) + "\'");
        }

        // Open selected project in Atom
        private void OpenInAtom(object sender, RoutedEventArgs e)
        {
            if (SelectedProject != null)
                System.Diagnostics.Process.Start("cmd.exe",
                    "-Command atom.cmd \'" +
                    Path.Combine(ProjectsDirectoryPath, SelectedProject.Name) + "\'");
        }

        // Open selected project in powershell (latest version from Store)
        private void OpenInTerminal(object sender, RoutedEventArgs e)
        {
            if (SelectedProject != null)
                System.Diagnostics.Process.Start("pwsh.exe",
                    "-WorkingDirectory \"" + Path.Combine(ProjectsDirectoryPath, SelectedProject.Name) + "\"");

        }

        // Open selected project in the file explorer
        private void OpenInExplorer(object sender, RoutedEventArgs e)
        {
            if (SelectedProject != null)
                System.Diagnostics.Process.Start("explorer.exe",
                Path.Combine(ProjectsDirectoryPath, SelectedProject.Name));
        }

        // Add a todo / task to the selected project
        private void addTask(object sender, RoutedEventArgs e)
        {
            if (SelectedProject != null)
            {
                var name = ToDoName.Text;
                var checkBoxItem = new CheckBox();
                checkBoxItem.Content = name;
                checkBoxItem.Checked += new RoutedEventHandler((s, e) =>
                {
                    SelectedProject.ToggleToDoItem(name);
                    DBHelper.SetProjectInfo(SelectedProject);
                });
                checkBoxItem.Unchecked += new RoutedEventHandler((s, e) =>
                {
                    SelectedProject.ToggleToDoItem(name);
                    DBHelper.SetProjectInfo(SelectedProject);
                });
                SelectedProject.AddToDoItem(name);
                ToDoList.Items.Add(checkBoxItem);
                DBHelper.SetProjectInfo(SelectedProject);
            }
        }

        // Remove selected todo / task from the selected project
        private void removeTask(object sender, RoutedEventArgs e)
        {
            if (SelectedProject != null && ToDoList.SelectedItem != null)
            {
                var name = (string)((CheckBox)ToDoList.SelectedItem).Content;
                ToDoList.Items.Remove((CheckBox)ToDoList.SelectedItem);
                SelectedProject.RemoveToDoItem(name);
                DBHelper.SetProjectInfo(SelectedProject);
            }
        }

        // Change the project directory folder and backup and reset datbase
        private void ChangeFolder(object sender, RoutedEventArgs e)
        {
            Dialog.Show(new RestartWindow());
            Dialog.Show(new ChangeFolderWindow(ProjectsDirectoryPath, PreferencesHelper));
            DBHelper.backupDB();
        }

        // Run the executable for selected project
        private void OpenExecutable(object sender, RoutedEventArgs e)
        {
            if (SelectedProject != null)
            {
                System.Diagnostics.Process.Start(SelectedProject.ExecutablePath);
            }
        }
    }
}
