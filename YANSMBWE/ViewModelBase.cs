using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace YANSMBWE
{
    public abstract class ViewModelBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        public virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));


        // TODO: This is a bad workaround, refactor?
        public virtual bool RaiseAndSetIfChanged<T>(ref T? content, T? value, [CallerMemberName] string? propertyName = null)
            => RaiseAndSetIfChanged(ref content, value, new string?[] { propertyName });

        public virtual bool RaiseAndSetIfChanged<T>(ref T? content, T? value, params string?[] propertyName)
        {
            if (ReferenceEquals(content, value))
                return false;

            content = value;
            foreach (string? name in propertyName)
            {
                OnPropertyChanged(name);
            }
            return true;
        }
    }
}
