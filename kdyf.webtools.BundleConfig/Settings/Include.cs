using System.Configuration;

namespace kdyf.webtools.BundleConfig.Settings
{
    internal class Include : ConfigurationElement
    {
        [ConfigurationProperty("virtualPath", IsRequired = true)]
        public string VirtualPath
        {
            get { return (string)this["virtualPath"]; }
            set { this["virtualPath"] = value; }
        }
    }
}