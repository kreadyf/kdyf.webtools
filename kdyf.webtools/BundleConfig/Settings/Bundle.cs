using System.Configuration;

namespace BundleConfig.Settings
{
    internal class Bundle : ConfigurationElement
    {
        [ConfigurationProperty("virtualPath", IsRequired = true)]
        public string VirtualPath
        {
            get { return (string)this["virtualPath"]; }
            set { this["virtualPath"] = value; }
        }

        //[ConfigurationProperty("isCss", IsRequired = true)]
        //public bool IsCss
        //{
        //    get { return (bool)this["isCss"]; }
        //    set { this["isCss"] = value; }
        //}
        [ConfigurationProperty("Transformers")]
        public TransformerCollection Transformers
        {
            get { return (TransformerCollection)this["Transformers"]; }
            set { this["Transformers"] = value; }
        }

        [ConfigurationProperty("Files")]
        public IncludeCollection Files
        {
            get { return (IncludeCollection)this["Files"]; }
            set { this["Files"] = value; }
        }

        [ConfigurationProperty("Directories")]
        public IncludeDirectoryCollection Directories
        {
            get { return (IncludeDirectoryCollection)this["Directories"]; }
            set { this["Directories"] = value; }
        }
    }

    [ConfigurationCollection(typeof(Transformer))]
    public class TransformerCollection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new Transformer();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((Transformer)element).Value;
        }
    }

    [ConfigurationCollection(typeof(Include))]
    public class IncludeCollection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new Include();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((Include)element).VirtualPath;
        }
    }

    [ConfigurationCollection(typeof(IncludeDirectory))]
    public class IncludeDirectoryCollection : IncludeCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new IncludeDirectory();
        }
    }
}