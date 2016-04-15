using MahApps.Metro;
using MahApps.Metro.Controls;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace BudgetHelper
{
	public partial class MainWindow : MetroWindow
	{
		public ObservableCollection<CultureInfo> AvailableCultures
		{
			get
			{
				return new ObservableCollection<CultureInfo>(Settings.Parameters.AvailableLocalizations.Languages.ToList().Select(l => l.Culture));
			}
		}

		public MainWindow()
		{
			Settings.Parameters.ReloadLocalization();
			InitializeComponent();
			Settings.AccentTile.GetAccentTiles().ForEach(a => { ThemeTilesWrapPanel.Children.Add(a); a.Click += AccentTile_Click; });
			SetDefaults();
		}

		private void SetDefaults()
		{
			LanguageSelector.SelectedItem = Settings.Parameters.CurrentCulture;
			ThemeSwitch.IsChecked = Settings.Parameters.GetAppSettingsValue(Settings.Constants.ConfigKeys.APP_THEME) == "BaseDark";
		}

		private void SettingsButtonClick(object sender, RoutedEventArgs e)
		{
			this.SettingsFlyout.IsOpen = !this.SettingsFlyout.IsOpen;
		}

		private void LanguageSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			if (e.AddedItems.Count > 0)
			{
				Settings.Parameters.SetLocalization(e.AddedItems[0] as CultureInfo, true);
			}
		}

		private void LanguageSelector_Click(object sender, RoutedEventArgs e)
		{
			LanguageSelector.IsExpanded = !LanguageSelector.IsExpanded;
		}

		private void AccentTile_Click(object sender, RoutedEventArgs e)
		{
			var accentTile = sender as Settings.AccentTile;
			if (accentTile != null)
			{
				ThemeManager.ChangeAppStyle(Application.Current, accentTile.Accent, ThemeManager.DetectAppStyle(this).Item1);
				Settings.Parameters.SaveSetting(Settings.Constants.ConfigKeys.APP_ACCENT, accentTile.Accent.Name);
			}
		}

		private void UseDarkThemeSwitch_IsCheckedChanged(object sender, System.EventArgs e)
		{
			var ts = sender as ToggleSwitch;
			if (ts != null && ts.IsChecked.HasValue)
			{
				AppTheme theme = ThemeManager.GetAppTheme(ts.IsChecked.Value ? "BaseDark" : "BaseLight");
				ThemeManager.ChangeAppStyle(Application.Current, 
					ThemeManager.DetectAppStyle(this).Item2, theme);
				Settings.Parameters.SaveSetting(Settings.Constants.ConfigKeys.APP_THEME, theme.Name);
			}
		}
	}
}
