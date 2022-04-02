using BsbSearch.Models;
using BsbSearch.Services;
using BsbSearch.Test.Builders;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace BsbSearch.Test.Services
{
    public class PartnerServiceTests
    {
        private PartnerService service;
        private Mock<IFileService> fileService = new Mock<IFileService>();

        public PartnerServiceTests() => 
            this.service = new PartnerService(new Mock<ILogger<PartnerService>>().Object, fileService.Object);

        [Fact]
        public async Task IsKeyValid_Should_Return_False_When_no_Partner()
        {
            fileService.Setup(f => f.GetAllPartners()).Returns(Task.FromResult<List<Partner>?>(null));
            
            var result = await service.IsKeyValid("", "");

            Assert.False(result);
        }

        [Fact]
        public async Task IsKeyValid_Should_Return_False_When_Key_Is_Wrong()
        {
            fileService.Setup(f => f.GetAllPartners())
                .Returns(Task.FromResult<List<Partner>?>(new PartnerBuilder().AllPartners().Build()));

            var result = await service.IsKeyValid("122", "4555");

            Assert.False(result);
        }

        [Fact]
        public async Task IsKeyValid_Should_Return_True_When_Key_Is_Correct()
        {
            fileService.Setup(f => f.GetAllPartners())
                .Returns(Task.FromResult<List<Partner>?>(new PartnerBuilder().AllPartners().Build()));

            var result = await service.IsKeyValid("Team2", "Key2");

            Assert.True(result);
        }
    }
}