
using EmploymentDotNetTask.Data;
using EmploymentDotNetTask.Helpers.ImplementSection;
using EmploymentDotNetTask.Helpers.Interface;
using EmploymentDotNetTask.IServices;
using EmploymentDotNetTask.Services;
using Microsoft.EntityFrameworkCore;
using NLog.Extensions.Logging;

namespace EmploymentDotNetTask.API
{
	public class Program
	{
		public static void Main(string[] args)
		{

			NLogProviderOptions nlpopts = new NLogProviderOptions
			{
				IgnoreEmptyEventId = true,
				CaptureMessageTemplates = true,
				CaptureMessageProperties = true,
				ParseMessageTemplates = true,
				IncludeScopes = true,
				ShutdownOnDispose = true
			};
			//Get AppSettings, this should be a hiden file through Manage user secrets
			IConfiguration config = new ConfigurationBuilder()
							.AddJsonFile("appsettings.json")
							.Build();

			var builder = WebApplication.CreateBuilder(args);

			// Add services to the container.

			builder.Services.AddControllers();



			//Add Logging to project
			builder.Services.AddLogging(
					builder =>
					{
						builder.AddConsole().SetMinimumLevel(LogLevel.Trace);
						builder.SetMinimumLevel(LogLevel.Trace);
						builder.AddNLog(nlpopts);
					});
			builder.Host.ConfigureLogging(logging =>
			{
				logging.ClearProviders();
				logging.AddNLog();
			});

			//Dependency Injection of Services and Helpers
			builder.Services.AddScoped<IApplicationService, ApplicationService>();
			builder.Services.AddScoped<IQuestionTypeService, QuestionTypeService>();
			builder.Services.AddScoped<ILogHelper, LogHelper>();


			// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
			builder.Services.AddEndpointsApiExplorer();
			builder.Services.AddSwaggerGen();

			//Add Database
			var cosmosPK = config["CosmosDb:PrimaryKey"];
			var cosmosUri = config["CosmosDb:URI"];
			var consmosDatabase = config["CosmosDb:DatabaseName"];
			builder.Services.AddDbContext<EmploymentDbContext>(options =>
			   options.UseCosmos(cosmosUri, cosmosPK, consmosDatabase));

			var app = builder.Build();

			// Configure the HTTP request pipeline.
			if (app.Environment.IsDevelopment())
			{
				app.UseSwagger();
				app.UseSwaggerUI();
			}

			app.UseHttpsRedirection();

			app.UseAuthorization();


			app.MapControllers();

			app.Run();
		}
	}
}
