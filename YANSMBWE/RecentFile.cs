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
    public class RecentFile
    {
        public string Name { get; set; } = "FileName";
        public string Path { get; set; } = "C:/Users/MyUser/Documents/YetAnotherNewSuperMarioBrosWiiEditor/MyAwesomeProject.prj";
        public DateTime LastAccessed { get; set; } = new DateTime(2020, 1, 10);
        public IBitmap Icon { get => ImageHandler.GetImageFromAssembly(IconPath); }
        public string IconPath { get; set; } = "assets/images/arc_icon.png";


        public RecentFile() { }
        public RecentFile(string Name, string Path, DateTime LastAccessed) 
        {
            this.Name = Name;
            this.Path = Path;
            this.LastAccessed = LastAccessed;
        }
    }
}
