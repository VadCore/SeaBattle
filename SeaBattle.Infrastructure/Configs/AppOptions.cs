namespace SeaBattle.UI.Configs
{
	public class AppOptions : IAppOptions
	{
		public string JsonDataPath { get; set; }

		public string DbConnectionString { get; set; }

		public bool IsSerializable { get; set; }
	}
}
