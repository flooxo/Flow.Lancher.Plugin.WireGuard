using System.Collections.Generic;

namespace Flow.Launcher.Plugin.WireGuard
{
    public interface IWireGuardInterfaceService
    {
        /// <summary>
        /// Gets or sets the list of WireGuard interfaces.
        /// </summary>
        List<WireGuardInterface> WireguardInterfaces { get; set; }

        /// <summary>
        /// Returns a List of all WireGuard Interfaces.
        /// </summary>
        /// <returns>A List of all WireGuard Interfaces</returns>
        IEnumerable<WireGuardInterface> GetAll();
    }
}