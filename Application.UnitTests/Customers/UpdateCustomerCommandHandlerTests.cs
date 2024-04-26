using Application.Customers.Commands;
using Application.Customers.Handlers;
using Core.Interfaces;
using Core.Models.Entities;
using Moq;
using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;

namespace Application.UnitTests.Customers
{
    public class UpdateCustomerCommandHandlerTests
    {
        [Fact]
        public async Task UpdateCustomerCommandHandler_Should_Update_Customer()
        {
            // Arrange
            var mockCustomerRepository = new Mock<ICustomerRepository>();
            var mockUnitOfWork = new Mock<IUnitOfWork>();

            // Setup mock repository
            mockCustomerRepository.Setup(r => r.Update(It.IsAny<Customer>()))
                                  .Returns(true);
            mockCustomerRepository.Setup(r => r.FindById(It.IsAny<Guid>()))
                                  .ReturnsAsync(new Customer { });
            // Setup mock unit of work
            mockUnitOfWork.Setup(_ => _.CustomerRepository).Returns(mockCustomerRepository.Object);

            var handler = new UpdateCustomerCommandHandler(mockUnitOfWork.Object);

            var command = new UpdateCustomerCommand
            {
                FirstName = "John",
                LastName = "Doe",
                DateOfBirth = "1990-02-03",
                PhoneNumber = "+989117115755",
                Email = "john.doe@example.com",
                BankAccountNumber = "1234567890123456"
            };
            var context = new ValidationContext(command);
            var results = new List<ValidationResult>();
            // Act

            var isValid = Validator.TryValidateObject(command, context, results, true);
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(isValid);
            Assert.True(result.Item1);
        }

        [Fact]
        public async Task UpdateCustomerCommandHandler_Should_Return_Error_When_Customer_Not_Found()
        {
            // Arrange
            var mockCustomerRepository = new Mock<ICustomerRepository>();
            var mockUnitOfWork = new Mock<IUnitOfWork>();

            // Setup mock repository
            mockCustomerRepository.Setup(r => r.Update(It.IsAny<Customer>()))
                                  .Returns(true);
            mockCustomerRepository.Setup(r => r.FindById(It.IsAny<Guid>()))
                                  .ReturnsAsync(() => null);
            // Setup mock unit of work
            mockUnitOfWork.Setup(_ => _.CustomerRepository).Returns(mockCustomerRepository.Object);

            var handler = new UpdateCustomerCommandHandler(mockUnitOfWork.Object);

            var command = new UpdateCustomerCommand
            {
                FirstName = "John",
                LastName = "Doe",
                DateOfBirth = "1990-02-03",
                PhoneNumber = "+989117115755",
                Email = "john.doe@example.com",
                BankAccountNumber = "1234567890123456"
            };
            var context = new ValidationContext(command);
            var results = new List<ValidationResult>();
            // Act

            var isValid = Validator.TryValidateObject(command, context, results, true);
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(isValid);
            Assert.False(result.Item1);
        }

        [Fact]
        public async Task UpdateCustomerCommandHandler_With_Duplicate_Email_Should_Return_Error()
        {
            // Arrange
            var mockCustomerRepository = new Mock<ICustomerRepository>();
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            var existingCustomerWithEmail = new Customer { Email = "existing@example.com" };
            // Setup mock repository
            mockCustomerRepository.Setup(r => r.FindById(It.IsAny<Guid>()))
                                  .ReturnsAsync(new Customer { });
            mockCustomerRepository.Setup(r => r.FindByEmail(It.IsAny<string>()))
                                  .ReturnsAsync(existingCustomerWithEmail);

            // Setup mock unit of work
            mockUnitOfWork.Setup(_ => _.CustomerRepository).Returns(mockCustomerRepository.Object);

            var handler = new UpdateCustomerCommandHandler(mockUnitOfWork.Object);

            var command = new UpdateCustomerCommand
            {
                FirstName = "John",
                LastName = "Doe",
                DateOfBirth = "1990-02-03",
                Email = "existing@example.com",
                BankAccountNumber = "1234567890123456"
            };
            // Act

            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.Equal((false, "Email already exists."), result);
        }

        [Fact]
        public async Task UpdateCustomerCommandHandler_With_Duplicate_Details_Should_Return_Error()
        {
            // Arrange
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            var customerRepositoryMock = new Mock<ICustomerRepository>();

            var existingCustomerWithDetails = new Customer { FirstName = "John", LastName = "Doe", DateOfBirth = "1990-02-03" };
            customerRepositoryMock.Setup(r => r.FirstOrDefaultAsync(It.IsAny<Expression<Func<Customer, bool>>>())).ReturnsAsync(existingCustomerWithDetails);
            customerRepositoryMock.Setup(r => r.FindById(It.IsAny<Guid>()))
                                  .ReturnsAsync(new Customer { });
            unitOfWorkMock.SetupGet(u => u.CustomerRepository).Returns(customerRepositoryMock.Object);

            var handler = new UpdateCustomerCommandHandler(unitOfWorkMock.Object);

            var request = new UpdateCustomerCommand { FirstName = "John", LastName = "Doe", DateOfBirth = "1990-02-03" };

            // Act
            var result = await handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.Equal((false, "Customer with the same details already exists."), result);
        }

        [Fact]
        public async Task UpdateCustomerCommandHandler_Should_Return_False_When_PhoneNumber_Is_Not_Valid()
        {
            // Arrange
            var mockCustomerRepository = new Mock<ICustomerRepository>();
            var mockUnitOfWork = new Mock<IUnitOfWork>();

            // Setup mock repository
            mockCustomerRepository.Setup(r => r.Update(It.IsAny<Customer>()))
                                  .Callback<Customer>(_ => _.Id = Guid.NewGuid());

            // Setup mock unit of work
            mockUnitOfWork.Setup(uow => uow.CustomerRepository).Returns(mockCustomerRepository.Object);

            var handler = new UpdateCustomerCommandHandler(mockUnitOfWork.Object);

            var command = new UpdateCustomerCommand
            {
                FirstName = "John",
                LastName = "Doe",
                DateOfBirth = "1990-02-03",
                PhoneNumber = "+9833234344",
                Email = "john.doe@example.com",
                BankAccountNumber = "1234567890123456"
            };
            var context = new ValidationContext(command);
            var results = new List<ValidationResult>();
            // Act

            var isValid = Validator.TryValidateObject(command, context, results, true);
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.False(isValid);
            Assert.False(result.Item1);
        }
    }
}