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
    public class InventoryInfoViewFileManagerTests
    {
        private Mock<IFileProcessor> _mockFileProcessor;
        private InventoryInfoViewFileManager _target;

        [SetUp]
        public void SetUp()
        {
            _mockFileProcessor = new Mock<IFileProcessor>();
            _target = new InventoryInfoViewFileManager(_mockFileProcessor.Object);
        }

        [Test]
        public void TS045_ReadFile_読込成功時にtrueとデータが返ること()
        {
            // Arrange
            string dummyPath = "dummy_inventory_list.csv";
            var expectedData = new List<InventoryListData>
            {
                // Datas.cs に定義されたプロパティを使用
                new InventoryListData { ProductName = "テスト商品", Stock = 50, UnitPrice = 100 }
            };

            _mockFileProcessor
                .Setup(m => m.ReadFile(dummyPath, out expectedData))
                .Returns(true);

            // Act
            bool result = _target.ReadFile(dummyPath, out List<InventoryListData> actualData);

            // Assert
            Assert.That(result, Is.True);
            Assert.That(actualData.Count, Is.EqualTo(1));
            Assert.That(actualData[0].ProductName, Is.EqualTo("テスト商品"));
            Assert.That(actualData[0].Stock, Is.EqualTo(50));
        }

        [Test]
        public void TS046_SaveFile_パス指定なしの場合_生成されたパスで保存処理が呼ばれること()
        {
            // Arrange
            var inputData = new List<InventoryListData>
            {
                new InventoryListData { ProductName = "テスト商品", Stock = 20 }
            };

            _mockFileProcessor.SetupGet(m => m.Extension).Returns(".csv");
            string expectedPath = Path.Combine(Application.StartupPath, "data.csv");

            _mockFileProcessor
                .Setup(m => m.SaveFile(expectedPath, inputData))
                .Returns(true);

            // Act
            bool result = _target.SaveFile(inputData);

            // Assert
            Assert.That(result, Is.True);
            _mockFileProcessor.Verify(m => m.SaveFile(expectedPath, inputData), Times.Once);
        }

        [Test]
        public void TS047_SaveFile_パス指定ありの場合_指定したパスで保存処理が呼ばれること()
        {
            // Arrange
            var inputData = new List<InventoryListData>
            {
                new InventoryListData { ProductName = "テスト商品", Stock = 10 }
            };
            string customPath = @"C:\Export\inventory_list.csv";

            _mockFileProcessor
                .Setup(m => m.SaveFile(customPath, inputData))
                .Returns(true);

            // Act
            bool result = _target.SaveFile(customPath, inputData);

            // Assert
            Assert.That(result, Is.True);
            _mockFileProcessor.Verify(m => m.SaveFile(customPath, inputData), Times.Once);
        }

        [Test]
        public void TS048_ReadFile_例外発生時に再スローされること()
        {
            // Arrange
            string dummyPath = "error.csv";
            List<InventoryListData> dummyData;

            _mockFileProcessor
                .Setup(m => m.ReadFile(dummyPath, out dummyData))
                .Throws(new InvalidOperationException("File Read Error"));

            // Act & Assert
            var ex = Assert.Throws<InvalidOperationException>(() => _target.ReadFile(dummyPath, out List<InventoryListData> _));
            Assert.That(ex.Message, Is.EqualTo("File Read Error"));
        }
    }
}