using Avalonia.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YANSMBWE
{
    public abstract class FileEditorTab : UserControl
    {
        public abstract string TabName { get; }
        public abstract bool IsActive { get; set; }

        public abstract void Close();
    }
}
