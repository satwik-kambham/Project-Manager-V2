using System.IO;
using System.Windows;

namespace Project_Manager_V2;

// Window to change project folder
public partial class ChangeFolderWindow
{
    // Path to folder contaning projects
    private string path;

    // Helper class to save path to the preferences
    private PreferencesHelper preferencesHelper;

    public ChangeFolderWindow(string path, PreferencesHelper preferencesHelper)
    {
        this.path = path;
        InitializeComponent();
        Path.Text = path;
        this.preferencesHelper = preferencesHelper;
    }
    
    // Check if the folder selected by user exists
    // If it exists, save and close
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
