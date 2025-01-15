using System.Xml;

namespace IdsSampleClient.Helpers;
internal class TreeNodeHelper
{
    /// <summary>
    /// Renders a node of XML into a TreeNode. Recursive if inside the node there are more child nodes.
    /// </summary>
    /// <param name="xmlNode">XML node.</param>
    /// <param name="treeNode">Tree node.</param>
    internal static void AddNode(XmlNode xmlNode, TreeNode treeNode)
    {
        // Loop through the XML nodes until the leaf is reached.
        // Add the nodes to the TreeView during the looping process.
        // If the node has child nodes, the function will call itself.
        if (xmlNode.HasChildNodes)
        {
            var nodeList = xmlNode.ChildNodes;

            for (int x = 0; x <= nodeList.Count - 1; x++)
            {
                var xNode = xmlNode.ChildNodes[x];
                treeNode.Nodes.Add(new TreeNode(xNode?.Name));
                var tNode = treeNode.Nodes[x];
                if (xNode != null)
                {
                    AddNode(xNode, tNode);
                }
            }
        }
        else
        {
            // Here you need to pull the data from the XmlNode based on the
            // type of node, whether attribute values are required, and so forth.
            treeNode.Text = (xmlNode.OuterXml).Trim();
        }
    }
}
