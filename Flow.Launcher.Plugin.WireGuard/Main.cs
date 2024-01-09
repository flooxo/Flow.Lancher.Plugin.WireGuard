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
            var interfaces = interfaceService.GetAll()
                .Where(interface_ => interface_.Name.Contains(query.Search, StringComparison.OrdinalIgnoreCase));
            var connectedInterface = interfaces.FirstOrDefault(interface_ => interface_.IsConnected);
            var hasConnection = connectedInterface != null;

            var results = interfaces
                .Where(interface_ => interface_ != connectedInterface)
                .Select(interface_ => new Result
                {
                    Title = interface_.Name,
                    SubTitle = interface_.GetSubTitle(Context, hasConnection, connectedInterface),
                    IcoPath = Image,
                    Score = 0,
                    Action = _ =>
                    {
                        if (interface_.IsConnected)
                        {
                            interface_.Deactivate();
                        }
                        else
                        {
                            interface_.Activate(hasConnection, connectedInterface);
                        }

                        return true;
                    }
                })
               .ToList();

            //Set disconnect as top result if available
            if (connectedInterface != null)
            {
                var topResult = new Result
                {
                    Title = connectedInterface.Name,
                    SubTitle = connectedInterface.GetSubTitle(Context, hasConnection, connectedInterface),
                    IcoPath = Image,
                    Score = 1,
                    Action = _ =>
                    {
                        connectedInterface.Deactivate();
                        return true;
                    }
                };
                results.Insert(0, topResult);
            }

            return results;
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
                    SettingsFileLocation = settingsFileLocation,
                    WireGuardConfigPath = @"C:\Program Files\WireGuard\Data\Configurations"
                };

                settings.Save();

                interfaceService = new WireGuardInterfaceService(settings.WireGuardConfigPath);
            }
            else
            {
                settings = JsonSerializer.Deserialize<Settings>(File.ReadAllText(settingsFileLocation));
                settings.SettingsFileLocation = settingsFileLocation;
                interfaceService = new WireGuardInterfaceService(settings.WireGuardConfigPath);
            }

            settings.OnSettingsChanged = (s) =>
            {
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
