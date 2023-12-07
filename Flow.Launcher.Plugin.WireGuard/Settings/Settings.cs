using System;
using System.IO;
using System.Text.Json;

namespace Flow.Launcher.Plugin.WireGuard
{
    public class Settings
    {
        internal string SettingsFileLocation;
        public string WireGuardConfigPath { get; set; }

        internal Action<Settings> OnSettingsChanged { get; set; }

        /// <summary>
        /// Saves the current settings to the settings file.
        /// </summary>
        internal void Save()
        {
            File.WriteAllText(SettingsFileLocation, JsonSerializer.Serialize(this));
        }
    }
}