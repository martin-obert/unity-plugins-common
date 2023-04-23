using System;
using System.Linq;
using NUnit.Framework;
using Obert.Common.Runtime.Repositories;
using Obert.Common.Runtime.Repositories.Components;

namespace Tests.EditorMode
{
    public class RepositoryData
    {
        public override bool Equals(object obj)
        {
            if (obj is RepositoryData repositoryData) return Equals(repositoryData);
            return ReferenceEquals(this, obj);
        }

        protected bool Equals(RepositoryData other)
        {
            return Id == other.Id;
        }

        public override int GetHashCode()
        {
            return Id;
        }

        public RepositoryData(int id)
        {
            Id = id;
        }

        public int Id { get; }
        public string Value { get; set; }
    }

    public class InMemoryRepositoryTests
    {
        private const int DataCount = 10;

        private RepositoryData[] repositoryDatas;

        [SetUp]
        public void SetupTest()
        {
            repositoryDatas =
                Enumerable.Range(0, DataCount).Select(x => new RepositoryData(x)).ToArray();
        }

        [TearDown]
        public void TearDownTest()
        {
            repositoryDatas = null;
        }

        [Test]
        public void InMemory_Repository_Single()
        {
            IRepository<RepositoryData> repository = new InMemoryRepository<RepositoryData>(repositoryDatas);
            Assert.AreEqual(DataCount, repository.Many().Count());
            Assert.AreEqual(repositoryDatas.First(), repository.FirstOrDefault());
            Assert.AreEqual(repositoryDatas.ElementAt(DataCount / 2),
                repository.FirstOrDefault(x => x == repositoryDatas.ElementAt(DataCount / 2)));
        }

        [Test]
        public void InMemory_Repository_Many()
        {
            IRepository<RepositoryData> repository = new InMemoryRepository<RepositoryData>(repositoryDatas);
            Assert.AreEqual(DataCount, repository.Many().Count());
            var datas = repositoryDatas.Skip(2).Take(3).ToArray();

            foreach (var repositoryData in repository.Many(x => datas.Contains(x)))
            {
                Assert.IsTrue(datas.Contains(repositoryData));
            }
        }

        [Test]
        public void InMemory_Repository_Add()
        {
            IRepository<RepositoryData> repository = new InMemoryRepository<RepositoryData>(repositoryDatas);

            var id = repository.FirstOrDefault().Id;

            repository.AddSingle(new RepositoryData(id), true);

            var count = repository.Count();
            Assert.IsNotNull(repository.FirstOrDefault(x => x.Id == id));
            Assert.Greater(count, repositoryDatas.Length);

            repository.AddSingle(new RepositoryData(id));

            Assert.IsNotNull(repository.FirstOrDefault(x => x.Id == id));
            Assert.AreEqual(count, repository.Count());
        }

        [Test]
        public void InMemory_Repository_Remove()
        {
            IRepository<RepositoryData> repository = new InMemoryRepository<RepositoryData>(repositoryDatas);

            var id = repository.FirstOrDefault().Id;

            repository.DeleteSingle(new RepositoryData(id));

            Assert.IsNull(repository.FirstOrDefault(x => x.Id == id));
            var count = repository.Count();
            Assert.Less(count, repositoryDatas.Length);

            repository.DeleteSingle(new RepositoryData(id));

            Assert.IsNull(repository.FirstOrDefault(x => x.Id == id));
            Assert.AreEqual(count, repository.Count());
        }

        [Test]
        public void InMemory_Repository_ClearAll()
        {
            IRepository<RepositoryData> repository = new InMemoryRepository<RepositoryData>(repositoryDatas);

            repository.ClearAll();
            Assert.IsEmpty(repository.Many());
        }

        [Test]
        public void InMemory_Repository_UpdateSingle()
        {
            IRepository<RepositoryData> repository = new InMemoryRepository<RepositoryData>(repositoryDatas);
            var guid = Guid.NewGuid().ToString();
            var id = repository.FirstOrDefault().Id;
            var item = repository.FirstOrDefault(x => x.Id == id);
            Assert.IsNotNull(item);
            Assert.IsNull(item.Value);
            repository.UpdateSingle(new RepositoryData(id) { Value = guid });
            item = repository.FirstOrDefault(x => x.Id == id);
            Assert.IsNotNull(item);
            Assert.IsNotNull(item.Value);
            Assert.IsNotEmpty(item.Value);
            Assert.AreEqual(guid, item.Value);
        }
    }
}