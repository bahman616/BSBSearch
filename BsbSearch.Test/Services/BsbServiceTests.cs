using BsbSearch.Models;
using BsbSearch.Services;
using BsbSearch.Test.Builders;
using Microsoft.Extensions.Logging;
using Moq;
using Moq.Protected;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace BsbSearch.Test.Services
{
    public class BsbServiceTests
    {
        private BsbService service;
        private Mock<IFileService> fileService = new Mock<IFileService>();
        private Mock<IHttpClientFactory> _clientFactory = new Mock<IHttpClientFactory>();
        private Mock<IPartnerService> _partnerService = new Mock<IPartnerService>();

        public BsbServiceTests() => 
            this.service = new BsbService(new Mock<ILogger<BsbService>>().Object, fileService.Object, _partnerService.Object, _clientFactory.Object);

        [Fact]
        public async Task GetBsbRecord_Should_Return_ArgumentNullException_When_Bsb_IsNull()
        {
            await Assert.ThrowsAsync<ArgumentNullException>(() => service.GetBsbRecord(string.Empty));
        }

        [Fact]
        public async Task GetBsbRecord_Should_Return_ArgumentOutOfRangeException_When_Bsb_Not_Six_Characters()
        {
            await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => service.GetBsbRecord("123."));
        }

        [Fact]
        public async Task GetBsbRecord_Should_Filter_Bsbs()
        {
            fileService
                .Setup(f => f.GetAllBsbRecords())
                .Returns(Task.FromResult<List<BsbRecord>?>(
                    new BsbRecordBuilder().AllRecordsWithAllFields().Build()));

            var result = await service.GetBsbRecord("985555");

            Assert.NotNull(result);
            Assert.Equal("985555", result?.Number);
            Assert.Equal("Hongkong & Shanghai Banking Aust", result?.Name);
        }

        [Fact]
        public async Task GetBsbRecord_Should_Return_Empty_When_Bsb_Not_Found()
        {
            fileService.Setup(f => f.GetAllBsbRecords())
                .Returns(Task.FromResult<List<BsbRecord>?>(
                    new BsbRecordBuilder().AllRecordsWithAllFields().Build()));

            var result = await service.GetBsbRecord("Wrongd");

            Assert.Null(result);
        }

        [Fact]
        public async Task UpdateBsbRecord_Should_Return_Call_UpdateBsbRecord_With_The_Right_Input_When_GetAllBsbRecords_Returns_A_List()
        {
            var bsbRecords = new BsbRecordBuilder().AllRecordsWithAllFields().Build();

            fileService.Setup(f => f.GetAllBsbRecords())
                .Returns(Task.FromResult<List<BsbRecord>?>(bsbRecords));

            var newRecord = new BsbRecord(
                "4", "111222", "CBA", "ANZ Smart Choice", "115 Pitt Street", "Sydney", "NSW", "2000", "PEH");

            await service.UpdateBsbRecord(newRecord.Id, newRecord);

            bsbRecords.Remove(bsbRecords[2]);
            bsbRecords.Add(newRecord);

            fileService.Verify(f => f.UpdateBsbRecord(bsbRecords));
        }

        [Fact]
        public async Task UpdateBsbRecord_Should_Return_Call_UpdateBsbRecord_With_The_Right_Input_When_GetAllBsbRecords_Returns_Null()
        {
            fileService.Setup(f => f.GetAllBsbRecords())
                .Returns(Task.FromResult<List<BsbRecord>?>(null));

            var bsbRecord = new BsbRecordBuilder().SingleRecordsWithAllFields().Build()[0];
            await service.UpdateBsbRecord(bsbRecord.Id, bsbRecord);

            fileService.Verify(f => f.UpdateBsbRecord(new List<BsbRecord> { bsbRecord }));
        }

        [Fact]
        public async Task UpdateBsbRecord_Should_Notify_All_Partners()
        {
            fileService.Setup(f => f.GetAllBsbRecords())
                .Returns(Task.FromResult<List<BsbRecord>?>(null));

            _partnerService.Setup(f => f.GetAllPartners())
                .Returns(Task.FromResult<List<Partner>?>(new PartnerBuilder().AllPartners().Build()));

            // Mock HTTP calls to partners
            var response = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(String.Empty)
            };

            var httpMessageHandler = new Mock<HttpMessageHandler>();
            httpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync",
                  ItExpr.Is<HttpRequestMessage>(req => req.Method == HttpMethod.Post),
                  ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(response);

            var httpClient = new HttpClient(httpMessageHandler.Object);

            httpClient.BaseAddress = new Uri("http://example.domain"); 

            _clientFactory.Setup(_ => _.CreateClient(It.IsAny<string>())).Returns(httpClient).Verifiable();

            var httpClientMock = new Mock<HttpClient>(httpClient);

            var bsbRecord = new BsbRecordBuilder().SingleRecordsWithAllFields().Build()[0];
            await service.UpdateBsbRecord(bsbRecord.Id, bsbRecord);

            httpMessageHandler.Protected().Verify(
               "SendAsync",
               Times.Exactly(1), 
               ItExpr.Is<HttpRequestMessage>(req =>
                  req.Method == HttpMethod.Post  
                  && req.RequestUri == new Uri("http://url1.com")
               ),
               ItExpr.IsAny<CancellationToken>()
            );

            httpMessageHandler.Protected().Verify(
               "SendAsync",
               Times.Exactly(1),
               ItExpr.Is<HttpRequestMessage>(req =>
                  req.Method == HttpMethod.Post
                  && req.RequestUri == new Uri("http://url2.com")
               ),
               ItExpr.IsAny<CancellationToken>()
            );
        }
    }
}