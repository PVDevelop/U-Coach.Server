using System;
using System.Configuration;

namespace PVDevelop.UCoach.Server.Configuration
{
    /// <summary>
    /// Загружает настройки из app.config
    /// </summary>
    public class ConfigurationSectionSettingsProvider<T> : ISettingsProvider<T>
    {
        public T Settings { get; private set; }

        public ConfigurationSectionSettingsProvider(string sectionName)
        {
            if(sectionName == null)
            {
                throw new ArgumentNullException("sectionName");
            }
            Settings = (T)ConfigurationManager.GetSection(sectionName);
        }
    }
}
