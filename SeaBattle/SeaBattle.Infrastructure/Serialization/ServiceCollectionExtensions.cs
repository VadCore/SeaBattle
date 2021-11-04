using Microsoft.Extensions.DependencyInjection;

namespace SeaBattle.Infrastructure.Serialization
{
	public static class ServiceCollectionExtensions
	{
		public static void AddSerializationContext<TContext>(this IServiceCollection services, string jsonDataPath) where TContext : SerializationContext<TContext>, new()
		{
			services.AddScoped<TContext>(options =>
			{
				var context = SerializationContext<TContext>.Load(jsonDataPath);

				if (context is null)
				{
					context = new TContext { JsonDataPath = jsonDataPath };
				}

				return context;
			});
		}
	}
}
