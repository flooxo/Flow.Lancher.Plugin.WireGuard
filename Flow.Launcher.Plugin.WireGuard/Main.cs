using System.Collections.Generic;
using Flow.Launcher.Plugin;
using System.IO;
using System.Diagnostics;

namespace Flow.Launcher.Plugin.WireGuard
{
    public class WireGuardPlugin : IPlugin, IPluginI18n
    {
        internal PluginInitContext Context;

        private const string Image = $"Images\wireguard.png";

        private string tunnelPaths = @"C:\Program Files\WireGuard\Data\Configurations\";

        public List<Result> Query(Query query)
        {
            var filePaths = Directory.GetFiles(tunnelPaths);

            var resultList = new List<Result>();
            foreach (var filePath in filePaths)
            {
                resultList.Add(new Result
                {
                    Title = GetFileNameWithoutExtensions(filePath),
                    SubTitle = filePath,
                    IcoPath = Image,
                    Action = e =>
                    {
                        //TODO: wireguard has to run?
                        var command = $"wireguard.exe /installtunnelservice \"{filePath}\"";
                        ProcessStartInfo info = new()
                        {
                            FileName = "cmd.exe",
                            Verb = "runas",
                            Arguments = $"/c {command}",
                            UseShellExecute = true,
                            WindowStyle = ProcessWindowStyle.Hidden
                        };

                        Process.Start(info);

                        return true;
                    }
                });
            }

            return resultList;
        }

        public void Init(PluginInitContext context)
        {
            Context = context;
            if (!Directory.Exists(tunnelPaths))
            {
                throw new Exception($"Plugin Wireguard: {tunnelPaths} not found.");
            }
        }

        public string GetTranslatedPluginTitle()
        {
            return Context.API.GetTranslation("plugin_wireguard_name");
        }

        public string GetTranslatedPluginDescription()
        {
            return Context.API.GetTranslation("plugin_wireguard_plugin_description");
        }

        private string GetFileNameWithoutExtensions(string filePath)
        {
            return Path.GetFileNameWithoutExtension(filePath.EndsWith(".conf.dpapi") ? Path.GetFileNameWithoutExtension(filePath) : filePath);
        }
    }
}
