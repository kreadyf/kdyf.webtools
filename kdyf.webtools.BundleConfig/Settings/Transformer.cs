using System.Configuration;

namespace kdyf.webtools.BundleConfig.Settings
{
    internal class Transformer : ConfigurationElement
    {
        [ConfigurationProperty("value", IsRequired = true)]
        public string Value
        {
            get { return (string)this["value"]; }
            set { this["value"] = value; }
        }

    }
}