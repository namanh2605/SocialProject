using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using SocialProject.Data.Models;

namespace SocialProject.Data;

public partial class SocialMediaContext : DbContext
{
    public SocialMediaContext()
    {
    }

    public SocialMediaContext(DbContextOptions<SocialMediaContext> options)
        : base(options)
    {
    }

    
    public DbSet<Post> Posts { get; set; }

}
