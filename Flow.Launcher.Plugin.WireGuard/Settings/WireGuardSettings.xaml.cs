using Microsoft.Win32;
using System;
using System.Windows;
using System.Windows.Controls;
using System.IO;
using Forms = System.Windows.Forms;

namespace Flow.Launcher.Plugin.WireGuard
{
	public partial class WireGuardSettings : UserControl
	{
		private Settings settings;

		public WireGuardSettings(Settings settings)
		{
			InitializeComponent();
			this.settings = settings;
		}

		private void UserControl_Loaded(object sender, RoutedEventArgs e)
		{
			WireGuardConfigPath.Text = settings.WireGuardConfigPath;
		}

		private void btnOpenPath_Click(object sender, RoutedEventArgs e)
		{
			Forms.FolderBrowserDialog fdb = new Forms.FolderBrowserDialog();

			var result = fdb.ShowDialog();
			if (result == Forms.DialogResult.OK)
			{
				WireGuardConfigPath.Text = fdb.SelectedPath;
				settings.WireGuardConfigPath = fdb.SelectedPath;
				settings.Save();
			}
		}
	}
}