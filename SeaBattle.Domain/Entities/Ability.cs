namespace SeaBattle.Domain.Entities
{
	public abstract class Ability<TAbility> : BaseEntity<TAbility> where TAbility : Ability<TAbility>
	{
		public int ShipId { get; set; }
		public int ReloadTurn { get; set; }

		public Ability(int shipId)
		{
			ShipId = shipId;
		}
		public Ability()
		{
		}
	}
}
