using System;
using Microsoft.EntityFrameworkCore;
using todoAPI.Models;

namespace todoAPI.Models
{
}

public class dbContext : DbContext
{
    public dbContext(DbContextOptions<dbContext> options)
        : base(options)
    {
    }

    public DbSet<Courses> Courses { get; set; } = null!;

    //public List<Courses> GetCoursesUsingSqlQuery(string sqlQuery)
    //{
    //    return Courses.FromSqlRaw(sqlQuery).ToList();
    //}

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Courses>().HasKey(c => c.CourseId);
        // 其他配置...
    }
}


