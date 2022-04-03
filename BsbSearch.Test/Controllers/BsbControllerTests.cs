using BsbSearch.Controllers;
using BsbSearch.Models;
using BsbSearch.Services;
using BsbSearch.Test.Builders;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

namespace BsbSearch.Test.Controllers
{
    public class BsbControllerTests
    {
        private Mock<IBsbService> _bsbService = new Mock<IBsbService>();
        private Mock<IRequestHistoryService> _requestHistory = new Mock<IRequestHistoryService>();

        public BsbControllerTests()
        {          
            _bsbService.Setup(bsb => bsb.GetAllBsbRecords())
                .ReturnsAsync(new BsbRecordBuilder().AllRecordsWithAllFields().Build());

            _bsbService.Setup(bsb => bsb.GetBsbRecord(It.IsAny<string>()))
                .ReturnsAsync(new BsbRecordBuilder().SingleRecordsWithAllFields().Build()[0]);
        }
        private BsbController CreateController(string teamName)
        {
            var httpContext = new DefaultHttpContext();
            httpContext.Request.Headers["team-name"] = teamName;
            return new BsbController(new Mock<ILogger<BsbController>>().Object, _bsbService.Object, _requestHistory.Object)
            {
                ControllerContext = new ControllerContext()
                {
                    HttpContext = httpContext
                }
            };
        }

        [Fact]
        public async Task Get_All_Bsb_Records_Returns_All_Bsb_Records()
        {
            var controller = CreateController("local");
            var allBsbRecords = await controller.Get();

            Assert.Equal(3, allBsbRecords?.Count);
        }

        [Fact]
        public async Task Get_All_Bsb_Records_Does_Not_Save_Request_History_When_Passed_team_Is_Local()
        {
            var controller = CreateController("local");
            var allBsbRecords = await controller.Get();

            Assert.Equal(3, allBsbRecords?.Count);
            _requestHistory.Verify(r => r.Add(It.IsAny<RequestHistory>()), Times.Never);
        }

        [Fact]
        public async Task Get_All_Bsb_Records_Saves_Request_History_When_Passed_team_Is_Not_Local()
        {
            var controller = CreateController("Not-local");
            var allBsbRecords = await controller.Get();

            Assert.Equal(3, allBsbRecords?.Count);

            _requestHistory.Verify(r => r.Add(It.IsAny<RequestHistory>()), Times.Once);
        }

        [Fact]
        public async Task Get_All_Bsb_Records_Saves_Request_History_When_GetAllBsbRecords_Fails()
        {
            _bsbService.Setup(bsb => bsb.GetAllBsbRecords()).Throws(new Exception());

            var controller = CreateController("Not-local");
            
            await Assert.ThrowsAsync<Exception>(() => controller.Get());

            _requestHistory.Verify(r => r.Add(It.IsAny<RequestHistory>()), Times.Once);
        }

        [Fact]
        public async Task Get_A_Bsb_Record_Returns_All_Bsb_Records()
        {
            var controller = CreateController("local");
            var bsbRecord = await controller.Get("012002");

            Assert.NotNull(bsbRecord);
            Assert.Equal("012002", bsbRecord?.Number);
        }

        [Fact]
        public async Task Get_A_Bsb_Record_Does_Not_Save_Request_History_When_Passed_team_Is_Local()
        {
            var controller = CreateController("local");
            var bsbRecord = await controller.Get("012002");

            Assert.NotNull(bsbRecord);
            Assert.Equal("012002", bsbRecord?.Number);
            _requestHistory.Verify(r => r.Add(It.IsAny<RequestHistory>()), Times.Never);
        }

        [Fact]
        public async Task Get_A_Bsb_Record_Saves_Request_History_When_Passed_team_Is_Not_Local()
        {
            var controller = CreateController("Not-local");
            var bsbRecord = await controller.Get("012002");

            Assert.NotNull(bsbRecord);
            Assert.Equal("012002", bsbRecord?.Number);
            _requestHistory.Verify(r => r.Add(It.IsAny<RequestHistory>()), Times.Once);
        }

        [Fact]
        public async Task Get_A_Bsb_Record_Saves_Request_History_When_GetAllBsbRecords_Fails()
        {
            _bsbService.Setup(bsb => bsb.GetBsbRecord(It.IsAny<string>())).Throws(new Exception());

            var controller = CreateController("Not-local");

            await Assert.ThrowsAsync<Exception>(() => controller.Get("012002"));

            _requestHistory.Verify(r => r.Add(It.IsAny<RequestHistory>()), Times.Once);
        }
    }
}
