using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using todoAPI.Models;

public class dbContext : DbContext
{
    public dbContext(DbContextOptions<dbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // 添加 Courses 實體的映射配置
        modelBuilder.Entity<Courses>().ToTable("Courses");

        // 其他映射配置...

        base.OnModelCreating(modelBuilder);
    }


    public void InsertCourses(string courseId, string courseName, string teacher)
    {
        var courseIdParam = new SqlParameter("@CourseId", courseId);
        var courseNameParam = new SqlParameter("@CourseName", courseName);
        var teacherParam = new SqlParameter("@Teacher", teacher);

        this.Database.ExecuteSqlRaw("EXEC InsertCourses @CourseId, @CourseName, @Teacher",
            courseIdParam, courseNameParam, teacherParam);
    }

    public IEnumerable<Courses> GetAllCoursesFromStoredProcedure()
    {
        var results = new List<Courses>();

        try
        {
            var storedProcedureName = "GetAllCourses";
            results = this.Set<Courses>().FromSqlInterpolated($"EXEC {storedProcedureName}").ToList();

        }
        catch (Exception ex)
        {
            // 處理異常，例如日誌或返回空列表
            Console.WriteLine($"An error occurred while executing the stored procedure: {ex.Message}");
        }

        return results;
    }

}



//public class dbContext : DbContext
//{
//    public dbContext(DbContextOptions<dbContext> options)
//        : base(options)
//    {
//    }

//    public DbSet<Courses> Courses { get; set; } = null!;

//    //public List<Courses> GetCoursesUsingSqlQuery(string sqlQuery)
//    //{
//    //    return Courses.FromSqlRaw(sqlQuery).ToList();
//    //}

//    protected override void OnModelCreating(ModelBuilder modelBuilder)
//    {
//        modelBuilder.Entity<Courses>().HasKey(c => c.CourseId);
//        // 其他配置...
//    }
//}





