

using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using XmlDataPopulate.Model;


namespace XmlDataPopulate.Context
{
    public static class SeedData
    {
        public static void Initialize(NodeDbContext context)
        {
            if (context.Nodes.Any()) return;

            var xmlData = XDocument.Load("C:\\Users\\subha\\Downloads\\EquipmentDescriptionFormat 1.xml");
            var rootNode = ParseXmlNode(xmlData.Root, null);
            context.Nodes.Add(rootNode);
            context.SaveChanges();
        }

        private static Node ParseXmlNode(XElement element, Node parent)
        {
            var node = new Node
            {
                Name = element.Attribute("Name").Value,
                NodeType = element.Attribute("NodeType").Value,
                Parent = parent
            };

            foreach (var childElement in element.Elements())
            {
                var childNode = ParseXmlNode(childElement, node);
                node.Children.Add(childNode);
            }

            return node;
        }
    }
}
