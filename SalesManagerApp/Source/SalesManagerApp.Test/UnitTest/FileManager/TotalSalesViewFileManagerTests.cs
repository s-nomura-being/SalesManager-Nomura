using NUnit.Framework;
using Moq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms; // Application.StartupPath のため必要
using SalesManagerApp.FileIO;
using SalesManagerApp.Data;
using FileControlLib;

namespace SalesManagerApp.Test.UnitTest.FileManager
{
    [TestFixture]
    public class TotalSalesViewFileManagerTests
    {
        private Mock<IFileProcessor> _mockFileProcessor;
        private TotalSalesViewFileManager _target;

        [SetUp]
        public void SetUp()
        {
            _mockFileProcessor = new Mock<IFileProcessor>();
            _target = new TotalSalesViewFileManager(_mockFileProcessor.Object);
        }

        [Test]
        public void TS037_ReadFile_読込成功時にtrueとデータが返ること()
        {
            // Arrange
            string dummyPath = "dummy_totalsales.csv";
            var expectedData = new List<TotalSalesData>
            {
                new TotalSalesData { Period = "2026年6月", TotalSales = 10000 }
            };

            _mockFileProcessor
                .Setup(m => m.ReadFile(dummyPath, out expectedData))
                .Returns(true);

            // Act
            bool result = _target.ReadFile(dummyPath, out List<TotalSalesData> actualData);

            // Assert
            Assert.That(result, Is.True);
            Assert.That(actualData.Count, Is.EqualTo(1));
            Assert.That(actualData[0].Period, Is.EqualTo("2026年6月"));
            Assert.That(actualData[0].TotalSales, Is.EqualTo(10000));
        }

        [Test]
        public void TS038_SaveFile_パス指定なしの場合_生成されたパスで保存処理が呼ばれること()
        {
            // Arrange
            var inputData = new List<TotalSalesData>
            {
                new TotalSalesData { Period = "2026年", TotalSales = 50000 }
            };

            // 拡張子のモック設定
            _mockFileProcessor.SetupGet(m => m.Extension).Returns(".csv");

            // 期待されるパスの算出
            string expectedPath = Path.Combine(Application.StartupPath, "data.csv");

            _mockFileProcessor
                .Setup(m => m.SaveFile(expectedPath, inputData))
                .Returns(true);

            // Act (パス指定なしのオーバーロードを呼び出し)
            bool result = _target.SaveFile(inputData);

            // Assert
            Assert.That(result, Is.True);
            _mockFileProcessor.Verify(m => m.SaveFile(expectedPath, inputData), Times.Once);
        }

        [Test]
        public void TS039_SaveFile_パス指定ありの場合_指定したパスで保存処理が呼ばれること()
        {
            // Arrange
            var inputData = new List<TotalSalesData>
            {
                new TotalSalesData { Period = "2026年第1週", TotalSales = 1500 }
            };
            string customPath = @"C:\Temp\custom_sales.csv";

            _mockFileProcessor
                .Setup(m => m.SaveFile(customPath, inputData))
                .Returns(true);

            // Act (パス指定ありのオーバーロードを呼び出し)
            bool result = _target.SaveFile(customPath, inputData);

            // Assert
            Assert.That(result, Is.True);
            _mockFileProcessor.Verify(m => m.SaveFile(customPath, inputData), Times.Once);
        }

        [Test]
        public void TS040_ReadFile_例外発生時に再スローされること()
        {
            // Arrange
            string dummyPath = "error.csv";
            List<TotalSalesData> dummyData;

            _mockFileProcessor
                .Setup(m => m.ReadFile(dummyPath, out dummyData))
                .Throws(new UnauthorizedAccessException("Access Denied"));

            // Act & Assert
            var ex = Assert.Throws<UnauthorizedAccessException>(() => _target.ReadFile(dummyPath, out List<TotalSalesData> _));
            Assert.That(ex.Message, Is.EqualTo("Access Denied"));
        }
    }
}