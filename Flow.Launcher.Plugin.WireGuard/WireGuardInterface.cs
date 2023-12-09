using System;
using System.Diagnostics;
using System.Collections.Generic;

namespace Flow.Launcher.Plugin.WireGuard
{
    public class WireGuardInterface
    {
        /// <summary>
        /// Gets or sets the name of the interface.
        /// </summary>
        public string name { get; set; }

        /// <summary>
        /// Gets or sets the path of the WireGuard configuration file.
        /// </summary>
        public string path { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the interface is connected.
        /// </summary>
        public bool isConnected { get; set; }

        /// <summary>
        /// Activates the WireGuard interface by installing the tunnel service.
        /// </summary>
        public void activate()
        {
            string command = $"wireguard.exe /installtunnelservice \"{path}\"";

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
                isConnected = true;
            }
            catch (Exception e)
            {
                //Log.Error($"Failed to install tunnel service for {tunnelPath}");
                //Log.Exception(e);
            }
        }

        /// <summary>
        /// Deactivates the WireGuard interface by uninstalling the tunnel service.
        /// </summary>
        public void deactivate()
        {
            string command = $"wireguard.exe /uninstalltunnelservice \"{name}\"";

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
                isConnected = false;
            }
            catch (Exception e)
            {
                //Log.Error($"Failed to uninstall tunnel service for {name}");
                //Log.Exception(e);
            }
        }

        public string getSubTitle(PluginInitContext Context)
        {
            if (isConnected)
            {
                return Context.API.GetTranslation("plugin_wireguard_disconnect");
            }
            else
            {
                return Context.API.GetTranslation("plugin_wireguard_connect");
            }
        }
    }
}