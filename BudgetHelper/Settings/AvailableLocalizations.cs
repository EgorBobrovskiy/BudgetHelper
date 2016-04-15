using System;
using System.Configuration;
using System.Globalization;
using System.Collections.Generic;

namespace BudgetHelper.Settings
{
	public class Language : ConfigurationElement
	{
		protected CultureInfo _cultureInfo = null;

		[ConfigurationProperty("name", IsRequired = true)]
		public string Name
		{
			get
			{
				return this["name"] as string;
			}
		}

		public CultureInfo Culture
		{
			get
			{
				CultureInfo value = _cultureInfo;
				if (value == null)
				{
					var name = Name;
					try
					{
						if (!string.IsNullOrWhiteSpace(this.Name))
						{
							value = new CultureInfo(name);
						}
					}
					catch (CultureNotFoundException e)
					{
						Console.WriteLine("Culture " + e.InvalidCultureName + "not found. Check config file");
					}
				}
				return value;
			}
		}
	}

	public class Languages : ConfigurationElementCollection
	{
		public List<Language> ToList()
		{
			List<Language> ret = new List<Language>(this.Count);
			foreach (Language lang in this)
			{
				ret.Add(lang);
			}
			return ret;
		}

		public Language this[int index]
		{

			get
			{
				return BaseGet(index) as Language;
			}
			set
			{
				if (BaseGet(index) != null)
				{
					BaseRemoveAt(index);
				}
				this.BaseAdd(index, value);
			}
		}

		public new Language this[string responseString]
		{
			get
			{
				return BaseGet(responseString) as Language;
			}
			set
			{
				var oldLang = BaseGet(responseString);
				if (oldLang != null)
				{
					BaseRemoveAt(BaseIndexOf(oldLang));
				}
				BaseAdd(value);
			}
		}

		protected override ConfigurationElement CreateNewElement()
		{
			return new Language();
		}

		protected override object GetElementKey(ConfigurationElement element)
		{
			return ((Language)element).Name;
		}
	}

	public class AvailableLocalizationsConfig : ConfigurationSection
	{
		public static AvailableLocalizationsConfig GetConfig()
		{
			return (ConfigurationManager.GetSection("AvailableLocalizations") as AvailableLocalizationsConfig) ?? new AvailableLocalizationsConfig();
		}

		public CultureInfo GetLocalization(string name)
		{
			CultureInfo ci = null;

			foreach (Language lang in Languages)
			{
				if (lang.Name == name)
				{
					ci = lang.Culture;
					break;
				}
			}

			return ci;
		}

		[ConfigurationProperty("Languages")]
		[ConfigurationCollection(typeof(Languages), AddItemName = "Language")]
		public Languages Languages
		{
			get
			{
				return this["Languages"] as Languages;
			}
		}
	}
}