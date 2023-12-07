using Flow.Launcher.Plugin;
using System;
using System.Text.Json;
using System.IO;
using System.Diagnostics;
using System.Collections;
using System.Collections.Generic;

namespace Flow.Launcher.Plugin.WireGuard
{
    public class WireGuardPlugin : IPlugin, IPluginI18n, ISettingProvider
    {
        internal PluginInitContext Context;
        private Settings settings;

        private const string Image = @"Images\wireguard.png";

        private string configPath = @"C:\Program Files\WireGuard\Data\Configurations\";
        private string[] tunnelPaths = Array.Empty<string>();

        /// <summary>
        /// Queries the WireGuard plugin for Flow Launcher.
        /// </summary>
        /// <param name="query">The query entered by the user.</param>
        /// <returns>A list of results based on the query.</returns>
        public List<Result> Query(Query query)
        {
            var resultList = new List<Result>(tunnelPaths.Length);

            foreach (var tunnelPath in tunnelPaths)
            {
                resultList.Add(new Result
                {
                    Title = GetFileNameWithoutExtensions(tunnelPath),
                    SubTitle = tunnelPath,
                    IcoPath = Image,
                    Action = e =>
                    {
                        //TODO: wireguard has to run?
                        getProcess(tunnelPath, Command.install);
                        return true;
                    }
                });
            }

            return resultList;
        }

        /// <summary>
        /// Initializes the WireGuard plugin for Flow Launcher.
        /// </summary>
        /// <param name="context">The plugin initialization context.</param>
        public void Init(PluginInitContext context)
        {
            Context = context;

            var settingsFolderLocation =
                Path.Combine(
                    Directory.GetParent(
                        Directory.GetParent(context.CurrentPluginMetadata.PluginDirectory).FullName)
                    .FullName,
                    "Settings", "Plugins", "Flow.Launcher.Plugin.WireGuard");

            var settingsFileLocation = Path.Combine(settingsFolderLocation, "Settings.json");

            if (!Directory.Exists(settingsFolderLocation))
            {
                Directory.CreateDirectory(settingsFolderLocation);

                settings = new Settings
                {
                    SettingsFileLocation = settingsFileLocation
                };

                settings.Save();
            }
            else
            {
                settings = JsonSerializer.Deserialize<Settings>(File.ReadAllText(settingsFileLocation));
                settings.SettingsFileLocation = settingsFileLocation;
            }

            settings.OnSettingsChanged = (s) => settings.Save();

            if (!Directory.Exists(configPath))
            {
                //Log.Exception($"Plugin Wireguard: {configPath} not found."); //TODO: log
            }
            else
            {
                tunnelPaths = Directory.GetFiles(configPath);
                tunnelPaths = Array.FindAll(tunnelPaths, tunnelPath =>
                    tunnelPath.EndsWith(".conf", StringComparison.OrdinalIgnoreCase) ||
                    tunnelPath.EndsWith(".conf.dpapi", StringComparison.OrdinalIgnoreCase));
            }
        }

        /// <summary>
        /// Gets the file name without extensions.
        /// </summary>
        /// <param name="filePath">The file path.</param>
        /// <returns>The file name without extensions.</returns>
        private string GetFileNameWithoutExtensions(string filePath)
        {
            return Path.GetFileNameWithoutExtension(filePath.EndsWith(".conf.dpapi") ? Path.GetFileNameWithoutExtension(filePath) : filePath);
        }

        private void getProcess(string tunnelPath, Command commandType)
        {
            string command = "";
            if (commandType == Command.install)
            {
                command = $"wireguard.exe /installtunnelservice \"{tunnelPath}\"";
            }
            else if (commandType == Command.uninstall)
            {
                var tunnelName = GetFileNameWithoutExtensions(tunnelPath);
                command = $"wireguard.exe /uninstall \"{tunnelName}\"";
            }
            ProcessStartInfo info = new()
            {
                FileName = "cmd.exe",
                Verb = "runas",
                Arguments = $"/c {command}",
                UseShellExecute = true,
                WindowStyle = ProcessWindowStyle.Hidden
            };

            try
            {
                Process.Start(info);
            }
            catch (Exception e)
            {
                //Log.Error($"Failed to install tunnel service for {tunnelPath}");
                //Log.Exception(e);
            }
        }

        /// <summary>
        /// Creates the setting panel for the WireGuard plugin.
        /// </summary>
        /// <returns>The setting panel control.</returns>
        public System.Windows.Controls.Control CreateSettingPanel()
        {
            return new WireGuardSettings(settings);
        }
    }

    enum Command
    {
        install,
        uninstall
    }
}
        uninstall
    }
}
