using Avalonia.Controls;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YANSMBWE.U8;

namespace YANSMBWE
{
    public static class FileHandler
    {
        public static FileType GetFileType(string path)
        {
            string fileName = Path.GetFileName(path);
            if (fileName.ToUpper().EndsWith(".LZ"))
                if (fileName.Length > 3)
                    fileName = fileName[..^3];
                else
                    return FileType.Unknown;

            if (fileName.ToLower().EndsWith(".arc"))
                return FileType.Arc;

            return FileType.Unknown;
        }

        public static Window OpenFileInNewWindow(string filePath)
        {
            FileType fileType = GetFileType(filePath);

            return fileType switch
            {
                FileType.Arc => new ArcExplorer(filePath),
                FileType.Unknown => throw new ArgumentException("Unknown file type"),
                _ => throw new UnreachableException("Filetype has to be something"),
            };
        }
    }
}
