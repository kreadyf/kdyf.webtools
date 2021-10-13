using ImageProcessor;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Umbraco.Core;
using Umbraco.Core.Composing;
using Umbraco.Core.Configuration.UmbracoSettings;
using Umbraco.Core.IO;
using Umbraco.Core.Models;
using Umbraco.Core.PropertyEditors.ValueConverters;
using Umbraco.Core.Services;
using Umbraco.Core.Services.Implement;

namespace kdyf.webtools.Umbraco8.ImageProcessing
{
    [RuntimeLevel(MinLevel = RuntimeLevel.Run)]
    public class ImagePreprocessingComposer : ComponentComposer<ImagePreprocessingComponent>
    {
    }

    public class ImagePreprocessingComponent : IComponent
    {
        private readonly IMediaFileSystem _mediaFileSystem;
        private readonly IContentSection _contentSection;
        private readonly IMediaService _mediaService;
        private readonly IMediaTypeService _mediaTypeService;
        private readonly IContentTypeBaseServiceProvider _contentTypeBaseService;

        public ImagePreprocessingComponent(IMediaFileSystem mediaFileSystem, IContentSection contentSection,
            IMediaService mediaService, IMediaTypeService mediaTypeService, IContentTypeBaseServiceProvider contentTypeBaseService)
        {
            _mediaFileSystem = mediaFileSystem;
            _contentSection = contentSection;
            _mediaService = mediaService;
            _mediaTypeService = mediaTypeService;
            _contentTypeBaseService = contentTypeBaseService;
        }

        public void Initialize()
        {
            MediaService.Saving += MediaService_Saving;
        }

        private void MediaService_Saving(IMediaService sender, Umbraco.Core.Events.SaveEventArgs<Umbraco.Core.Models.IMedia> e)
        {
            IEnumerable<string> supportedTypes = _contentSection.ImageFileTypes.ToList();
            foreach (IMedia media in e.SavedEntities)
            {
                if (media.HasProperty("umbracoFile"))
                {
                    string info = media.GetValue<string>(Constants.Conventions.Media.File);
                    string path = JsonConvert.DeserializeObject<ImageCropperValue>(info).Src;
                    string extension = Path.GetExtension(path).Substring(1);
                    if (supportedTypes.InvariantContains(extension))
                    {
                        string fullPath = _mediaFileSystem.GetFullPath(path);

                        var inStream = sender.GetMediaFileContentStream(fullPath);

                        using (var outStream = new MemoryStream())
                        {
                            using (ImageFactory imageFactory = new ImageFactory(preserveExifData: false))
                            {
                                var image = imageFactory.Load(inStream).AutoRotate().Save(outStream);

                                sender.SetMediaFileContent(fullPath, outStream);
                            }
                        }
                    }
                }
            }
        }

        public void Terminate()
        {

        }

    }
}
