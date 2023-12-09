using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
namespace Flow.Launcher.Plugin.WireGuard
{
    public class WireGuardInterfaceService : IWireGuardInterfaceService
    {
        public List<WireGuardInterface> wireguardInterfaces { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="WireGuardInterfaceService"/> class.
        /// Populate the wireguardInterfaces list by retrieving all WireGuard configuration files in the specified path
        /// <param name="configPath">The path to the WireGuard configuration files.</param>
        /// </summary>
        public WireGuardInterfaceService(string configPath)
        {
            wireguardInterfaces = Directory.GetFiles(configPath)
                .Where(configFile => configFile.EndsWith(".conf", StringComparison.OrdinalIgnoreCase) ||
                                     configFile.EndsWith(".conf.dpapi", StringComparison.OrdinalIgnoreCase))
                .Select(config => new WireGuardInterface
                {
                    name = GetFileNameWithoutExtensions(config),
                    path = config,
                    isConnected = false
                })
                .ToList();
        }

        /// <summary>
        /// Retrieves all WireGuard interfaces.
        /// </summary>
        /// <returns>An enumerable collection of WireGuard interfaces.</returns>
        public IEnumerable<WireGuardInterface> GetAll()
        {
            return wireguardInterfaces;
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
    }
}