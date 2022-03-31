using System.Text.Json;
using System.IO;
using System;

namespace Project_Manager_V2
{
    public class PreferencesHelper
    {
        public string preferencesLocation = @".\Preferences.json";

        // Get preferences from json preferences file
        public Preferences getPreferences()
        {
            if (!File.Exists(preferencesLocation)) return new Preferences();
            try
            {
                string jsonString = File.ReadAllText(preferencesLocation);
                Preferences preferences = JsonSerializer.Deserialize<Preferences>(jsonString);
                return preferences;
            }
            catch (Exception e)
            {
                Console.WriteLine("Cannot read preferences, using default preferences");
                Console.WriteLine("Error: ");
                Console.WriteLine(e);
                return new Preferences();
            }
        }

        // Write preferences to json preferences file
        public async void setPreferences(Preferences preferences)
        {
            try
            {
                using FileStream createStream = File.Create(preferencesLocation);
                await JsonSerializer.SerializeAsync(createStream, preferences);
                await createStream.DisposeAsync();
            }
            catch (Exception e)
            {
                // Unable to set preferences
                Console.WriteLine("Unable to set preferences");
                Console.WriteLine("Error: ");
                Console.WriteLine(e);
            }
        }
    }
}
