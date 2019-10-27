using System;

namespace Domain
{
	public class Player
	{
		public Guid Id { get; set; } = Guid.NewGuid();
		public string FullName { get; set; }
		public virtual Team Team { get; set; }
	}
}
