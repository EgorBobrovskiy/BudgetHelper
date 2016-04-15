using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using MahApps.Metro;

namespace BudgetHelper
{
	/// <summary>
	/// Interaction logic for App.xaml
	/// </summary>
	public partial class App : Application
	{
		protected override void OnStartup(StartupEventArgs e)
		{
			LoadTheme();
			base.OnStartup(e);
		}

		protected void LoadTheme()
		{
			string accentName = Settings.Parameters.GetAppSettingsValue(Settings.Constants.ConfigKeys.APP_ACCENT);
			if (string.IsNullOrWhiteSpace(accentName))
				accentName = "Lime";
			string themeName = Settings.Parameters.GetAppSettingsValue(Settings.Constants.ConfigKeys.APP_THEME);
			if (string.IsNullOrWhiteSpace(themeName))
				themeName = "BaseLight";
			ThemeManager.ChangeAppStyle(Current,
									ThemeManager.GetAccent(accentName),
									ThemeManager.GetAppTheme(themeName));
		}
	}
}
