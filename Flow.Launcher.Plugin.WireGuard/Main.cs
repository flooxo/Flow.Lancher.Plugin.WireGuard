﻿using Flow.Launcher.Plugin;
using System.Collections.Generic;
using System.IO;
using System.Diagnostics;
using System;
using System.Collections;

namespace Flow.Launcher.Plugin.WireGuard
{
    public class WireGuardPlugin : IPlugin, IPluginI18n
    {
        internal PluginInitContext Context;

        private const string Image = @"Images\wireguard.png";

        private string configPath = @"C:\Program Files\WireGuard\Data\Configurations\";
        private string[] tunnelPaths = Array.Empty<string>();

        public List<Result> Query(Query query)
        {
            var resultList = new List<Result>(tunnelPaths.Length);

            foreach (var tunnelPath in tunnelPaths)
            {
                resultList.Add(new Result
                {
                    Title = GetFileNameWithoutExtensions(tunnelPath),
                    SubTitle = tunnelPath,
                    IcoPath = Image,
                    Action = e =>
                    {
                        //TODO: wireguard has to run?
                        getProcess(tunnelPath, Command.install);
                        return true;
                    }
                });
            }

            return resultList;
        }

        public void Init(PluginInitContext context)
        {
            Context = context;
            if (!Directory.Exists(configPath))
            {
                //Log.Exception($"Plugin Wireguard: {configPath} not found."); //TODO: log
            }
            else
            {
                tunnelPaths = Directory.GetFiles(configPath);
                tunnelPaths = Array.FindAll(tunnelPaths, tunnelPath =>
                    tunnelPath.EndsWith(".conf", StringComparison.OrdinalIgnoreCase) ||
                    tunnelPath.EndsWith(".conf.dpapi", StringComparison.OrdinalIgnoreCase));
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

        private void getProcess(string tunnelPath, Command commandType){
            string command = "";
            if (commandType == Command.install){
                command = $"wireguard.exe /installtunnelservice \"{tunnelPath}\"";
            } else if (commandType == Command.uninstall){
                var tunnelName = GetFileNameWithoutExtensions(tunnelPath);
                command = $"wireguard.exe /uninstall \"{tunnelName}\"";
            }
            ProcessStartInfo info = new()
            {
                FileName = "cmd.exe",
                Verb = "runas",
                Arguments = $"/c {command}",
                UseShellExecute = true,
                WindowStyle = ProcessWindowStyle.Hidden
            };

            try{
                Process.Start(info);
            } catch (Exception e) {
                //Log.Error($"Failed to install tunnel service for {tunnelPath}");
                //Log.Exception(e);
            }
        }
    }

    enum Command
{
    install,    
    uninstall
}
}
