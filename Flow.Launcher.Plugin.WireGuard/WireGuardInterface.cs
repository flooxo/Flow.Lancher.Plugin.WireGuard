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
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the path of the WireGuard configuration file.
        /// </summary>
        public string Path { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the interface is connected.
        /// </summary>
        public bool IsConnected { get; set; }

        /// <summary>
        /// Activates the WireGuard interface by installing the tunnel service.
        /// Disables the tunnel service of the connected interface if necessary because allow only one connection at a time.
        /// </summary>
        /// <param name="hasConnection">A value indicating whether another interface has an active connection.</param>
        /// <param name="connectedInterface">The connected WireGuard interface, otherwise null.</param>
        public void Activate(bool hasConnection, WireGuardInterface connectedInterface)
        {
            string command = hasConnection ?
                $"wireguard.exe /uninstalltunnelservice \"{connectedInterface.Name}\" && wireguard.exe /installtunnelservice \"{Path}\"" :
                $"wireguard.exe /installtunnelservice \"{Path}\"";

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
                IsConnected = true;
                if (hasConnection)
                {
                    connectedInterface.IsConnected = false;
                }
            }
            catch (Exception)
            {
            }
        }

        /// <summary>
        /// Deactivates the WireGuard interface by uninstalling the tunnel service.
        /// </summary>
        public void Deactivate()
        {
            string command = $"wireguard.exe /uninstalltunnelservice \"{Name}\"";

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
                IsConnected = false;
            }
            catch (Exception)
            {
            }
        }

        public string GetSubTitle(PluginInitContext Context, bool hasConnection, WireGuardInterface connectedInterface)
        {
            if (IsConnected)
            {
                return Context.API.GetTranslation("plugin_wireguard_disconnect");
            }
            else if (hasConnection)
            {
                return string.Format(Context.API.GetTranslation("plugin_wireguard_switch"),
                                        connectedInterface.Name);
            }
            else
            {
                return Context.API.GetTranslation("plugin_wireguard_connect");
            }
        }
    }
}