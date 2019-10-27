using DataAccess;
using Domain;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace TeamsAndPlayers
{
	/*
	+Создать приложение, которое позволяет выводить на экран спортивные команды и их игроков. 
	+Для этого используйте определённый тип загрузки, 
	+а также пагинацию. 
	+Спортивные команды на ваш вкус. 
	+Сделайте seed данных, чтобы в БД они просаживались по созданию.
	*/
	class Program
	{
		private static string serverName = "BorisHome\\Boris";

		private static Random random = new Random((int)DateTime.Now.Ticks);

		static string RandomString(int size)
		{
			StringBuilder builder = new StringBuilder();
			char ch;
			ch = Convert.ToChar(Convert.ToInt32(Math.Floor(26 * random.NextDouble() + 65)));
			builder.Append(ch);
			for (int i = 0; i < size; i++)
			{
				ch = Convert.ToChar(Convert.ToInt32(Math.Floor(26 * random.NextDouble() + 65)) + 32);
				builder.Append(ch);
			}
			return builder.ToString();
		}

		static void AddTeamsWithPlayers(int teams)
		{
			for (int i = 0; i < teams; i++)
			{
				Team team = new Team
				{
					Name = RandomString(random.Next(4, 8))
				};
				AddPlayers(team, 7);  // В моем случае это будут команды по киберспортивным дисциплинам, а значит 5 основных и 2 в запасе
			}
		}

		private static void AddPlayers(Team team, int quantity)
		{
			using (var context = new Context(serverName))
			{
				for (int i = 0; i < quantity; i++)
				{
					Player player = new Player
					{
						FullName = (RandomString(random.Next(3, 8)) + " " + RandomString(random.Next(3, 8))),
						Team = team
					};
					context.Add(player);
					context.SaveChanges();
				}
			}
		}

		static void Show()
		{
			int teamsCount;
			int pages, pageSize = 3, page;

			using (var context = new Context(serverName))
			{
				teamsCount = context.Teams.ToList().Count;
			}
			pages = teamsCount / pageSize;
			if (teamsCount % pageSize != 0)
			{
				pages++;
			}
			while (true)
			{
				Console.WriteLine($"Введите страницу (1 - {pages}; 0 - Выход):");

				if (Int32.TryParse(Console.ReadLine(), out page) == false || page > pages || page < 0)
				{
					page = 1;
					Console.WriteLine("Введен неверный номер страницы");
					Console.ReadLine();
					Console.Clear();
					continue;
				}

				if (page == 0) { return; }

				ShowPage(pageSize, page);
			}
		}

		private static void ShowPage(int pageSize, int page)
		{
			Console.Clear();
			Console.WriteLine($"Страница {page}:");
			using (var context = new Context(serverName))
			{
				var result = context.Teams.OrderBy(x => x.Name).ToList();
				var pagingResult = result.Skip((page - 1) * pageSize).Take(pageSize);

				foreach (var team in pagingResult)
				{
					Console.WriteLine($"\tИгроки команды {team.Name}:");
					List<Player> players = team.Players.OrderBy(x => x.FullName).ToList();
					foreach (var player in players)
					{
						Console.WriteLine($"{player.FullName}");
					}
				}
			}
		}

		static void Main()
		{
			AddTeamsWithPlayers(0);	// Введите необходимое кол-во команд для добавления в БД
			Show();
		}
	}
}
