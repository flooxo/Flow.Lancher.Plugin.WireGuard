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
            //todo: get all wireguard connections
            // TODO: verschieben nach init, variabel machen
            // path can be .conf or .conf.dpapi
            var tunnelPaths = @"C:\Program Files\WireGuard\Data\Configurations\";            
            var filePaths = Directory.GetFiles(tunnelPaths);

            var resultList = new List<Result>();
            foreach (var filePath in filePaths)
            {
                resultList.Add(new Result
                {
                    Title = Path.GetFileNameWithoutExtension(filePath),
                    SubTitle = filePath,
                    IcoPath = "Images/wireguard.png",
                    Action = e =>
                    {
                        //TODO: as action, connect/disconnect to wireguard connection
                        var command = $"wireguard.exe /installtunnelservice \"{filePath}.dpapi\"";
                        ProcessStartInfo info = new()
                        {
                            FileName = "cmd.exe",
                            Verb = "runas",
                            //Arguments = $"/c {command}",
                            UseShellExecute = true,
                            WindowStyle = ProcessWindowStyle.Hidden
                        };

                        Process.Start(info);

                        return true;
                    }
                });
            }

            // var result = new Result
            // {
            //     Title = "WIREGUARD",
            //     SubTitle = $"tunnelPaths: {tunnelPaths}",
            //     IcoPath = "Images/wireguard.png"
            //     Action = c =>
            //     {
            //         //todo: as action, connect/disconnect to wireguard connection
            //         Context.API.ShowMsg(Context.API.GetTranslation("plugin_helloworldcsharp_greet_title"),
            //                                 Context.API.GetTranslation("plugin_helloworldcsharp_greet_subtitle"));
            //         return true;
            //     },
            // };
            // return new List<Result> { result };

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
    }
}
