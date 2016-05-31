using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BloomFilterCore
{
	public static class Settings
	{
		public static bool Output_Compress = SettingsReader.GetSetting<bool>("Output.Compress");
		public static string Output_Filename = SettingsReader.GetSetting<string>("Output.Filename");
		
		public static class SettingsReader
		{
			public static T GetSetting<T>(string SettingName)
			{
				try
				{
					if (!SettingExists(SettingName))
					{
						return default(T);
					}
					else
					{
						return (T)Convert.ChangeType(ConfigurationManager.AppSettings[SettingName], typeof(T));
					}
				}
				catch
				{
					return default(T);
				}
			}

			private static bool SettingExists(string SettingName, bool CheckForEmptyValue = true)
			{
				try
				{
					if (string.IsNullOrWhiteSpace(SettingName))
					{
						return false;
					}

					if (!ConfigurationManager.AppSettings.AllKeys.Contains(SettingName))
					{
						return false;
					}

					if (CheckForEmptyValue)
					{
						if (string.IsNullOrWhiteSpace(ConfigurationManager.AppSettings[SettingName]))
						{
							return false;
						}
					}

					return true;
				}
				catch
				{
					return false;
				}
			}
		}
	}
}
