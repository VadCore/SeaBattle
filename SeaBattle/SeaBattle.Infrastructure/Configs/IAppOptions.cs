namespace SeaBattle.UI.Configs
{
	public interface IAppOptions
	{
		public string JsonDataPath { get; }
		public string DbConnectionString { get; }
		public bool IsSerializable { get; }
	}
}
