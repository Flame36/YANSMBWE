using Avalonia;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace YANSMBWE
{
    public static class ImageHandler
    {


        //TODO: Cache bitmaps?
        public static IBitmap GetImageFromAssembly(string path)
        {
            Uri uri;

            if (path.StartsWith("avares://"))
            {
                uri = new Uri(path);
            }
            else
            {
                Assembly assembly = Assembly.GetEntryAssembly() ?? throw new NullReferenceException("Assembly is null");
                string assemblyName = assembly.GetName().Name ?? throw new NullReferenceException("Assembly name is null");
                uri = new Uri($"avares://{assemblyName}/{path}");
            }

            IAssetLoader assets = AvaloniaLocator.Current.GetService<IAssetLoader>() ?? throw new NullReferenceException("No AssetLoader service");
            var asset = assets.Open(uri);

            return new Bitmap(asset);
        }
    }
}
