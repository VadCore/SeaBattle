using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SeaBattle.Application.Services;
using SeaBattle.Application.Services.Interfaces;
using SeaBattle.Domain.Interfaces;
using SeaBattle.Infrastructure;
using SeaBattle.Infrastructure.Data;
using SeaBattle.Infrastructure.Interfaces;
using SeaBattle.Infrastructure.Repositories;
using System;

namespace SeaBattle.UI
{
    class Program
    {
		string connectionString = "Server=(localdb)\\mssqllocaldb;Database=SeaBattleTestDB1;Trusted_Connection=True;";
		static void Main(string[] args)
		{
			var host = Host.CreateDefaultBuilder()
				.ConfigureServices((context, services) =>
				{
					services.AddScoped(typeof(IDataHandler<>), typeof(DataHandler<>));

					services.AddScoped<IDataContext>(options =>
					{
						var dataHandler = ((IDataHandler<SeaBattleContext>)options.GetService(typeof(IDataHandler<SeaBattleContext>)));
						var context = dataHandler.Load();

						if (context is null)
						{
							context = new SeaBattleContext(dataHandler);
						}

						return context;
					});

					services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
					services.AddScoped<IUnitOfWork, UnitOfWork>();

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
