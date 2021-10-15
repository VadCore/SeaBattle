using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SeaBattle.Application.Services;
using SeaBattle.Application.Services.Interfaces;
using SeaBattle.Domain.Interfaces;
using SeaBattle.Infrastructure;
using SeaBattle.Infrastructure.Data;
using SeaBattle.Infrastructure.Interfaces;
using SeaBattle.Infrastructure.Repositories;
using SeaBattle.UI.Configs;
using Microsoft.Extensions.Configuration;
using System;
using Microsoft.Extensions.Options;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using System.Reflection;

namespace SeaBattle.UI
{
    class Program
    {

		static void Main(string[] args)
		{

			var builder = new ConfigurationBuilder()
				.SetBasePath(Directory.GetCurrentDirectory())
				.AddJsonFile("appsettings.json", optional: false);

			var Configuration = builder.Build();

			var SomePath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

		

			var host = Host.CreateDefaultBuilder()
				.ConfigureServices((context, services) =>
				{
					services.AddSingleton<IConfiguration>(provider => Configuration);

					services.Configure<AppOptions>(Configuration.GetSection(nameof(AppOptions)));
					services.AddSingleton<IAppOptions>(options => options.GetService<IOptions<AppOptions>>().Value);

					//services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
					//services.AddScoped<IUnitOfWork, UnitOfWork>();

					services.AddScoped(typeof(IRepository<>), typeof(ORMRepository<>));
					services.AddScoped<SeaBattleContext>();

					services.AddScoped<IDataHandler, DataHandler>();

					services.AddScoped<IUnitOfWork>(options =>
					{
						var dataHandler = ((IDataHandler)options.GetService(typeof(IDataHandler)));
						var unitOfWork = dataHandler.Load();

						if (unitOfWork is null)
						{
							unitOfWork = new UnitOfWork(dataHandler);
						}

						return unitOfWork;
					});

					services.AddScoped<IBoardService, BoardService>();
					services.AddScoped<IPlayerService, PlayerService>();
					services.AddScoped<IShipService, ShipService>();
					services.AddScoped<ISupportAbilityService, SupportAbilityService>();
					services.AddScoped<IBattleAbilityService, BattleAbilityService>();
				}).Build();

			

			ActivatorUtilities.CreateInstance<Application>(host.Services).Run();
		}
	}
}
