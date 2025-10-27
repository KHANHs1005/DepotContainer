using Xunit;
using Moq;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using DepotContainer.API.Controllers;
using DepotContainer.Application.Interfaces.Repositories;
using System.Threading.Tasks;

namespace DepotContainer.UnitTests.TestAPI
{
    public class AuthControllerTests
    {
        private readonly Mock<IStaffRepository> _mockRepo;
        private readonly AuthController _controller;

        public AuthControllerTests()
        {
            _mockRepo = new Mock<IStaffRepository>();
            _controller = new AuthController(_mockRepo.Object);
        }

        [Fact]
        public async Task Login_Should_Return_BadRequest_When_Username_Or_Password_Is_Empty()
        {
            // Arrange
            var req = new LoginRequest { Username = "", Password = "" };

            // Act
            var result = await _controller.Login(req);

            // Assert
            result.Should().BeOfType<BadRequestObjectResult>();
            var badReq = result as BadRequestObjectResult;
            badReq!.Value.Should().BeEquivalentTo(new { message = "Vui lòng nhập đầy đủ username và password." });
        }

        [Fact]
        public async Task Login_Should_Return_Unauthorized_When_Staff_Not_Found()
        {
            // Arrange
            var req = new LoginRequest { Username = "notfound", Password = "123" };
            _mockRepo.Setup(r => r.GetByUsernameAsync("notfound")).ReturnsAsync((DepotContainer.Domain.Entities.Staff?)null);

            // Act
            var result = await _controller.Login(req);

            // Assert
            result.Should().BeOfType<UnauthorizedObjectResult>();
            var unauth = result as UnauthorizedObjectResult;
            unauth!.Value.Should().BeEquivalentTo(new { message = "Tài khoản không tồn tại." });
        }

        [Fact]
        public async Task Login_Should_Return_Unauthorized_When_Wrong_Password()
        {
            // Arrange
            var staff = new DepotContainer.Domain.Entities.Staff
            {
                StaffId = 1,
                StaffName = "A",
                Username = "a",
                Password = "123",
                IsActive = true
            };
            _mockRepo.Setup(r => r.GetByUsernameAsync("a")).ReturnsAsync(staff);

            var req = new LoginRequest { Username = "a", Password = "wrong" };

            // Act
            var result = await _controller.Login(req);

            // Assert
            result.Should().BeOfType<UnauthorizedObjectResult>();
            var unauth = result as UnauthorizedObjectResult;
            unauth!.Value.Should().BeEquivalentTo(new { message = "Sai mật khẩu." });
        }

        [Fact]
        public async Task Login_Should_Return_Unauthorized_When_Account_Inactive()
        {
            // Arrange
            var staff = new DepotContainer.Domain.Entities.Staff
            {
                StaffId = 2,
                StaffName = "B",
                Username = "b",
                Password = "123",
                IsActive = false
            };
            _mockRepo.Setup(r => r.GetByUsernameAsync("b")).ReturnsAsync(staff);

            var req = new LoginRequest { Username = "b", Password = "123" };

            // Act
            var result = await _controller.Login(req);

            // Assert
            result.Should().BeOfType<UnauthorizedObjectResult>();
            var unauth = result as UnauthorizedObjectResult;
            unauth!.Value.Should().BeEquivalentTo(new { message = "Tài khoản đã bị khóa." });
        }

        [Fact]
        public async Task Login_Should_Return_Ok_When_Success()
        {
            // Arrange
            var staff = new DepotContainer.Domain.Entities.Staff
            {
                StaffId = 3,
                StaffName = "C",
                Username = "c",
                Password = "123",
                IsActive = true,
                StaffType = "Admin"
            };
            _mockRepo.Setup(r => r.GetByUsernameAsync("c")).ReturnsAsync(staff);

            var req = new LoginRequest { Username = "c", Password = "123" };

            // Act
            var result = await _controller.Login(req);

            // Assert
            result.Should().BeOfType<OkObjectResult>();
            var ok = result as OkObjectResult;
            ok!.Value.Should().BeEquivalentTo(new
            {
                message = "Đăng nhập thành công",
                staffId = staff.StaffId,
                staffName = staff.StaffName,
                staffType = staff.StaffType,
                username = staff.Username
            }, opts => opts.ExcludingMissingMembers());
        }

        [Fact]
        public async Task Login_Should_Handle_Repository_Exception()
        {
            // Arrange
            var req = new LoginRequest { Username = "err", Password = "123" };
            _mockRepo.Setup(r => r.GetByUsernameAsync("err"))
                     .ThrowsAsync(new System.Exception("DB error"));

            // Act
            Func<Task> act = async () => await _controller.Login(req);

            // Assert
            await act.Should().ThrowAsync<System.Exception>()
                .WithMessage("DB error");
        }
    }
}
