using BundleConfig.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Optimization;
using Bundle = System.Web.Optimization.Bundle;

using settings = BundleConfig.Settings;

namespace BundleConfig
{
    public class ConfiguredBundles
    {
        public static IEnumerable<Bundle> All
        {
            get
            {
                return BundleConfig.Settings.BundleConfig.Settings.Bundles.Cast<settings.Bundle>().Select(bundle =>
                {
                    var res = new Bundle(bundle.VirtualPath);

                    res.Include(bundle.Files.Cast<settings.Include>().Select(file => file.VirtualPath).ToArray());
                    foreach (IncludeDirectory dir in bundle.Directories)
                        res.IncludeDirectory(dir.VirtualPath, string.IsNullOrEmpty(dir.Pattern) ? "" : dir.Pattern, dir.IncludeSubdirectories);

                    foreach (settings.Transformer tf in bundle.Transformers)
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
