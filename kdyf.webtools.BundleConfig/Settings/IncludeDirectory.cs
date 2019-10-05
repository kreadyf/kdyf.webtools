using System.Configuration;

namespace kdyf.webtools.BundleConfig.Settings
{
    internal class IncludeDirectory : Include
    {
        [ConfigurationProperty("pattern", IsRequired = false)]
        public string Pattern
        {
            get { return (string)this["pattern"]; }
            set { this["pattern"] = value; }
        }

        [ConfigurationProperty("includeSubdirectories", IsRequired = false)]
        public bool IncludeSubdirectories
        {
            get { return (bool)this["includeSubdirectories"]; }
            set { this["includeSubdirectories"] = value; }
        }
    }
}