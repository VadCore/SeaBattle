namespace SeaBattle.Domain.Entities
{
	public class Player : BaseEntity<Player>
	{
		public int BoardId { get; set; }
		public string Nick { get; set; }
		public int ActiveUnitsCount { get; set; }

		public Board Board { get; set; }

		//public int UserId { get; set; }

		public Player(string nick, int boardId)
		{
			Nick = nick;
			BoardId = boardId;
		}

		public Player()
		{
		}
	}
}
