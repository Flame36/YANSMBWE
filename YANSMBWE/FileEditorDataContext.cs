using DynamicData.Binding;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Formats.Tar;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YANSMBWE.U8;

namespace YANSMBWE
{
    public class FileEditorDataContext : ViewModelBase
    {
        public ObservableCollection<FileEditorTab> Tabs { get; set; } = new();

        FileEditorTab? selectedTab;
        public FileEditorTab? SelectedTab { get => selectedTab; set => ChangeSelectedTab(value); }
        public bool HasSelectedTab { get => SelectedTab is not null; }

        public FileEditorDataContext()
        {
            Tabs.CollectionChanged += TabsChanged;
        }

        private void TabsChanged(object? sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (!HasSelectedTab || Tabs.Contains(SelectedTab))
                return;
            SelectedTab = Tabs.Any() ? Tabs.First() : null;//TODO: FirstOrDefault?
        }

        //TODO: Fix race condition
        void ChangeSelectedTab(FileEditorTab? newTab)
        {
            if (selectedTab is not null)
                selectedTab.IsActive = false;
            if (newTab is not null)
                newTab.IsActive = true;
            RaiseAndSetIfChanged(ref selectedTab, newTab, nameof(SelectedTab), nameof(HasSelectedTab));
        }

        public void CloseTab(FileEditorTab tab)
        {
            tab.Close();
            Tabs.Remove(tab);
        }
    }
}
