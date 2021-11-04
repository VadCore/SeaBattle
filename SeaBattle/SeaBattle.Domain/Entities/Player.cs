namespace SeaBattle.Domain.Entities
{
	public class Player : BaseEntity<Player>
	{
		public int BoardId { get; set; }
		public int ActiveUnitsCount { get; set; }

		public Board Board { get; set; }

		//public User User { get; set; }

		public int UserId { get; set; }

		public Player(User user, int boardId)
		{
			BoardId = boardId;
			UserId = user.Id;
		}

		public Player()
		{
		}
	}
}
