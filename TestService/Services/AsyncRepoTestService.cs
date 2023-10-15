using DK_NuGet_Library;
using DK_NuGet_Library.Interfaces;
using Microsoft.EntityFrameworkCore;
using TestService.Contexts;
using TestService.Models;

namespace TestService.Services
{
	public class AsyncRepoTestService
	{
		private readonly IAsyncRepository<TestContext> _repository;
		private static readonly string[] RandomQuotes = new string[5] { "People who annoy you", "Get in my belly", "Oh behave", "Get out of my swamp", "Made you look" };

		public AsyncRepoTestService(IAsyncRepository<TestContext> repository)
		{
			_repository = repository;
		}


		public async Task AddItem(BasicClass entry)
		{
			await _repository.AddItem(entry);
		}

		public async Task AddItems(List<BasicClass> items)
		{
			await _repository.AddItems(items);
		}

		public async Task RemoveItem(BasicClass entry)
		{
			await _repository.RemoveItem(entry);
		}

		public async Task RemoveItemByRefnr(int refnr)
		{
			await _repository.RemoveItem<BasicClass>(x => x.Refnr == refnr);
		}

		public async Task RemoveItemById(Guid id)
		{
			await _repository.RemoveItem<BasicClass>(x => x.Id == id);
		}

		public async Task RemoveItemsWhere(int refnr)
		{
			await _repository.RemoveItems<BasicClass>(query => query.Where(x => x.Refnr == refnr));
		}

		public async Task<List<BasicClass>> GetEntries()
		{
			return await _repository.GetAllItems<BasicClass>();
		}

		public async Task<List<string>> GetEntriesByColumn()
		{
			return await _repository.GetAllForColumn<BasicClass, string>(q => q.Select(x => x.TestField!));
		}

		public async Task<List<BasicClass>> GetEntriesWithCollection()
		{
			return await _repository.GetAllItems<BasicClass>(q => q.Include(x => x.BasicEntries));
		}

		public async Task<BasicClass> GetById(Guid id)
		{
			return await _repository.GetItem<BasicClass>(q => q.Where(x => x.Id == id).Include(c => c.BasicEntries));
		}

		public async Task<BasicClass> GetByRefnr(int refNr)
		{
			return await _repository.GetItem<BasicClass>(q => q.Where(x => x.Refnr == refNr).Include(c => c.BasicEntries));
		}

		public async Task<BasicClass> UpdateRandomCollectionItem()
		{
			List<BasicClass> items = await _repository.GetAllItems<BasicClass>(q => q.Include(e => e.BasicEntries));
			Random rand = new Random();
			BasicClass toEdit = items[rand.Next(0, items.Count)];
			BasicClass old = await _repository.GetItem<BasicClass>(query => query.Where(x => x.Id == toEdit.Id).Include(b => b.BasicEntries));

			toEdit.BasicEntries.Last().ValueToLoad = RandomQuotes[rand.Next(0, 4)];


			await _repository.UpdateItem(toEdit);
			return toEdit;
		}

		public async Task UpdateEntry()
		{
			Random rand = new Random();
			List<BasicClass> existing = await _repository.GetAllItems<BasicClass>(q => q.Include(e => e.BasicEntries))!;
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

			await _repository.UpdateItems(toUpdate);
		}



		public async Task DeleteEntries(int amount)
		{
			List<BasicClass> list = await _repository.GetAllItems<BasicClass>(q => q.Include(e => e.BasicEntries));
			List<BasicClass> toBeRemoved = new List<BasicClass>();
			for (int i = 0; i < amount; i++)
			{
				toBeRemoved.Add(list[i]);
			}
			await _repository.RemoveItems(toBeRemoved);

		}

		public async Task DeleteEntryWhere(int refnr)
		{
			await _repository.RemoveItems<BasicClass>(query => query.Where(x => x.Refnr == refnr));
		}


		public async Task GenerateNewEntries()
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
			await _repository.AddItems(newEntries);
		}
	}
}
