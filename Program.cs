using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Serilog;
using System;
using System.IO;

namespace EFCoreOverride
{
    public abstract class EFCoreTest
    {
        public DbContextOptions<ApplicationDbContext> ServerOptions { get; }

        public EFCoreTest()
        {
            Directory.CreateDirectory("./../../../Logs/");
            string sqllitePath = Path.Combine("./../../../Logs/", "test.db");

            Log.Logger = new LoggerConfiguration()
                .WriteTo.File("./../../../Logs/log.txt", outputTemplate: "{Timestamp:HH:mm:ss} [{Level:u3}] {Message:lj}{NewLine}{Exception}")
                .WriteTo.Console()
                .CreateLogger();

            ServerOptions = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseSqlite("Data Source=" + sqllitePath)
                .EnableSensitiveDataLogging(true)
                .EnableDetailedErrors(true)
                .LogTo(Log.Information, new[]
                {
                    DbLoggerCategory.Database.Command.Name,
                    DbLoggerCategory.Update.Name
                },
                LogLevel.Information)
                .Options;

            using (var testContext = new ApplicationDbContext(ServerOptions))
            {
                testContext.Database.EnsureCreated();
            }
        }
    }

    public class EventsTest : EFCoreTest
    {
        public void WriteUploadEvent()
        {
            using (var testContext = new ApplicationDbContext(ServerOptions))
            {
                var uploadEvent = new FileUploadEvent()
                {
                    Data = new FileUploadData()
                    {
                        Filename = "TEST FILE",
                        UploadUrl = "www.google.com"
                    },
                    Time = DateTime.UtcNow,
                };

                Log.Information(uploadEvent.Content);
                testContext.TestEvents.Add(uploadEvent);
                Log.Information(uploadEvent.Content);
                testContext.SaveChanges();
            }
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            var eventsTest = new EventsTest();
            eventsTest.WriteUploadEvent();
        }
    }
}
