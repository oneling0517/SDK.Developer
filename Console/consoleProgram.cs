using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using CsvHelper;
using CsvHelper.Configuration;
//using Internal;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using todoAPI.Models;


namespace CsvToDatabase
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            try
            {
                // 上傳 CSV 到資料庫
                await UploadCsvToDatabase();

                // 從資料庫匯出資料為 CSV
                await ExportDatabaseToCsv();

                Console.WriteLine("Process completed successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An unexpected error occurred. Details: {ex.Message}");
            }
        }

        public static async Task UploadCsvToDatabase()
        {
            using (var reader = new StreamReader("course.csv"))
            using (var csv = new CsvReader(reader, new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                HeaderValidated = null,
                MissingFieldFound = null
            }))
            {
                var records = csv.GetRecords<Courses>().ToList();

                using (var httpClient = new HttpClient())
                {
                    var jsonContent = Newtonsoft.Json.JsonConvert.SerializeObject(records);
                    var stringContent = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                    // 設定 Web API 的 URL
                    var apiUrl = "https://localhost:7289/swagger/index.html";

                    // 發送 HTTP POST 請求至 Web API
                    var response = await httpClient.PostAsync(apiUrl, stringContent);

                    // 檢查回應是否成功
                    if (response.IsSuccessStatusCode)
                    {
                        Console.WriteLine("Data successfully uploaded to the Web API.");
                    }
                    else
                    {
                        Console.WriteLine($"Failed to upload data. Status code: {response.StatusCode}");
                    }
                }
            }
        }


        public static async Task ExportDatabaseToCsv()
        {
            using (var httpClient = new HttpClient())
            {
                var apiUrl = "https://localhost:7289/api/data/get"; // Web API 取得資料的端點

                var response = await httpClient.GetAsync(apiUrl);

                if (response.IsSuccessStatusCode)
                {
                    var responseData = await response.Content.ReadAsStringAsync();

                    // 輸出回應內容（除錯用）
                    Console.WriteLine("Web API Response:");
                    Console.WriteLine(responseData);

                    using (var writer = new StreamWriter("exported_data.csv"))
                    {
                        writer.Write(responseData);
                        Console.WriteLine("Data successfully exported from the Web API to CSV.");
                    }
                }
                else
                {
                    Console.WriteLine($"Failed to export data. Status code: {response.StatusCode}");
                }
            }
        }

    }
}

