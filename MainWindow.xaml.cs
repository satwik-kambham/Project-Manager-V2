using HandyControl.Controls;
using HandyControl.Themes;
using HandyControl.Tools;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.IO;
using System;

namespace Project_Manager_V2
{
    public partial class MainWindow
    {
        public string ProjectsDirectoryPath { get; set; }

        public MainWindow()
        {
            InitializeComponent();
            ThemeManager.Current.ApplicationTheme = ApplicationTheme.Dark;
            // Setting default projects folder path
            ProjectsDirectoryPath = 
                Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory), "Projects");
            if (!Directory.Exists(ProjectsDirectoryPath))
                Directory.CreateDirectory(ProjectsDirectoryPath);
            InitializeProjectsList();
        }

        // Finds projects in folder and adds them to projects list
        public void InitializeProjectsList()
        {
            ProjectsList.Items.Clear();
            foreach (var project in Directory.GetDirectories(ProjectsDirectoryPath))
            {
                ProjectsList.Items.Add(Path.GetRelativePath(ProjectsDirectoryPath, project));
            }
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
        private void EditClicked(object sender, RoutedEventArgs e)
        {
            if (ProjectsList.SelectedIndex != -1)
            {

            }
        }
    }
}
