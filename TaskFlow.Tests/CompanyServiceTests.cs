using Xunit;
using Moq;
using TaskFlow.Services;
using TaskFlow.Interfaces;

public class CompanyServiceTests
{
    [Fact]
    public void IsDuplicateCompany_ShouldReturnTrue_WhenCompanyExists()
    {
        // Arrange
        //var mockRepo = new Mock<ICompanyService>();

        //mockRepo.Setup(r => r.CompanyExists("ABC Ltd"))
        //        .Returns(true);

        //var service = new CompanyService(mockRepo.Object);

        //// Act
        //var result = service.IsDuplicateCompany("ABC Ltd");

        //// Assert
        //Assert.True(result);
    }
}