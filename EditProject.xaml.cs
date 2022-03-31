using HandyControl.Controls;
using HandyControl.Themes;
using HandyControl.Tools;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Microsoft.Win32;

namespace Project_Manager_V2
{
    // Windows to edit selected project
    public partial class EditProject : HandyControl.Controls.Window
    {
        // Info of project to be edited
        public ProjectInfo projectInfo;

        public EditProject(ProjectInfo projectInfo)
        {
            InitializeComponent();
            this.projectInfo = projectInfo;
            if (projectInfo != null) displayProjectInfo();
        }

        // Display the information of the project in the window
        public void displayProjectInfo()
        {
            ProjectName.Content = projectInfo.Name;
            ProjectDescription.Text = projectInfo.Description;
            ProjectTags.Text = projectInfo.Tags;
            ProjectExecutablePath.Text = projectInfo.ExecutablePath;
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

        // Save changes to database and close window
        private void SaveChanges(object sender, RoutedEventArgs e)
        {
            if (projectInfo == null) this.Close();

            projectInfo.Description = ProjectDescription.Text;
            projectInfo.Tags = ProjectTags.Text;
            if (Image_1.Uri != null) projectInfo.ScreenshotPath = Image_1.Uri.ToString();
            this.Close();
        }

        // Close window without saving changes
        private void CancelChanges(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        
        // Open dialog to select executable of project
        private void SelectExecutable(object sender, RoutedEventArgs e)
        {
            var executableDialog = new OpenFileDialog();
            executableDialog.Filter = "Executable (.exe)|*.exe";

            if (executableDialog.ShowDialog(this) == true)
            {
                var executablePath = executableDialog.FileName;
                projectInfo.ExecutablePath = executablePath;
                ProjectExecutablePath.Text = executablePath;
            }
        }
    }
}
