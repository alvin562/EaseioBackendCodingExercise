using System;
using System.Threading.Tasks;
using Xunit;
using Moq;
using Microsoft.AspNetCore.Mvc;
using EaseioBackendCodingExercise.Controllers;
using EaseioBackendCodingExercise.Services;
using EaseioBackendCodingExercise.Models;

namespace EaseioBackendCodingExercise.Tests
{
    public class GuidControllerTests
    {
        private readonly Mock<IGUIDService> _mockService;
        private readonly GuidController _controller;

        public GuidControllerTests()
        {
            _mockService = new Mock<IGUIDService>();
            _controller = new GuidController(_mockService.Object);
        }

        [Fact]
        public async Task GetGUIDRecord_ReturnsRecord_WhenValidRecordFound()
        {
            string guid = "1234567890ABCDEF1234567890ABCDEF";
            var dt = DateTimeOffset.Parse("2026-10-01T14:18:00Z");

            _mockService.Setup(s => s.GetByGuidAsync(guid))
                        .ReturnsAsync(new GUIDRecord(guid, dt, "alice"));

            var result = await _controller.GetGUIDRecord(guid);

            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async Task GetGUIDRecord_ReturnsNotFound_WhenRecordExpired()
        {
            string guid = "1234567890ABCDEF1234567890ABCDEF";
            var dt = DateTimeOffset.Parse("2024-10-01T14:18:00Z");

            _mockService.Setup(s => s.GetByGuidAsync(guid))
                        .ReturnsAsync(new GUIDRecord(guid, dt, "alice"));

            var result = await _controller.GetGUIDRecord(guid);

            Assert.IsType<NotFoundObjectResult>(result);
        }

        [Fact]
        public async Task GetGUIDRecord_ReturnsNotFound_WhenRecordDoesNotExist()
        {
            string guid = "1234567890ABCDEF1234567890ABCDEF";

            _mockService.Setup(s => s.GetByGuidAsync(guid))
                        .ReturnsAsync((GUIDRecord?)null);

            var result = await _controller.GetGUIDRecord(guid);

            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task CreateGUIDRecord_ReturnsBadRequest_WhenGuidIsInvalid()
        {
            string badGuid = "NOTVALID";
            var request = new CreateGUID
            {
                Expires = DateTimeOffset.UtcNow.AddDays(1),
                User = "tester"
            };

            var result = await _controller.CreateGUIDRecord(badGuid, request);

            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task CreateGUIDRecord_ReturnsConflict_WhenRecordAlreadyExists()
        {
            string guid = "F2FBDCC74FB1661D376AB6932E96D65E"; // valid 32-char hex
            var request = new CreateGUID
            {
                Expires = DateTimeOffset.UtcNow.AddDays(1),
                User = "tester"
            };

            _mockService.Setup(s => s.CreateAsync(It.IsAny<GUIDRecord>()))
                .ReturnsAsync(false); // simulate conflict

            var result = await _controller.CreateGUIDRecord(guid, request);

            var conflict = Assert.IsType<ConflictObjectResult>(result);
            Assert.Contains("already exists", conflict.Value!.ToString());
        }

        [Fact]
        public async Task CreateGUIDRecord_ReturnsCreatedAt_WhenSuccessful()
        {
            string guid = "F2FBDCC74FB1661D376AB6932E96D65E"; // valid hex
            var request = new CreateGUID
            {
                Expires = DateTimeOffset.UtcNow.AddDays(1),
                User = "tester"
            };

            _mockService.Setup(s => s.CreateAsync(It.IsAny<GUIDRecord>()))
                .ReturnsAsync(true);

            var result = await _controller.CreateGUIDRecord(guid, request);

            var created = Assert.IsType<CreatedAtActionResult>(result);
            Assert.Equal(nameof(GuidController.GetGUIDRecord), created.ActionName);

            Assert.IsType<GUIDRecord>(created.Value);
            var returnedRecord = (GUIDRecord)created.Value!;
            Assert.Equal(guid, returnedRecord.Guid);
        }

        [Fact]
        public async Task UpdateGUIDRecord_ShouldReturnNoContent_WhenUpdateSucceeds()
        {
            string guid = "test-guid";
            var request = new UpdateGUID { Expires = DateTime.UtcNow.AddHours(2) };

            _mockService.Setup(s => s.UpdateAsync(guid, request.Expires))
                .ReturnsAsync(true);

            var result = await _controller.UpdateGUIDRecord(guid, request);

            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task UpdateGUIDRecord_ShouldReturnNotFound_WhenUpdateFails()
        {
            string guid = "missing-guid";
            var request = new UpdateGUID { Expires = DateTime.UtcNow.AddHours(1) };

            _mockService.Setup(s => s.UpdateAsync(guid, request.Expires))
                .ReturnsAsync(false);

            var result = await _controller.UpdateGUIDRecord(guid, request);

            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task DeleteGUIDRecord_ShouldReturnOk_WhenDeleteSucceeds()
        {
            string guid = "existing-guid";

            _mockService.Setup(s => s.DeleteAsync(guid))
                .ReturnsAsync(true);

            var result = await _controller.DeleteGUIDRecord(guid);

            Assert.IsType<OkResult>(result);
        }

        [Fact]
        public async Task DeleteGUIDRecord_ShouldReturnNotFound_WhenDeleteFails()
        {
            string guid = "missing-guid";

            _mockService.Setup(s => s.DeleteAsync(guid))
                .ReturnsAsync(false);

            var result = await _controller.DeleteGUIDRecord(guid);

            var notFound = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal($"Unable to delete record as GUID is not found: {guid}", notFound.Value);
        }
    }
}