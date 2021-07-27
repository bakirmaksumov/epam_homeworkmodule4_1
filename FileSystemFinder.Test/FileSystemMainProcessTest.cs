using NUnit.Framework;
using Moq;
using System.IO;
using FileSystemFinder;
using System;


namespace FileSystemFinder.Test
{
    [TestFixture]
    public class FileSystemMainProcessTest
    {
        private IFileSystemMainProcess mainProcess;
        private Mock<FileSystemInfo> fileSystemMock;
        [SetUp]
        public void TestInitailProcess()
        {
            mainProcess = new FileSystemMainProcess();
            fileSystemMock = new Mock<FileSystemInfo>();
        }

        [Test]
        public void FoundCallItem()
        {
            //arrange
            var fileSystemItem = fileSystemMock.Object;
            int delegateCallCountResult = 0;
            int expectedResult = 1;
            //act
            mainProcess.ProcessItemFinded(fileSystemItem, null, (a, b) => delegateCallCountResult++, null, OnEvent);
            //assert
            Assert.AreEqual(expectedResult, delegateCallCountResult);
        }

        [Test]
        public void FilteredFoundCallItem()
        {
            //arrange
            var fileSystemItem = fileSystemMock.Object;
            int delegateCallCountResult = 0;
            int expectedResult = 2;
            //act
            mainProcess.ProcessItemFinded(fileSystemItem, info=>true, (a, b) => delegateCallCountResult++, (a, b) => delegateCallCountResult++, OnEvent);
            //assert
            Assert.AreEqual(expectedResult, delegateCallCountResult);
        }

        [Test]
        public void NotPassFilterItem()
        {
            //arrange
            var fileSystemItem = fileSystemMock.Object;
            int delegateCallCountResult = 0;
            int expectedResult = 1;
            //act
            mainProcess.ProcessItemFinded(fileSystemItem, info => false, (a, b) => delegateCallCountResult++, (a, b) => delegateCallCountResult++, OnEvent);
            //assert
            Assert.AreEqual(expectedResult, delegateCallCountResult);

        }

        [Test]
        public void ItemFoundContinueActionSearch()
        {
            //arrange
            var fileSystemItem = fileSystemMock.Object;
            //act
            var act=mainProcess.ProcessItemFinded(fileSystemItem, null, (a, b) => { },null, OnEvent);
            //assert
            Assert.AreEqual(StatusEnum.ContinueSearch,act);

        }

        [Test]
        public void FoundedItemElementAction()
        {
            //arrange
            var fileSystemItem = fileSystemMock.Object;
            //act
            var act = mainProcess.ProcessItemFinded(fileSystemItem, info=>true, (a, b) => { }, (a, b) => { }, OnEvent);
            //assert
            Assert.AreEqual(StatusEnum.ContinueSearch, act);

        }


        [Test]
        public void FoundedItemSkipElementAction()
        {
            //arrange
            var fileSystemItem = fileSystemMock.Object;
            int delegateCallCountResult = 0;
            int expectedResult = 1;
            //act
            var act = mainProcess.ProcessItemFinded(fileSystemItem, info => true, (a, b) => {

                delegateCallCountResult++;
                b.ActionType = StatusEnum.SkipElement;
            }, (a, b) => delegateCallCountResult++, OnEvent);
            //assert

            Assert.AreEqual(StatusEnum.SkipElement, act);
            Assert.AreEqual(expectedResult, delegateCallCountResult);
        }

        [Test]
        public void FilteredFoundedItemSkipElementAction()
        {
            //arrange
            var fileSystemItem = fileSystemMock.Object;
            int delegateCallCountResult = 0;
            int expectedResult = 2;
            //act
            var act = mainProcess.ProcessItemFinded(fileSystemItem, info => true, (a, b) => delegateCallCountResult++, (a, b) => {

                delegateCallCountResult++;
                b.ActionType = StatusEnum.SkipElement;
            }, OnEvent);
            //assert

            Assert.AreEqual(StatusEnum.SkipElement, act);
            Assert.AreEqual(expectedResult, delegateCallCountResult);
        }
        [Test]
        public void FoundedItemsStopElementAction()
        {
            //arrange
            var fileSystemItem = fileSystemMock.Object;
            int delegateCallCountResult = 0;
            int expectedResult = 1;
            //act
            var act = mainProcess.ProcessItemFinded(fileSystemItem, info => true, (a, b) => {

                delegateCallCountResult++;
                b.ActionType = StatusEnum.StopSearch;
            }, (a, b) => delegateCallCountResult++, OnEvent);
            //assert

            Assert.AreEqual(StatusEnum.StopSearch, act);
            Assert.AreEqual(expectedResult, delegateCallCountResult);
        }

        [Test]
        public void FilteredFoundedItemStopSeachElementAction()
        {
            //arrange
            var fileSystemItem = fileSystemMock.Object;
            int delegateCallCountResult = 0;
            int expectedResult = 2;
            //act
            var act = mainProcess.ProcessItemFinded(fileSystemItem, info => true, (a, b) => delegateCallCountResult++, (a, b) => {

                delegateCallCountResult++;
                b.ActionType = StatusEnum.StopSearch;
            }, OnEvent);
            //assert

            Assert.AreEqual(StatusEnum.StopSearch, act);
            Assert.AreEqual(expectedResult, delegateCallCountResult);
        }
        private void OnEvent<TArgs>(EventHandler<TArgs> someEvent, TArgs args)
        {
            someEvent?.Invoke(this, args);
        }
    }
}