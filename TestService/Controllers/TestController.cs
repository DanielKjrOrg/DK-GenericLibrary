using Microsoft.AspNetCore.Mvc;
using TestService.Models;
using TestService.Services;

namespace TestService.Controllers
{
	[Route("TestService")]
	public class TestController : ControllerBase
	{
		private readonly RepoTestService _testService;

		public TestController(RepoTestService genericTestService)
		{
			_testService = genericTestService;
		}

		[HttpGet]
		[Route("all")]
		public List<BasicClass> GetAllItems()
		{
			return _testService.GetEntries();
		}

		[HttpGet]
		[Route("all/columns")]
		public List<string> GetColumns()
		{
			return _testService.GetColumn();
		}

		[HttpGet]
		[Route("all/withcollection")]
		public List<BasicClass> GetAllItemsWithCollection()
		{
			return _testService.GetEntriesWithCollection();
		}

		[HttpGet]
		[Route("all/by/age")]
		public List<BasicClass> GetAllItemsByAge()
		{
			return _testService.GetByAge();
		}

		[HttpGet]
		[Route("all/by/age/desc")]
		public List<BasicClass> GetAllItemsByAgeDesc()
		{
			return _testService.GetByAgeDesc();
		}
		[HttpGet]
		[Route("by/id/{id}")]
		public BasicClass GetById(Guid id)
		{
			return _testService.GetById(id);
		}


		[HttpGet]
		[Route("by/refnr/{refnr}")]
		public BasicClass GetByRefnr(int refnr)
		{
			return _testService.GetByRefnr(refnr);
		}



		[HttpPost]
		[Route("new")]
		public void AddNewBasicEntry([FromBody] BasicClass entry)
		{
			_testService.SaveEntry(entry);
		}

		[HttpPost]
		[Route("generate")]
		public void GenerateEntries()
		{
			_testService.GenerateNewEntries();
		}

		[HttpDelete]
		[Route("delete")]
		public void DeleteBasicEntry(Guid id)
		{
			_testService.DeleteEntry(id);
		}

		[HttpDelete]
		[Route("delete/refnr{refnr}")]
		public void DeleteBasicEntry(int refnr)
		{
			_testService.DeleteEntryWhere(refnr);
		}

		[HttpDelete]
		[Route("delete/amount/{amount}")]
		public void DeleteXEntries(int amount)
		{
			_testService.DeleteEntries(amount);
		}

	

		[HttpPatch]
		[Route("update")]
		public void UpdateBasicEntry()
		{
			_testService.UpdateEntry();
		}

		[HttpPatch]
		[Route("updateRandom")]
		public BasicClass UpdateRandom()
		{
			return _testService.UpdateRandomCollectionItem();
		}
	}
}
