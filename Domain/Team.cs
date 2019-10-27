using System;
using System.Collections.Generic;

namespace Domain
{
	public class Team
	{
		public Guid Id { get; set; } = Guid.NewGuid();
		public string Name { get; set; }
		public virtual ICollection<Player> Players { get; set; }
	}
}
