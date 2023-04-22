using System.Linq;
using Moq;
using Newtonsoft.Json;
using NUnit.Framework;
using Obert.Common.Runtime.Repositories;

namespace Tests
{
    public class JsonRepositoryTests
    {
        private const int DataCount = 10;

        private RepositoryData[] repositoryDatas;

        [SetUp]
        public void SetupTest()
        {
            repositoryDatas =
                Enumerable.Range(0, DataCount).Select(x => new RepositoryData(x)).ToArray();
        }

        [Test]
        public void Json_Repository_Single()
        {
            var fileProviderMock = new Mock<IFileProvider>();
            fileProviderMock
                .Setup(x => x.ReadAllText())
                .Returns(() => JsonConvert.SerializeObject(repositoryDatas));

            fileProviderMock.Setup(x => x.WriteAllText(It.IsAny<string>()));
            
            var repository =
                new JsonDataRepository<RepositoryData>(fileProviderMock.Object);
            
            Assert.AreEqual(DataCount, repository.Many().Count());

            var datas = repositoryDatas.Skip(2).Take(3).ToArray();

            var array = repository.Many(x => datas.Contains(x)).ToArray();
            
            foreach (var repositoryData in array)
            {
                Assert.IsTrue(datas.Contains(repositoryData));
            }
            repository.Save();
            
            fileProviderMock.Verify(x=>x.ReadAllText(), Times.Once);
            fileProviderMock.Verify(x=>x.WriteAllText(It.Is<string>(v => v.Equals(JsonConvert.SerializeObject(repositoryDatas)))));
        }
    }
}