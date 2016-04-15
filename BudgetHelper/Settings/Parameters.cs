using System.Configuration;
using System.Globalization;

namespace BudgetHelper.Settings
{
	sealed class Parameters
	{
		private static Configuration configuration = null;
		private static AvailableLocalizationsConfig availableLocalizations = null;
		private static AppSettingsSection appSettings = null;

		public static CultureInfo CurrentCulture
		{
			get
			{
				return WPFLocalization.LocalizationManager.UICulture;
			}
		}

		private static Configuration Configuration
		{
			get
			{
				if (configuration == null)
				{
					configuration = ConfigurationManager.OpenExeConfiguration(System.Windows.Forms.Application.ExecutablePath);
				}
				return configuration;
			}
		}

		public static AppSettingsSection AppSettings
		{
			get
			{
				if (appSettings == null)
				{
					appSettings = Configuration.AppSettings;
				}
				return appSettings;
			}
		}

		public static AvailableLocalizationsConfig AvailableLocalizations
		{
			get
			{
				if (availableLocalizations == null)
				{
					availableLocalizations = AvailableLocalizationsConfig.GetConfig();
				}
				return availableLocalizations;
			}
		}

		public static string GetAppSettingsValue(string key)
		{
			string value = string.Empty;

			if (!string.IsNullOrWhiteSpace(key))
			{
				var setting = AppSettings.Settings[key];
				if (setting != null)
					value = setting.Value;
			}

			return value;
		}

		/// <summary>
		/// Sets current thread ui culture value to culture saved in config
		/// </summary>
		public static void ReloadLocalization()
		{
			string cultName = GetAppSettingsValue(Constants.ConfigKeys.CURRENT_CULTURE);
			SetLocalization(cultName, false);
		}

		public static void SetLocalization(string name, bool save)
		{
			if (string.IsNullOrWhiteSpace(name) || name == System.Threading.Thread.CurrentThread.CurrentUICulture.Name)
			{
				return;
			}
 
			SetLocalization(AvailableLocalizations.GetLocalization(name), save);
		}

		public static void SetLocalization(CultureInfo culture, bool save)
		{
			if (culture != null)
			{
				WPFLocalization.LocalizationManager.UICulture = culture;
				if (save)
				{
					SaveSetting(Constants.ConfigKeys.CURRENT_CULTURE, culture.Name);
				}
			}
		}

		public static void SaveSetting(string key, string value)
		{
			AppSettings.Settings.Remove(key);
			AppSettings.Settings.Add(key, value);
			SaveConfiguration(true);
		}

		private static void SaveConfiguration(bool refresh)
		{
			Configuration.Save(ConfigurationSaveMode.Full, true);
			if (refresh)
			{
				ConfigurationManager.RefreshSection("appSettings");
			}
		}
	}
}
