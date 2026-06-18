using NUnit.Framework;
using Moq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using SalesManagerApp.FileIO;
using SalesManagerApp.Data;
using FileControlLib;

namespace SalesManagerApp.Test.UnitTest.FileManager
{
    [TestFixture]
    public class StoreMasterViewFileManagerTests
    {
        private Mock<IFileProcessor> _mockFileProcessor;
        private StoreMasterViewFileManager _target;

        [SetUp]
        public void SetUp()
        {
            _mockFileProcessor = new Mock<IFileProcessor>();
            _target = new StoreMasterViewFileManager(_mockFileProcessor.Object);
        }

        [Test]
        public void TS049_ReadFile_読込成功時にtrueとデータが返ること()
        {
            // Arrange
            string dummyPath = "dummy_store.csv";
            var expectedData = new List<StoreMasterData>
            {
                new StoreMasterData { ID = 1, StoreName = "テスト店舗" }
            };

            _mockFileProcessor
                .Setup(m => m.ReadFile(dummyPath, out expectedData))
                .Returns(true);

            // Act
            bool result = _target.ReadFile(dummyPath, out List<StoreMasterData> actualData);

            // Assert
            Assert.That(result, Is.True);
            Assert.That(actualData.Count, Is.EqualTo(1));
            Assert.That(actualData[0].StoreName, Is.EqualTo("テスト店舗"));
        }

        [Test]
        public void TS050_ReadFile_例外発生時に再スローされること()
        {
            // Arrange
            string dummyPath = "error.csv";
            List<StoreMasterData> dummyData;

            _mockFileProcessor
                .Setup(m => m.ReadFile(dummyPath, out dummyData))
                .Throws(new InvalidOperationException("File Read Error"));

            // Act & Assert
            var ex = Assert.Throws<InvalidOperationException>(() => _target.ReadFile(dummyPath, out List<StoreMasterData> _));
            Assert.That(ex.Message, Is.EqualTo("File Read Error"));
        }

        [Test]
        public void TS051_SaveFile_生成されたパスで保存処理が呼ばれること()
        {
            // Arrange
            var inputData = new List<StoreMasterData>
            {
                new StoreMasterData { ID = 2, StoreName = "保存店舗" }
            };

            _mockFileProcessor.SetupGet(m => m.Extension).Returns(".csv");

            // "master/店舗マスタ.csv" のように結合されることを検証
            string expectedPath = Path.Combine(Application.StartupPath, "master/店舗マスタ.csv");

            _mockFileProcessor
                .Setup(m => m.SaveFile(expectedPath, inputData))
                .Returns(true);

            // Act
            bool result = _target.SaveFile(inputData);

            // Assert
            Assert.That(result, Is.True);
            _mockFileProcessor.Verify(m => m.SaveFile(expectedPath, inputData), Times.Once);
        }
    }
}