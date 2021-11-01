using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using SeaBattle.Application.Services;
using SeaBattle.Application.Services.Interfaces;
using SeaBattle.Domain.Entities;
using SeaBattle.Domain.Interfaces;
using SeaBattle.Infrastructure;
using SeaBattle.Infrastructure.CustomIdentityProvider;
using SeaBattle.Infrastructure.Repositories;
using SeaBattle.Infrastructure.Serialization;
using SeaBattle.UI.Configs;
using System.IO;
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

					services.AddScoped<IUserStore<User>, CustomUserStore>();
					services.AddScoped<IRoleStore<Role>, CustomRoleStore>();

					if (Configuration.GetValue<bool>("AppOptions:IsSerializable"))
					{
						var jsonDataPath = Configuration.GetValue<string>("AppOptions:JsonDataPath");
						services.AddSerializationContext<SeaBattleSerializationContext>(jsonDataPath);
						services.AddScoped(typeof(IRepository<>), typeof(SerializationRepository<>));
					}
					else
					{
						services.AddScoped(typeof(IRepository<>), typeof(ORMRepository<>));
						services.AddScoped<SeaBattleORMContext>();
					}


					services.AddScoped<IUserService, UserService>();
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
