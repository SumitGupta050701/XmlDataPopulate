using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using System;
using System.Xml.Linq;
using System.Xml.Serialization;
using XmlDataPopulate.Context;
using XmlDataPopulate.Model;

namespace XmlDataPopulate.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class NodeController : Controller
    {
        private readonly NodeDbContext _context;

        public NodeController(NodeDbContext context)
        {
            _context = context;
        }

       

        [HttpGet("xml/{id}")]
        public async Task<ActionResult<string>> GetNodeXml(Guid id)
        {
            var node = await GetNodeWithChildrenAsync(id);

            if (node == null)
            {
                return NotFound();
            }

            var xml = SerializeToXml(node);
            return Content(xml, "application/xml");
        }

       

        [HttpPost("xml")]
        public async Task<IActionResult> PostNodeXml([FromBody] string xmlContent)
        {
            var node = DeserializeFromXml(xmlContent);

            if (node == null)
            {
                return BadRequest("Invalid XML data.");
            }

            _context.Nodes.Add(node);
            await _context.SaveChangesAsync();

            return Ok(node);
        }

        private Node DeserializeFromXml(string xmlContent)
        {
            var serializer = new XmlSerializer(typeof(Node));
            using (var reader = new StringReader(xmlContent))
            {
                return (Node)serializer.Deserialize(reader);
            }
        }
        private async Task LoadChildrenRecursively(Node node)
        {
            foreach (var child in node.Children)
            {
                // Load children for this child
                _context.Entry(child).Collection(c => c.Children).Load();

                // Recursively load children of the child
                await LoadChildrenRecursively(child);
            }
        }

        private async Task<Node> GetNodeWithChildrenAsync(Guid id)
        {
            var node = await _context.Nodes
                .Where(n => n.Id == id)
                .Include(n => n.Children)
                .ThenInclude(c => c.Children) // Adjust as needed for deeper levels
                .SingleOrDefaultAsync();

            if (node == null)
                return null;

            // Load children recursively
            await LoadChildrenRecursively(node);
            return node;
        }

        private string SerializeToXml(Node node)
        {
            var xElement = new XElement("Node",
                new XAttribute("Name", node.Name),
                new XAttribute("NodeType", node.NodeType),
                // Recursively include children
                node.Children.Select(child => SerializeChildToXml(child))
            );

            return xElement.ToString();
        }

        private XElement SerializeChildToXml(Node child)
        {
            return new XElement("Node",
                new XAttribute("Name", child.Name),
                new XAttribute("NodeType", child.NodeType),
                // Recursively include children's children
                child.Children.Select(grandChild => SerializeChildToXml(grandChild))
            );
        }
    }
}
