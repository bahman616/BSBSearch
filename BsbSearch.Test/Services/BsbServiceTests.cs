using BsbSearch.Services;
using BsbSearch.Test.Builders;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using Xunit;

namespace BsbSearch.Test.Services
{
    public class BsbServiceTests
    {
        private BsbService service;
        private Mock<IFileService> fileService = new Mock<IFileService>();

        public BsbServiceTests() => 
            this.service = new BsbService(new Mock<ILogger<BsbService>>().Object, fileService.Object);

        [Fact]
        public void GetBsbRecord_Should_Return_ArgumentNullException_When_Bsb_IsNull()
        {
            Assert.Throws<ArgumentNullException>(() => service.GetBsbRecord(string.Empty));
        }

        [Fact]
        public void GetBsbRecord_Should_Return_ArgumentOutOfRangeException_When_Bsb_Not_Six_Characters()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => service.GetBsbRecord("123."));
        }

        [Fact]
        public void GetBsbRecord_Should_Filter_Bsbs()
        {
            fileService.Setup(
                f => f.GetAllBsbRecords()).Returns(new BsbRecordBuilder().WithAllFields().Build());

            var result = service.GetBsbRecord("985555");

            Assert.NotNull(result);
            Assert.Equal("985555", result?.Number);
            Assert.Equal("Hongkong & Shanghai Banking Aust", result?.Name);
        }

        [Fact]
        public void GetBsbRecord_Should_Return_Empty_When_Bsb_Not_Found()
        {
            fileService.Setup(
                f => f.GetAllBsbRecords()).Returns(new BsbRecordBuilder().WithAllFields().Build());

            var result = service.GetBsbRecord("Wrongd");

            Assert.Null(result);
        }
    }
}