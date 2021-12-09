using System.Text.Json;
using System.IO;

namespace Project_Manager_V2
{
    public class PreferencesHelper
    {
        public string preferencesLocation = @".\Preferences.json";

        public Preferences getPreferences()
        {
            if (!File.Exists(preferencesLocation)) return new Preferences();

            string jsonString = File.ReadAllText(preferencesLocation);
            Preferences preferences = JsonSerializer.Deserialize<Preferences>(jsonString);
            return preferences;
        }

        public async void setPreferences(Preferences preferences)
        {
            using FileStream createStream = File.Create(preferencesLocation);
            await JsonSerializer.SerializeAsync(createStream, preferences);
            await createStream.DisposeAsync();
        }
    }
}
