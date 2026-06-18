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
    public class ProductMasterViewFileManagerTests
    {
        private Mock<IFileProcessor> _mockFileProcessor;
        private ProductMasterViewFileManager _target;

        [SetUp]
        public void SetUp()
        {
            _mockFileProcessor = new Mock<IFileProcessor>();
            _target = new ProductMasterViewFileManager(_mockFileProcessor.Object);
        }

        [Test]
        public void TS052_ReadFile_読込成功時にtrueとデータが返ること()
        {
            // Arrange
            string dummyPath = "dummy_product.csv";
            var expectedData = new List<ProductMasterData>
            {
                new ProductMasterData { ID = 1, ProductName = "テスト商品", UnitPrice = 1000, CategoryID = 5 }
            };

            _mockFileProcessor
                .Setup(m => m.ReadFile(dummyPath, out expectedData))
                .Returns(true);

            // Act
            bool result = _target.ReadFile(dummyPath, out List<ProductMasterData> actualData);

            // Assert
            Assert.That(result, Is.True);
            Assert.That(actualData.Count, Is.EqualTo(1));
            Assert.That(actualData[0].ProductName, Is.EqualTo("テスト商品"));
            Assert.That(actualData[0].UnitPrice, Is.EqualTo(1000));
        }

        [Test]
        public void TS053_ReadFile_例外発生時に再スローされること()
        {
            // Arrange
            string dummyPath = "error.csv";
            List<ProductMasterData> dummyData;

            _mockFileProcessor
                .Setup(m => m.ReadFile(dummyPath, out dummyData))
                .Throws(new InvalidOperationException("File Read Error"));

            // Act & Assert
            var ex = Assert.Throws<InvalidOperationException>(() => _target.ReadFile(dummyPath, out List<ProductMasterData> _));
            Assert.That(ex.Message, Is.EqualTo("File Read Error"));
        }

        [Test]
        public void TS054_SaveFile_生成されたパスで保存処理が呼ばれること()
        {
            // Arrange
            var inputData = new List<ProductMasterData>
            {
                new ProductMasterData { ID = 2, ProductName = "保存商品" }
            };

            _mockFileProcessor.SetupGet(m => m.Extension).Returns(".csv");

            // "master/商品マスタ.csv" のように結合されることを検証
            string expectedPath = Path.Combine(Application.StartupPath, "master/商品マスタ.csv");

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