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

		/// <summary>
		/// Initializes a new instance of the <see cref="WireGuardSettings"/> class.
		/// </summary>
		/// <param name="settings">The settings object.</param>
		public WireGuardSettings(Settings settings)
		{
			InitializeComponent();
			this.settings = settings;
		}

		/// <summary>
		/// Handles the Loaded event of the UserControl.
		/// </summary>
		/// <param name="sender">The event sender.</param>
		/// <param name="e">The event arguments.</param>
		private void UserControl_Loaded(object sender, RoutedEventArgs e)
		{
			WireGuardConfigPath.Text = settings.WireGuardConfigPath;
		}

		/// <summary>
		/// Handles the Click event of the btnOpenPath button.
		/// </summary>
		/// <param name="sender">The event sender.</param>
		/// <param name="e">The event arguments.</param>
		private void btnOpenPath_Click(object sender, RoutedEventArgs e)
		{
			Forms.FolderBrowserDialog fdb = new Forms.FolderBrowserDialog();

			var result = fdb.ShowDialog();
			if (result == Forms.DialogResult.OK)
			{
				WireGuardConfigPath.Text = fdb.SelectedPath;
				settings.WireGuardConfigPath = fdb.SelectedPath;
				settings.Save();
				settings.OnSettingsChanged?.Invoke(settings);
			}
		}
	}
}