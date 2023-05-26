using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YANSMBWE
{
    public class MessageBoxButton
    {
        public string ButtonText { get; set; }
        public double Width { get => ButtonText.Length * 10 + 10; }
        public MessageBoxButton(string ButtonText)
        {
            this.ButtonText = ButtonText;
        }
    }
}
