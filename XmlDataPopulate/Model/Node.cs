using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;

namespace XmlDataPopulate.Model
{
    [XmlRoot("Node")]
    public class Node
    {
        [Key]
        public Guid Id { get; set; }
        [XmlAttribute("Name")]
        public string Name { get; set; }
        [XmlAttribute("NodeType")]
        public string NodeType { get; set; }
        public Guid? ParentId { get; set; }
        public Node Parent { get; set; }
        [XmlElement("Node")]
        public ICollection<Node> Children { get; set; } = new List<Node>();
    }
}
