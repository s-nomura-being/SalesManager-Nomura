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
    public class CategoryMasterViewFileManagerTests
    {
        private Mock<IFileProcessor> _mockFileProcessor;
        private CategoryMasterViewFileManager _target;

        [SetUp]
        public void SetUp()
        {
            _mockFileProcessor = new Mock<IFileProcessor>();
            _target = new CategoryMasterViewFileManager(_mockFileProcessor.Object);
        }

        [Test]
        public void TS055_ReadFile_読込成功時にtrueとデータが返ること()
        {
            // Arrange
            string dummyPath = "dummy_category.csv";
            var expectedData = new List<CategoryMasterData>
            {
                new CategoryMasterData { ID = 1, CategoryName = "テスト区分" } // Datas.cs の定義に従う
            };

            _mockFileProcessor
                .Setup(m => m.ReadFile(dummyPath, out expectedData))
                .Returns(true);

            // Act
            bool result = _target.ReadFile(dummyPath, out List<CategoryMasterData> actualData);

            // Assert
            Assert.That(result, Is.True);
            Assert.That(actualData.Count, Is.EqualTo(1));
            Assert.That(actualData[0].CategoryName, Is.EqualTo("テスト区分"));
        }

        [Test]
        public void TS056_ReadFile_例外発生時に再スローされること()
        {
            // Arrange
            string dummyPath = "error.csv";
            List<CategoryMasterData> dummyData;

            _mockFileProcessor
                .Setup(m => m.ReadFile(dummyPath, out dummyData))
                .Throws(new InvalidOperationException("File Read Error"));

            // Act & Assert
            var ex = Assert.Throws<InvalidOperationException>(() => _target.ReadFile(dummyPath, out List<CategoryMasterData> _));
            Assert.That(ex.Message, Is.EqualTo("File Read Error"));
        }

        [Test]
        public void TS057_SaveFile_生成されたパスで保存処理が呼ばれること()
        {
            // Arrange
            var inputData = new List<CategoryMasterData>
            {
                new CategoryMasterData { ID = 2, CategoryName = "保存区分" }
            };

            _mockFileProcessor.SetupGet(m => m.Extension).Returns(".csv");

            // "master/区分マスタ.csv" のように結合されることを検証
            string expectedPath = Path.Combine(Application.StartupPath, "master/区分マスタ.csv");

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