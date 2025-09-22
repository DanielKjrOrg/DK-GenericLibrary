using DK.GenericLibrary.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using RepoUnitTests.Fakes;
using static NUnit.Framework.Assert;

namespace RepoUnitTests
{
	class RepoTests
	{
		private ServiceProvider _serviceProvider;

		[SetUp]
		public void Setup()
		{
			_serviceProvider = TestServiceProvider.GetTransientServiceProvider(false);
		}

		[TearDown]
		public void Dispose()
		{
			_serviceProvider.Dispose();
		}



		[Test]
		public void CanUseScopedServiceProvider()
		{
			using var scope = TestServiceProvider.GetScopedAsyncServiceProvider(false);
			var dbContextFactory = scope.GetService<IDbContextFactory<FakeContext>>();
			var context = dbContextFactory!.CreateDbContext();
			var created = context.Database.EnsureCreated();
			True(created);
		}
		[Test]
		public void CanUseTransientServiceProvider()
		{
			using var scope = TestServiceProvider.GetTransientServiceProvider(false);
			var dbContextFactory = scope.GetService<IDbContextFactory<FakeContext>>();
			var context = dbContextFactory!.CreateDbContext();
			var created = context.Database.EnsureCreated();
			True(created);
		}

		[Test]
		public void CanUseSingletonServiceProvider()
		{
			using var scope = TestServiceProvider.GetSingletonAsyncServiceProvider(false);
			var dbContextFactory = scope.GetService<IDbContextFactory<FakeContext>>();
			var context = dbContextFactory!.CreateDbContext();
			var created = context.Database.EnsureCreated();
			True(created);
		}
		[Test]
		public void CanAdd()
		{
			//Arrange
			using var scope = _serviceProvider.CreateScope();
			var repository = scope.ServiceProvider.GetRequiredService<IRepository<FakeContext>>();
			var baseClass = new BasicClass { TestField = "Test1", Refnr = 1 };
			baseClass.BasicEntries.Add(new BasicEntry() { BasicClassId = baseClass.Id, ValueToLoad = "Test1" });

			// Act
			repository.AddItem(baseClass);

			//Assert
			var result = repository.GetItem<BasicClass>(q => q.Where(x => x.Id == baseClass.Id));
			That(result != null);

		}

		[Test]
		public void CanAddMultiple()
		{

			List<BasicClass> basicClassList = new List<BasicClass>()
			{
				new BasicClass { TestField = "Test1", Refnr = 1 },
				new BasicClass { TestField = "Test2", Refnr = 2 },
				new BasicClass { TestField = "Test3", Refnr = 3 }
			};


			using var scope = _serviceProvider.CreateScope();
			var repository = scope.ServiceProvider.GetRequiredService<IRepository<FakeContext>>();

			repository.AddItems(basicClassList);

			var result = repository.GetAllItems<BasicClass>();
			That(result.Count == 3);

		}

		[Test]
		public void CanRemove()
		{

			//Arrange
			using var scope = _serviceProvider.CreateScope();
			var repository = scope.ServiceProvider.GetRequiredService<IRepository<FakeContext>>();
			var baseClass = new BasicClass { TestField = "Test1", Refnr = 1 };
			baseClass.BasicEntries.Add(new BasicEntry() { BasicClassId = baseClass.Id, ValueToLoad = "Test1" });
			repository.AddItem(baseClass);

			//Ensure it is added
			var addedItem = repository.GetItem<BasicClass>(q => q.Where(x => x.Id == baseClass.Id));
			That(addedItem != null);
			//Act
			repository.RemoveItem(baseClass);

			//Assert
			var result = repository.GetItem<BasicClass>(q => q.Where(x => x.Id == baseClass.Id));
			That(result == null);

		}


		[Test]
		public void CanRemoveByExpression()
		{

			//Arrange
			using var scope = _serviceProvider.CreateScope();
			var repository = scope.ServiceProvider.GetRequiredService<IRepository<FakeContext>>();
			var baseClass = new BasicClass { TestField = "Test1", Refnr = 1 };
			baseClass.BasicEntries.Add(new BasicEntry() { BasicClassId = baseClass.Id, ValueToLoad = "Test1" });

			repository.AddItem(baseClass);

			var addedItem = repository.GetItem<BasicClass>(q => q.Where(x => x.Id == baseClass.Id));
			That(addedItem != null);

			repository.RemoveItem<BasicClass>(x => x.Id == baseClass.Id);

			var result = repository.GetAllItems<BasicClass>();
			Is.EqualTo(result.Count == 0);


		}


		[Test]
		public void CanRemoveMultiple()
		{
			List<BasicClass> basicClassList = new List<BasicClass>()
			{
				new BasicClass { TestField = "Test1", Refnr = 1 },
				new BasicClass { TestField = "Test2", Refnr = 2 },
				new BasicClass { TestField = "Test3", Refnr = 3 }
			};


			using var scope = _serviceProvider.CreateScope();
			var repository = scope.ServiceProvider.GetRequiredService<IRepository<FakeContext>>();

			repository.AddItems(basicClassList);

			That(repository.GetAllItems<BasicClass>().Count != 0);
			repository.RemoveItems<BasicClass>(basicClassList);

			var result = repository.GetAllItems<BasicClass>();
			That(result.Count == 0);
		}

		[Test]
		public void CanRemoveMultipleByExpression()
		{
			List<BasicClass> basicClassList = new List<BasicClass>()
			{
				new BasicClass { TestField = "Test1", Refnr = 1 },
				new BasicClass { TestField = "Test2", Refnr = 2 },
				new BasicClass { TestField = "Test3", Refnr = 3 }
			};


			using var scope = _serviceProvider.CreateScope();
			var repository = scope.ServiceProvider.GetRequiredService<IRepository<FakeContext>>();

			repository.AddItems(basicClassList);
			repository.RemoveItems<BasicClass>(x => x.Where(y => y.Refnr == 1));

			var result = repository.GetAllItems<BasicClass>();
			That(result.Count == 2);
		}

		[Test]
		public void CanGetByExpression()
		{
			//Arrange
			using var scope = _serviceProvider.CreateScope();
			var repository = scope.ServiceProvider.GetRequiredService<IRepository<FakeContext>>();
			var baseClass = new BasicClass { TestField = "Test1", Refnr = 1 };
			baseClass.BasicEntries.Add(new BasicEntry() { BasicClassId = baseClass.Id, ValueToLoad = "Test1" });

			// Act
			repository.AddItem(baseClass);

			//Assert
			var result = repository.GetItem<BasicClass>(q => q.Where(x => x.Id == baseClass.Id));
			That(result?.TestField, Is.EqualTo("Test1"));

		}


		[Test]
		public void CanGetByExpressionWithCollection()
		{

			//Create instance that is disposed of at end of function
			//Also makes use of a specific dependency injection methodology
			using var scope = _serviceProvider.CreateScope();
			//use reference to create an in memory database through interface and generics
			var repository = scope.ServiceProvider.GetRequiredService<IRepository<FakeContext>>();
			//Create instance of class used for testing
			var baseClass = new BasicClass { TestField = "Test1", Refnr = 1, BasicEntries = new List<BasicEntry>() { new BasicEntry() { ValueToLoad = "Test1" } } };

			//Add item to database
			repository.AddItem(baseClass);

			//Get item from database with Func expressions implementing Queryable interface
			var result = repository.GetItem<BasicClass>(q => q.Where(x => x.Id == baseClass.Id).Include(x => x.BasicEntries));
			//Assert that the returned instance is the same as what we added
			That(result.TestField, Is.EqualTo("Test1"));
			That(result.BasicEntries.First().Id, Is.EqualTo(baseClass.BasicEntries.First().Id));


		}


		[Test]
		public void CanGetAllWithoutExpression()
		{

			var baseClass = new BasicClass { TestField = "Test1", Refnr = 1 };
			var baseClass2 = new BasicClass { TestField = "Test2", Refnr = 2 };
			baseClass.BasicEntries.Add(new BasicEntry() { BasicClassId = baseClass.Id, ValueToLoad = "Test1" });
			baseClass.BasicEntries.Add(new BasicEntry() { BasicClassId = baseClass2.Id, ValueToLoad = "Test2" });

			using var scope = _serviceProvider.CreateScope();
			var repository = scope.ServiceProvider.GetRequiredService<IRepository<FakeContext>>();

			repository.AddItem(baseClass);
			repository.AddItem(baseClass2);

			var result = repository.GetAllItems<BasicClass>();
			That(result.Count == 2);

		}

		[Test]
		public void CanGetAllWithExpression()
		{

			var baseClass = new BasicClass { TestField = "Test1", Refnr = 1 };
			var baseClass2 = new BasicClass { TestField = "Test2", Refnr = 2 };
			baseClass.BasicEntries.Add(new BasicEntry() { BasicClassId = baseClass.Id, ValueToLoad = "Test1" });
			baseClass.BasicEntries.Add(new BasicEntry() { BasicClassId = baseClass2.Id, ValueToLoad = "Test2" });

			using var scope = _serviceProvider.CreateScope();
			var repository = scope.ServiceProvider.GetRequiredService<IRepository<FakeContext>>();

			repository.AddItem(baseClass);
			repository.AddItem(baseClass2);

			var result = repository.GetAllItems<BasicClass>(q => q.Where(x => x.Id == baseClass.Id));
			That(result.Count == 1);


		}

		[Test]
		public void CanGetAllForColumn()
		{
			List<BasicClass> basicClassList = new List<BasicClass>()
			{
				new BasicClass { TestField = "Test1", Refnr = 1 },
				new BasicClass { TestField = "Test2", Refnr = 2 },
				new BasicClass { TestField = "Test3", Refnr = 3 }
			};


			using var scope = _serviceProvider.CreateScope();
			var repository = scope.ServiceProvider.GetRequiredService<IRepository<FakeContext>>();

			repository.AddItems(basicClassList);

			List<string> result = repository.GetAllItems<BasicClass, string>(x => x.Select(s => s.TestField)!);
			List<int> test2Result = repository.GetAllItems<BasicClass, int>(x => x.Select(s => s.Refnr));
			That(result.Count == 3);
			That(test2Result.Count == 3 && test2Result.First() == 1);
		}


		[Test]
		public void CanUpdateItem()
		{
			List<BasicClass> basicClassList = new List<BasicClass>()
			{
				new BasicClass { TestField = "Test1", Refnr = 1 },
				new BasicClass { TestField = "Test2", Refnr = 2 },
				new BasicClass { TestField = "Test3", Refnr = 3 }
			};
			var bclass = basicClassList.First();
			bclass.BasicEntries.Add(new BasicEntry() { ValueToLoad = "something" });


			using var scope = _serviceProvider.CreateScope();
			var repository = scope.ServiceProvider.GetRequiredService<IRepository<FakeContext>>();

			repository.AddItems(basicClassList);

			var itemToEdit = repository.GetItem<BasicClass>(x => x.Where(q => q.Id == basicClassList.First().Id).Include(b => b.BasicEntries));
			itemToEdit.BasicEntries.First().ValueToLoad = "Edited";
			repository.UpdateItem(itemToEdit);

			var result = repository.GetItem<BasicClass>(x => x.Where(q => q.Id == basicClassList.First().Id).Include(b => b.BasicEntries));
			That(result.BasicEntries.First().ValueToLoad == "Edited");
		}


		[Test]
		public void CanUpdateMultipleItems()
		{
			List<BasicClass> basicClassList = new List<BasicClass>()
			{
				new BasicClass { TestField = "Test1", Refnr = 1 },
				new BasicClass { TestField = "Test2", Refnr = 2 },
				new BasicClass { TestField = "Test3", Refnr = 3 }
			};

			basicClassList.First().BasicEntries.Add(new BasicEntry() { ValueToLoad = "something" });
			basicClassList.Last().BasicEntries.Add(new BasicEntry() { ValueToLoad = "somethingElse" });
			using var scope = _serviceProvider.CreateScope();
			var repository = scope.ServiceProvider.GetRequiredService<IRepository<FakeContext>>();

			repository.AddItems(basicClassList);

			var items = repository.GetAllItems<BasicClass>(i => i.Include(b => b.BasicEntries));
			foreach (var item in items)
			{
				item.TestField = "Edited";
			}
			items.First().BasicEntries.First().ValueToLoad = "no";
			items.Last().BasicEntries.First().ValueToLoad = "nope";
			repository.UpdateItems(items);

			var result = repository.GetAllItems<BasicClass>(i => i.Include(b => b.BasicEntries));
			That(result.TrueForAll(x => x.TestField == "Edited"));
			That(result.First().BasicEntries.First().ValueToLoad == "no");
			That(result.Last().BasicEntries.First().ValueToLoad == "nope");
		}
	}
}
