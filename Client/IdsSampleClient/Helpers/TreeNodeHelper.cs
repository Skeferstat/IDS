using System.Xml;
using System.Xml.Linq;

namespace IdsSampleClient.Helpers;
internal class TreeNodeHelper
{
    /// <summary>
    /// Add a node to a tree node.
    /// </summary>
    /// <param name="xmlNode">Xml node.</param>
    /// <param name="treeNode">Tree node.</param>
    internal static void AddNode(XmlNode xmlNode, TreeNode treeNode)
    {
        if (xmlNode.HasChildNodes)
        {
            var xNodeChildren = xmlNode.ChildNodes;
            for (int x = 0; x <= xNodeChildren.Count - 1; x++)
            {
                XmlNode? xNode = xmlNode.ChildNodes[x];
                string nodeText = xNode!.Name; ;
                if (nodeText.Contains("text") == false)
                {
                    if (xNode.Attributes!.Count > 0)
                    {
                        for (int i = 0; i < xNode.Attributes.Count; i++)
                        {
                            nodeText += " " + xNode.Attributes[i].Name + "=" + "\"" + xNode.Attributes[i].Value + "\"";
                        }
                    }
                }

                treeNode.Nodes.Add(new TreeNode(nodeText));
                var tNode = treeNode.Nodes[x];
                AddNode(xNode, tNode);
            }
        }
        else
        {
            string nodeText = xmlNode.Name;
            if (nodeText.Contains("text") == false)
            {
                if (xmlNode.Attributes!.Count > 0)
                {
                    for (int i = 0; i < xmlNode.Attributes.Count; i++)
                    {
                        nodeText += xmlNode.Attributes[i].Name + "=" + xmlNode.Attributes[i].Value;
                    }
                }
            }
            else
            {
                nodeText = xmlNode.InnerText;
            }
            treeNode.Text = nodeText;
        }
    }


    /// <summary>
    /// Add a context menu to a tree view.
    /// </summary>
    /// <param name="treeView">Tree view.</param>
    internal static void AddContextMenu(TreeView treeView)
    {
        ContextMenuStrip contextMenu = new ContextMenuStrip();
        ToolStripMenuItem editItem = new ToolStripMenuItem("Edit");
        editItem.Click += (s, e) =>
        {
            if (treeView.SelectedNode != null)
            {
                treeView.SelectedNode.BeginEdit();
            }
        };

        contextMenu.Items.AddRange(new[] { editItem });
        treeView.ContextMenuStrip = contextMenu;
    }



    /// <summary>
    /// Convert a tree view to an xml string.
    /// </summary>
    /// <param name="treeView">Tree view.</param>
    /// <returns>Xml string.</returns>
    public static string ConvertToXml(TreeView treeView)
    {
        using StringWriter stringWriter = new StringWriter();
        stringWriter.WriteLine(@"<?xml version=""1.0"" encoding=""UTF-8"" standalone=""yes""?>");
        var xmlns = "xmlns=\"http://www.itek.de/Shop-Anbindung/Warenkorb/\"";
        stringWriter.WriteLine($"<{treeView.Nodes[0].Text} {xmlns}>");

        foreach (TreeNode node in treeView.Nodes)
        {
            ExportNode(node.Nodes, stringWriter, 1);
        }

        stringWriter.WriteLine($"</{treeView.Nodes[0].Text}>");
        return stringWriter.ToString();
    }

    private static void ExportNode(TreeNodeCollection tnc, StringWriter stringWriter, int indentLevel)
    {
        string indent = new string(' ', indentLevel * 2);

        foreach (TreeNode node in tnc)
        {
            if (node.Nodes.Count > 0)
            {
                if (node.Nodes.Count == 1)
                {
                    stringWriter.Write($"{indent}<{node.Text}>");
                    stringWriter.Write($"{node.Nodes[0].Text}");
                    stringWriter.WriteLine($"</{node.Text}>");
                }
                else
                {
                    stringWriter.WriteLine($"{indent}<{node.Text}>");
                    ExportNode(node.Nodes, stringWriter, indentLevel + 1);
                    stringWriter.WriteLine($"{indent}</{node.Text}>");
                }
            }
            else
            {
                stringWriter.WriteLine($"{indent}<{node.Text}></{node.Text}>"); 
            }
        }
    }




}
