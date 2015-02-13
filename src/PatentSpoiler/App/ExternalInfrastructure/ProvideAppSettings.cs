using System.Configuration;

namespace PatentSpoiler.App.ExternalInfrastructure
{
    public interface IProvideAppSettings
    {
        string this[string key] { get; }
    }

    public class ProvideAppSettings : IProvideAppSettings
    {
        public string this[string key]
        {
            get { return ConfigurationManager.AppSettings.Get(key); }
        }
    }
}