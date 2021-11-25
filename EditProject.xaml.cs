using HandyControl.Controls;
using HandyControl.Themes;
using HandyControl.Tools;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Project_Manager_V2
{
    public partial class EditProject : HandyControl.Controls.Window
    {
        public ProjectInfo projectInfo;
        public EditProject(ProjectInfo projectInfo)
        {
            InitializeComponent();
            this.projectInfo = projectInfo;
            if (projectInfo != null) displayProjectInfo();
        }

        public void displayProjectInfo()
        {
            ProjectName.Content = projectInfo.Name;
            ProjectDescription.Text = projectInfo.Description;
            ProjectTags.Text = projectInfo.Tags;
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

        private void SaveChanges(object sender, RoutedEventArgs e)
        {
            if (projectInfo == null) this.Close();

            projectInfo.Description = ProjectDescription.Text;
            projectInfo.Tags = ProjectTags.Text;
            if (Image_1.Uri != null) projectInfo.ScreenshotPath = Image_1.Uri.ToString();
            this.Close();
        }

        private void CancelChanges(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
