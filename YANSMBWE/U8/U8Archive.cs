using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using YANSMBWE.Utils;

namespace YANSMBWE.U8
{
    public class U8Archive
    {
        public const int HEADER_LENGTH = 32;
        public const int NODE_LENGTH = 12;

        public U8Header Header { get; set; }
        public List<U8Node> Nodes { get; set; }
        public U8Node Root { get; set; }

        public U8Archive(U8Header Header, List<U8Node> Nodes, U8Node Root)
        {
            this.Header = Header;
            this.Nodes = Nodes;
            this.Root = Root;
        }
        public U8Archive(U8Header Header, List<U8Node> Nodes) : this(Header, Nodes, Nodes.First()) { }


        static List<string> ReadStringTable(byte[] data, U8Header header, List<U8RawNode> nodes)
        {
            int index = (int)(header.RootNodeOffset + nodes.Count * NODE_LENGTH);
            List<string> stringTable = new();
            for (int i = 0; i < nodes.Count; i++)
            {
                string name = string.Empty;
                do
                {
                    name += (char)data[index];
                    index++;
                } while (!name.EndsWith('\0'));
                stringTable.Add(name[..^1]);
            }

            return stringTable;
        }
        static List<byte[]> ReadDataTable(byte[] data, List<U8RawNode> nodes)
        {
            List<byte[]> dataTable = new List<byte[]>();
            foreach (U8RawNode node in nodes)
            {
                if (node.Type != 0)
                {
                    dataTable.Add(Array.Empty<byte>());
                    continue;
                }

                dataTable.Add(data.SubArray(node.DataOffset, node.Size));
            }

            return dataTable;
        }

        public static U8Node BuildNodeTree(List<U8Node> nodes)
        {
            int nodeCount = nodes.Count;
            Stack<U8Node> stack = new();

            stack.Push(nodes[0]);

            for (int i = 1; i < nodeCount; i++)
            {
                U8Node node = nodes[i];
                if (node.IsDir)
                    stack.Push(node);
                else
                    stack.First().SubNodes.Add(node);

                while (i + 1 >= stack.Last().RawNode.Size)
                {
                    if (stack.First().IsRoot)
                        return stack.First();

                    node = stack.Pop();
                    stack.First().SubNodes.Add(node);
                }
            }

            return stack.First();
        }

        public static U8Archive FromBytes(byte[] data, bool checkMagic = true)
        {
            U8Header header = U8Header.FromBytes(data);

            if (checkMagic && !header.Tag.SequenceEqual(U8Header.Magic))
                throw new Exception("Header magic doesn't match"); // TODO: Find a good exception for this

            List<U8RawNode> rawNodes = new();

            U8RawNode rawRoot = U8RawNode.FromBytes(data, header);
            rawNodes.Add(rawRoot);
            while (rawNodes.Count != rawRoot.Size)
                rawNodes.Add(U8RawNode.FromBytes(data, header, rawNodes.Count));

            List<string> stringTable = ReadStringTable(data, header, rawNodes);
            List<byte[]> dataTable = ReadDataTable(data, rawNodes);

            List<U8Node> nodes = new();
            foreach (U8RawNode rawNode in rawNodes)
            {
                nodes.Add(U8Node.FromRawNode(rawNode, stringTable, dataTable));
            }

            nodes.First().Type |= U8NodeType.Root;

            U8Node nodeTreeRoot = BuildNodeTree(nodes);

            return new U8Archive(header, nodes, nodeTreeRoot);
        }

    }
}
