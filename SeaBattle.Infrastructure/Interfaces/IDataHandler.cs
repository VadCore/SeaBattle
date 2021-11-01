using SeaBattle.Infrastructure.Serialization;

namespace SeaBattle.Infrastructure.Interfaces
{
	public interface IDataHandler<TContext> where TContext : SerializationContext<TContext>
	{
		public void SaveContext(TContext context);

		public TContext Load();
	}
}
