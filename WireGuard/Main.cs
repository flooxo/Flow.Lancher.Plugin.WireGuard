using System.Collections.Generic;
using Flow.Launcher.Plugin;
using System.IO;
using System.Diagnostics;

namespace Flow.Launcher.Plugin.WireGuard
{
    public class Main : IPlugin, IPluginI18n
    {
        internal PluginInitContext Context;

        public List<Result> Query(Query query)
        {
            var tunnelPaths = @"C:\Program Files\WireGuard\Data\Configurations\";
            var filePaths = Directory.GetFiles(tunnelPaths);

            var resultList = new List<Result>();
            foreach (var filePath in filePaths)
            {
                resultList.Add(new Result
                {
                    Title = GetFileNameWithoutExtensions(filePath),
                    SubTitle = filePath,
                    IcoPath = "Images/wireguard.png",
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
        }

        public string GetTranslatedPluginTitle()
        {
            return Context.API.GetTranslation("plugin_helloworldcsharp_plugin_name");
        }

        public string GetTranslatedPluginDescription()
        {
            return Context.API.GetTranslation("plugin_helloworldcsharp_plugin_description");
        }

        private string GetFileNameWithoutExtensions(string filePath)
        {
            return Path.GetFileNameWithoutExtension(filePath.EndsWith(".conf.dpapi") ? Path.GetFileNameWithoutExtension(filePath) : filePath);
        }
    }
}
