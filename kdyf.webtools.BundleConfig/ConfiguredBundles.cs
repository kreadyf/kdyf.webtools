using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Optimization;
using kdyf.webtools.BundleConfig.Settings;
using Bundle = System.Web.Optimization.Bundle;

namespace BundleConfig
{
    public class ConfiguredBundles
    {
        public static IEnumerable<Bundle> All
        {
            get
            {
                return kdyf.webtools.BundleConfig.Settings.BundleConfiguration.Settings.Bundles.Cast<kdyf.webtools.BundleConfig.Settings.Bundle>().Select(bundle =>
                {
                    var res = new Bundle(bundle.VirtualPath);

                    res.Include(bundle.Files.Cast<Include>().Select(file => file.VirtualPath).ToArray());
                    foreach (IncludeDirectory dir in bundle.Directories)
                        res.IncludeDirectory(dir.VirtualPath, string.IsNullOrEmpty(dir.Pattern) ? "" : dir.Pattern, dir.IncludeSubdirectories);

                    foreach (Transformer tf in bundle.Transformers)
                    {
                        string[] typeInfo = tf.Value.Split(',').Select(s => s.Trim()).ToArray();

                        try
                        {
                            res.Transforms.Add(
                                Activator.CreateInstance(typeInfo[1], typeInfo[0]).Unwrap() as IBundleTransform);
                        }
                        catch (Exception ex)
                        {
                            throw new Exception($"Failed to load {tf.Value}.", ex);
                        }
                    }
                    return res;
                });

            }
        }

    }
}
