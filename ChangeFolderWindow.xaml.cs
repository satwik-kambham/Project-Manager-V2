using System.IO;
using System.Windows;

namespace Project_Manager_V2;

public partial class ChangeFolderWindow
{
    private string path;
    private PreferencesHelper preferencesHelper;
    public ChangeFolderWindow(string path, PreferencesHelper preferencesHelper)
    {
        this.path = path;
        InitializeComponent();
        Path.Text = path;
        this.preferencesHelper = preferencesHelper;
    }
    
    private void ValidateFolder(object sender, RoutedEventArgs e)
    {
        path = Path.Text;
        if (!Directory.Exists(path))
            Invalid.Visibility = Visibility.Visible;
        else
        {
            Preferences preferences = preferencesHelper.getPreferences();
            preferences.projectPath = Path.Text;
            preferencesHelper.setPreferences(preferences);
            CloseButton.Command?.Execute(null);
        }
    }
}