using Avalonia.Controls;
using System;
using System.Diagnostics;
using System.IO;

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

        public static FileEditorTab CreateNewFileTab(string filePath)
        {
            FileType fileType = GetFileType(filePath);

            return fileType switch
            {
                FileType.Arc => new ArcEditorTab(filePath),
                FileType.Unknown => throw new ArgumentException("Unknown file type"),
                _ => throw new UnreachableException("Filetype has to be something"),
            };
        }
    }
}
