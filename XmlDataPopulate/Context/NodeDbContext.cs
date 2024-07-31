
using Microsoft.EntityFrameworkCore;
using System;
using System.Drawing;
using XmlDataPopulate.Migrations;
using XmlDataPopulate.Model;
using static System.Runtime.InteropServices.JavaScript.JSType;
namespace XmlDataPopulate.Context
{
    public class NodeDbContext : DbContext
    {
        public NodeDbContext(DbContextOptions<NodeDbContext> options) : base(options) { }
        public DbSet<Node> Nodes { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Data Source = localhost\\SQLEXPRESS; Initial Catalog = NodeApp; Integrated Security = True; Trust Server Certificate = True");
        }

     
    }
}
