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
        /// Disables the tunnel service of the connected interface if necessary because allow only one connection at a time.
        /// </summary>
        /// <param name="hasConnection">A value indicating whether another interface has an active connection.</param>
        /// <param name="connectedInterface">The connected WireGuard interface, otherwise null.</param>
        public void activate(bool hasConnection, WireGuardInterface connectedInterface)
        {
            string command = hasConnection ?
                $"wireguard.exe /uninstalltunnelservice \"{connectedInterface.name}\" && wireguard.exe /installtunnelservice \"{path}\"" :
                $"wireguard.exe /installtunnelservice \"{path}\"";

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
                if (hasConnection)
                {
                    connectedInterface.isConnected = false;
                }
            }
            catch (Exception)
            {
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
            catch (Exception)
            {
            }
        }

        public string getSubTitle(PluginInitContext Context, bool hasConnection, WireGuardInterface connectedInterface)
        {
            if (isConnected)
            {
                return Context.API.GetTranslation("plugin_wireguard_disconnect");
            }
            else if (hasConnection)
            {
                return string.Format(Context.API.GetTranslation("plugin_wireguard_switch"),
                                        connectedInterface.name);
            }
            else
            {
                return Context.API.GetTranslation("plugin_wireguard_connect");
            }
        }
    }
}