using DK.GenericLibrary.Interfaces;
using Microsoft.EntityFrameworkCore;
using TestService.Contexts;
using TestService.Models;

namespace TestService.Services
{
	public class RepoTestService
	{
		private static readonly string[] RandomQuotes = new string[5] { "People who annoy you", "Get in my belly", "Oh behave", "Get out of my swamp", "Made you look" };


		private readonly IRepository<TestContext> _repository;

		public RepoTestService(IRepository<TestContext> repository)
		{
			_repository = repository;
		}


		/// <summary>
		/// Tests the default GetAllItems
		/// </summary>
		/// <returns></returns>
		public List<BasicClass> GetEntries()
		{
			return _repository.GetAllItems<BasicClass>();
		}


		public List<string> GetColumn()
		{
			return _repository.GetAllItems<BasicClass, string>(q => q.Select(x => x.TestField)!);
		}

		public List<BasicClass> GetEntriesWithCollection()
		{
			return _repository.GetAllItems<BasicClass>(q => q.Include(x => x.BasicEntries))!;
		}




		public BasicClass GetById(Guid id)
		{
			return _repository.GetItem<BasicClass>(query => query.Where(x => x.Id == id).Include("BasicEntries"))!;
		}

		public BasicClass GetByRefnr(int refnr)
		{
			return _repository.GetItem<BasicClass>(query => query.Where(x => x.Refnr == refnr).Include("BasicEntries"))!;
		}

		/// <summary>
		/// Tests GetAllItems with sorting query
		/// </summary>
		/// <returns></returns>
		public List<BasicClass> GetByAgeDesc()
		{
			return _repository.GetAllItems<BasicClass>(query => query.Include("BasicEntries").OrderByDescending(e => e.Oprettet))!;
		}

		/// <summary>
		/// Tests GetALlItems with different query
		/// </summary>
		/// <returns></returns>
		public List<BasicClass> GetByAge()
		{
			return _repository.GetAllItems<BasicClass>(query => query.OrderBy(x => x.Refnr).Include("BasicEntries")
				.GroupBy(r => r.Refnr).Select(entitites => entitites.OrderByDescending(e => e.Oprettet).First()));
		}


		/// <summary>
		/// Tests AddItem function
		/// </summary>
		/// <param name="entry"></param>
		public void SaveEntry(BasicClass entry)
		{
			_repository.AddItem(entry);
		}

		/// <summary>
		/// Updates random entry with a random quote
		/// Tests UpdateItem function
		/// </summary>
		public BasicClass UpdateRandomCollectionItem()
		{
			List<BasicClass> items = _repository.GetAllItems<BasicClass>(q => q.Include(e => e.BasicEntries));
			Random rand = new Random();
			BasicClass toEdit = items[rand.Next(0, items.Count)];
			BasicClass old = _repository.GetItem<BasicClass>(query => query.Where(x => x.Id == toEdit.Id).Include(b => b.BasicEntries));

			toEdit.BasicEntries.Last().ValueToLoad = RandomQuotes[rand.Next(0, 4)];


			_repository.UpdateItem(toEdit);
			return toEdit;
		}

		/// <summary>
		/// Tests RemoveItem function
		/// </summary>
		/// <param name="id"></param>
		public void DeleteEntry(Guid id)
		{
			_repository.RemoveItem<BasicClass>(x => x.Id == id);
		}


		/// <summary>
		/// Delete a set amount of entries
		/// Tests RemoveItems and can be used to remove clutter
		/// </summary>
		/// <param name="amount"></param>
		public void DeleteEntries(int amount)
		{
			List<BasicClass> list = _repository.GetAllItems<BasicClass>(q => q.Include(e => e.BasicEntries));
			List<BasicClass> toBeRemoved = new List<BasicClass>();
			for (int i = 0; i < amount; i++)
			{
				toBeRemoved.Add(list[i]);
			}
			_repository.RemoveItems(toBeRemoved);
		}

		/// <summary>
		/// Tests the RemoveItems function
		/// </summary>
		/// <param name="refnr"></param>
		public void DeleteEntryWhere(int refnr)
		{
			_repository.RemoveItems<BasicClass>(query => query.Where(x => x.Refnr == refnr));
		}

		/// <summary>
		/// Picks 5 random entries and updates its collection
		/// Tests the UpdateItems function
		/// </summary>
		public void UpdateEntry()
		{
			Random rand = new Random();
			List<BasicClass> existing = _repository.GetAllItems<BasicClass>(q => q.Include(e => e.BasicEntries))!;
			List<BasicClass> toUpdate = new List<BasicClass>();

			for (int i = 0; i < 5; i++)
			{
				int randNumber = rand.Next(0, existing.Count - 1);
				toUpdate.Add(existing[randNumber]);
				existing.Remove(existing[randNumber]);
			}
			foreach (var e in toUpdate)
			{
				foreach (var c in e.BasicEntries)
				{
					c.ValueToLoad = rand.Next(0, 10000).ToString();
				}

			}

			_repository.UpdateItems(toUpdate);
		}


		/// <summary>
		/// Adds 5 entities with 5 items in their collection to the database
		/// Tests the AddItems function, and provides entries to further test with
		/// </summary>
		public void GenerateNewEntries()
		{

			List<BasicClass> newEntries = new();
			Random rand = new Random();
			for (int i = 0; i < 5; i++)
			{
				newEntries.Add(new BasicClass()
				{
					Id = new Guid(),
					TestField = "Test" + i,
					Refnr = rand.Next(100, 10000)
				});
			}

			for (int i = 0; i < newEntries.Count; i++)
			{
				for (int j = 0; j < 5; j++)
				{
					newEntries[i].BasicEntries.Add(new BasicEntry()
					{
						Id = new Guid(),
						BasicClassId = newEntries[i].Id,
						ValueToLoad = "load this" + j
					});
				}

			}
			_repository.AddItems(newEntries);
		}
	}
}
