
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;

namespace System.Linq.Dynamic.Tests.Helpers.Entities
{
    public class BlogContext : DbContext
    {
        public BlogContext(string connectionString)
            : base(connectionString)
        {

        }

        public DbSet<Blog> Blogs { get; set; }

        public DbSet<Post> Posts { get; set; }
    }
}
