using Microsoft.Win32;
using System;
using System.Windows;
using System.Windows.Controls;
using System.IO;

namespace Flow.Launcher.Plugin.WireGuard
{
	public partial class WireGuardSettings : UserControl
	{
		private Settings settings;

		public WireGuardSettings(Settings settings) {
			InitializeComponent();
			this.settings = settings;
		}

		private void WireGuardSettings_Loaded(object sender, RoutedEventArgs e) {
			WireGuardConfigPath.Text = settings.WireGuardConfigPath;
		}

		private void btnOpenPath_Click(object sender, RoutedEventArgs e) {
			FolderBrowserDialog fdb = new FolderBrowserDialog();

			var result = fdb.ShowDialog();
			if(result == true)
			{
				WireGuardConfigPath.Text = fdb.SelectedPath;
				settings.WireGuardConfigPath = fdb.SelectedPath;
				settings.Save();
			}
		}
	}
}