using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Globalization;
using CsvHelper;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CsvHelper.Configuration;
using todoAPI.Models;
using Serilog;

[ApiController]
[Route("api/[controller]")]
public class CsvController : ControllerBase
{
    private readonly dbContext _dbContext;

    public CsvController(dbContext dbContext)
    {
        _dbContext = dbContext;

        if (_dbContext == null)
        {
            throw new InvalidOperationException("DbContext is not registered in the DI container.");
        }
    }

    [HttpPost("upload")]
    public async Task<IActionResult> UploadFile([FromForm] IFormFile file)
    {
        try
        {
            // 刪除數據庫中原有的內容
            _dbContext.Courses.RemoveRange(_dbContext.Courses);

            // 處理 CSV 檔案內容
            using (var reader = new StreamReader(file.OpenReadStream()))
            using (var csv = new CsvReader(reader, new CsvConfiguration(CultureInfo.InvariantCulture)))
            {
                _dbContext.Database.EnsureCreated();
                var records = csv.GetRecords<Courses>().ToList();

                var coursesToAdd = records.Select(data => new Courses
                {
                    CourseId = data.CourseId,
                    CourseName = data.CourseName,
                    Teacher = data.Teacher
                }).ToList();

                _dbContext.Courses.AddRange(coursesToAdd);
                _dbContext.SaveChanges();
            }

            return Ok("CSV data has been stored in the database.");
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal Server Error: {ex.Message}");
        }
    }


    [HttpGet("export")]
    public IActionResult ExportCsv()
    {
        try
        {
            var records = _dbContext.Courses.ToList();

            using (var memoryStream = new MemoryStream())
            using (var writer = new StreamWriter(memoryStream))
            using (var csvWriter = new CsvWriter(writer, new CsvConfiguration(CultureInfo.InvariantCulture)))
            {
                csvWriter.WriteRecords(records);
                writer.Flush();
                memoryStream.Seek(0, SeekOrigin.Begin);

                return File(memoryStream.ToArray(), "text/csv", "exported_data.csv");
            }
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal Server Error: {ex.Message}");
        }
    }
}
