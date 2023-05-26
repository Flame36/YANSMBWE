using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using YANSMBWE.U8;
using YANSMBWE.Utils;

namespace YANSMBWE
{
    public class ArcExplorerViewModel : ViewModelBase
    {
        public ObservableCollection<U8Node> Nodes { get; set; } = new();
        
        U8Archive? archive = null;
        public U8Archive? Archive { get => archive; set => RaiseAndSetIfChanged(ref archive, value, nameof(Archive), nameof(HasFile)); }
        public bool HasFile { get => archive is not null; }
        
        U8Node? selected = null;
        public U8Node? Selected { get => selected; set => RaiseAndSetIfChanged(ref selected, value, nameof(Selected), nameof(HasSelected), nameof(SelectedPathTask)); }
        public bool HasSelected { get => selected is not null; }
        public Task<string> SelectedPathTask { get => GetSelectedPathAsync(); }


        public ArcExplorerViewModel(string? filePath = null)
        {
            if (string.IsNullOrEmpty(filePath))
                return;
            Archive = U8Archive.FromBytes(File.ReadAllBytes(filePath));
            Nodes.Add(Archive.Root);
        }

        /*
         * Could be optimized? Yes
         * Does it matter with at most ~20 items? Nope
         * Oh also
         * TODO: Does this have a race condition?
         */
        async Task<string> GetSelectedPathAsync()
        {
            if (!HasSelected)
                return "";
            lock (Nodes)
            {
                static bool TreeSearch(U8Node root, U8Node target, ref List<string> path)
                {
                    if (root == target)
                    {
                        path.Add(root.Name);
                        return true;
                    }

                    foreach (U8Node subNode in root.SubNodes)
                    {
                        if (TreeSearch(subNode, target, ref path))
                        {
                            path.Add(root.Name);
                            return true;
                        }
                    }

                    return false;
                }

                List<string> path = new();
                TreeSearch(Nodes[0], Selected, ref path);
                path.Reverse();
                return "/" + Path.Join(path.ToArray()).Replace('\\', '/');
            }
        }
    }
}
