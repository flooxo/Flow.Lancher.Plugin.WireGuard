using Flow.Launcher.Plugin;
using System;
using System.IO;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Text.Json;


namespace Flow.Launcher.Plugin.WireGuard
{
    public class WireGuardPlugin : IPlugin, IPluginI18n, ISettingProvider
    {
        internal PluginInitContext Context;

        private Settings settings;

        private const string Image = @"Images\wireguard.png";

        private IWireGuardInterfaceService interfaceService;

        /// <summary>
        /// Queries the WireGuard plugin for Flow Launcher.
        /// </summary>
        /// <param name="query">The query entered by the user.</param>
        /// <returns>A list of results based on the query.</returns>
        public List<Result> Query(Query query)
        {
            return interfaceService.GetAll()
               .Where(interface_ => interface_.name.Contains(query.Search, StringComparison.OrdinalIgnoreCase))
               .Select(interface_ => new Result
               {
                   Title = interface_.name,
                   SubTitle = interface_.path,
                   IcoPath = Image,
                   Action = _ =>
                   {
                       if (interface_.isConnected)
                       {
                           interface_.deactivate();
                       }
                       else
                       {
                           interface_.activate();
                       }

                       return true;
                   }
               })
               .ToList();
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
                interfaceService = new WireGuardInterfaceService(settings.WireGuardConfigPath);
            }

            settings.OnSettingsChanged = (s) =>
            {
                settings.Save();
                interfaceService = new WireGuardInterfaceService(settings.WireGuardConfigPath);
            };
        }

        /// <summary>
        /// Retrieves the translated plugin title.
        /// </summary>
        /// <returns>The translated plugin title.</returns>
        public string GetTranslatedPluginTitle()
        {
            return Context.API.GetTranslation("plugin_wireguard_name");
        }

        /// <summary>
        /// Retrieves the translated description of the plugin.
        /// </summary>
        /// <returns>The translated plugin description.</returns>
        public string GetTranslatedPluginDescription()
        {
            return Context.API.GetTranslation("plugin_wireguard_plugin_description");
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
}
