using Microsoft.AspNetCore.Mvc;
using TestService.Models;
using TestService.Services;

namespace TestService.Controllers
{
	[Route("AsyncTestService")]
	public class AsyncTestController : ControllerBase
	{
		private readonly AsyncRepoTestService _repoTestService;

		public AsyncTestController(AsyncRepoTestService repoTestService)
		{
			_repoTestService = repoTestService;
		}


		[HttpGet]
		[Route("all")]
		public async Task<List<BasicClass>> GetAllItems()
		{
			return await _repoTestService.GetEntries();
		}

		[HttpGet]
		[Route("all/includecollection")]
		public async Task<List<BasicClass>> GetAllItemsWithCollection()
		{
			return await _repoTestService.GetEntriesWithCollection();
		}

		[HttpGet]
		[Route("by/refnr/{refnr}")]
		public async Task<BasicClass> GetByRefnr(int refnr)
		{
			return await _repoTestService.GetByRefnr(refnr);
		}

		[HttpGet]
		[Route("all/column")]
		public async Task<List<string>> GetColumns()
		{
			return await _repoTestService.GetEntriesByColumn();
		}

		[HttpPost]
		[Route("new")]
		public async Task AddItem([FromBody] BasicClass entry)
		{
			await _repoTestService.AddItem(entry);
		}

		[HttpPost]
		[Route("generate")]
		public async Task GenerateEntries()
		{
			await _repoTestService.GenerateNewEntries();
		}

		[HttpDelete]
		[Route("delete")]
		public async Task DeleteBasicEntry(Guid id)
		{
			await _repoTestService.RemoveItemById(id);
		}

		[HttpDelete]
		[Route("delete/refnr{refnr}")]
		public async Task DeleteBasicEntry(int refnr)
		{
			await _repoTestService.DeleteEntryWhere(refnr);
		}

		[HttpDelete]
		[Route("delete/amount/{amount}")]
		public async Task DeleteXEntries(int amount)
		{
			await _repoTestService.DeleteEntries(amount);
		}


		[HttpPatch]
		[Route("update")]
		public async Task UpdateBasicEntry()
		{
			await _repoTestService.UpdateEntry();
		}

		[HttpPatch]
		[Route("updateRandom")]
		public async Task<BasicClass> UpdateRandom()
		{
			return await _repoTestService.UpdateRandomCollectionItem();
		}
	}
}
