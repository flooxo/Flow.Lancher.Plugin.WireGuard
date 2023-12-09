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

        public WireGuardInterfaceService(string configPath)
        {
            wireguardInterfaces = new List<WireGuardInterface>(Directory.GetFiles(configPath).Length);

            var wireguardConfigFiles = Directory.GetFiles(configPath);
            wireguardConfigFiles = Array.FindAll(wireguardConfigFiles, wireguardConfigFile =>
                wireguardConfigFile.EndsWith(".conf", StringComparison.OrdinalIgnoreCase) ||
                wireguardConfigFile.EndsWith(".conf.dpapi", StringComparison.OrdinalIgnoreCase));

            foreach (var config in wireguardConfigFiles)
            {
                wireguardInterfaces.Add(
                    new WireGuardInterface
                    {
                        name = GetFileNameWithoutExtensions(config),
                        path = config,
                        isConnected = false
                    });
            }
        }

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