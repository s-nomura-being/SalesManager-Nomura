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
    public class DetailSalesViewFileManagerTests
    {
        private Mock<IFileProcessor> _mockFileProcessor;
        private DetailSalesViewFileManager _target;

        [SetUp]
        public void SetUp()
        {
            _mockFileProcessor = new Mock<IFileProcessor>();
            _target = new DetailSalesViewFileManager(_mockFileProcessor.Object);
        }

        [Test]
        public void TS041_ReadFile_読込成功時にtrueとデータが返ること()
        {
            // Arrange
            string dummyPath = "dummy_details.csv";
            var expectedData = new List<DetailSalesData>
            {
                new DetailSalesData { ProductName = "テスト商品", SalesCount = 10, Sales = 1000 }
            };

            _mockFileProcessor
                .Setup(m => m.ReadFile(dummyPath, out expectedData))
                .Returns(true);

            // Act
            bool result = _target.ReadFile(dummyPath, out List<DetailSalesData> actualData);

            // Assert
            Assert.That(result, Is.True);
            Assert.That(actualData.Count, Is.EqualTo(1));
            Assert.That(actualData[0].ProductName, Is.EqualTo("テスト商品"));
        }

        [Test]
        public void TS042_SaveFile_パス指定なしの場合_生成されたパスで保存処理が呼ばれること()
        {
            // Arrange
            var inputData = new List<DetailSalesData>
            {
                new DetailSalesData { ProductName = "テスト商品", SalesCount = 5, Sales = 500 }
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
        public void TS043_SaveFile_パス指定ありの場合_指定したパスで保存処理が呼ばれること()
        {
            // Arrange
            var inputData = new List<DetailSalesData>
            {
                new DetailSalesData { ProductName = "テスト商品", SalesCount = 2, Sales = 200 }
            };
            string customPath = @"C:\Export\detail_sales.csv";

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
        public void TS044_ReadFile_例外発生時に再スローされること()
        {
            // Arrange
            string dummyPath = "error.csv";
            List<DetailSalesData> dummyData;

            _mockFileProcessor
                .Setup(m => m.ReadFile(dummyPath, out dummyData))
                .Throws(new InvalidOperationException("File Read Error"));

            // Act & Assert
            var ex = Assert.Throws<InvalidOperationException>(() => _target.ReadFile(dummyPath, out List<DetailSalesData> _));
            Assert.That(ex.Message, Is.EqualTo("File Read Error"));
        }
    }
}