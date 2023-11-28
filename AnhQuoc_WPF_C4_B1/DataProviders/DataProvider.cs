using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace AnhQuoc_WPF_C4_B1
{
    public class DataProvider
    {
        #region Singleton Pattern
        private static readonly object Instancelock = new object();
        private static DataProvider _Instance;
        public static DataProvider Instance
        {
            get
            {
                lock (Instancelock)
                {
                    if (_Instance == null)
                        _Instance = new DataProvider();
                }
                return _Instance;
            }
        }
        #endregion

        public string pathData { get; set; }
        static XmlDocument doc = null;
        public XmlNode nodeRoot = null;

        public void Open()
        {
            if (doc == null)
                doc = new XmlDocument();
            doc.Load(pathData);
            nodeRoot = doc.DocumentElement;
        }

        public void Open(string path)
        {
            pathData = path;
            if (doc == null)
                doc = new XmlDocument();
            doc.Load(pathData);
            nodeRoot = doc.DocumentElement;
        }

        public void Close()
        {
            if (doc != null)
                doc.Save(pathData);
        }

        public XmlNode getNode(string xpath)
        {
            return nodeRoot.SelectSingleNode(xpath);
        }

        public XmlNode getNode(string xpath, int index)
        {
            XmlNode temp = doc.SelectSingleNode(xpath);
            for (int i = 0; i < index; i++)
            {
                temp = temp.NextSibling;
            }
            return temp;
        }

        public XmlNodeList getDsNode(string xpath)
        {
            return nodeRoot.SelectNodes(xpath);
        }

        public XmlNode createNode(string tagName)
        {
            XmlNode node = doc.CreateElement(tagName);
            return node;
        }

        public XmlAttribute createAttr(string name)
        {
            XmlAttribute attr = doc.CreateAttribute(name);
            return attr;
        }

        // Thêm 1 nút con tại vị trí cuối trong nút gốc (nút cha)
        public void AppendNode(XmlNode node, XmlNode newNode)
        {
            node.AppendChild(newNode);
        }

        public void PrependNode(XmlNode node, XmlNode newNode)
        {
            node.PrependChild(newNode);
        }

        public void InsertNode(XmlNode childNode, XmlNode refNode)
        {
            nodeRoot.InsertAfter(childNode, refNode);
        }

        public void RemoveNode(XmlNode refNode)
        {
            XmlNode parent = refNode.ParentNode;
            parent.RemoveChild(refNode);
        }

        public XmlNode FindNodeByNodeName(XmlNode parentNode, string nodeName)
        {
            foreach (XmlNode node in parentNode.ChildNodes)
            {
                if (node.Name == nodeName)
                {
                    return node;
                }
            }
            return null;
        }
    }
}
