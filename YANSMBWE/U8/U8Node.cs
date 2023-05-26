using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.IO;

namespace YANSMBWE.U8
{
    public class U8Node
    {
        public U8RawNode? RawNode { get; init; }
        public U8NodeType Type { get; set; }
        public bool IsRoot { get => (Type & U8NodeType.Root) == U8NodeType.Root; }
        public bool IsDir { get => (Type & U8NodeType.Dir) == U8NodeType.Dir; }
        public bool IsFile { get => !IsDir; }

        public string DisplayName { get => IsRoot ? "/" : Name; }

        public string Name { get; set; }
        public byte[] Data { get; set; }
        
        public ObservableCollection<U8Node> SubNodes { get; set; }

        public U8Node(U8NodeType Type, string Name, byte[] Data, ObservableCollection<U8Node>? SubNodes = null, U8RawNode? RawNode = null)
        {
            this.Type = Type;
            this.Name = Name;
            this.Data = Data;
            if (SubNodes is null)
                this.SubNodes = new();
            else
                this.SubNodes = SubNodes;
            this.RawNode = RawNode;
        }
        public U8Node(U8NodeType Type, string Name, byte[] Data) : this(Type, Name, Data, new()) { }

        public static U8Node FromRawNode(U8RawNode rawNode, List<string> stringTable, List<byte[]> dataTable)
            => new((U8NodeType)rawNode.Type, stringTable[rawNode.NodeIndex], dataTable[rawNode.NodeIndex], RawNode: rawNode);
    }
}
