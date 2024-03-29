﻿using System.Configuration;

namespace kdyf.webtools.BundleConfig.Settings
{
    public class BundleConfiguration : ConfigurationSection
    {
        private static BundleConfiguration settings;

        public static BundleConfiguration Settings
        {
            get
            {
                if (settings != null) return settings;

                System.Configuration.Configuration config =
                    System.Web.Configuration.WebConfigurationManager.OpenWebConfiguration("~");
                //: ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

                settings = config.GetSection("BundleConfig") as BundleConfiguration;

                return settings;
            }
        }

        [ConfigurationProperty("Bundles")]
        public BundleCollection Bundles
        {
            get { return (BundleCollection)this["Bundles"]; }
            set { this["Bundles"] = value; }
        }
    }

    [ConfigurationCollection(typeof(Bundle))]
    public class BundleCollection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new Bundle();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((Bundle)element).VirtualPath;
        }
    }

}