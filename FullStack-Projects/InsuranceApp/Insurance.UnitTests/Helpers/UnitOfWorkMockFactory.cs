using Moq;
using Insurance.Application.Interfaces.Repositories;

namespace Insurance.UnitTests.TestHelpers;

public static class UnitOfWorkMockFactory
{
    public static Mock<IUnitOfWork> Create()
    {
        var clientRepo = new Mock<IClientRepository>();
        var buildingRepo = new Mock<IBuildingRepository>();
        var geographyRepo = new Mock<IGeographyRepository>();
        var identifierRepo = new Mock<IClientIdentifierRepository>();

        var uow = new Mock<IUnitOfWork>();

        uow.Setup(x => x.Clients).Returns(clientRepo.Object);
        uow.Setup(x => x.Buildings).Returns(buildingRepo.Object);
        uow.Setup(x => x.Geography).Returns(geographyRepo.Object);
        uow.Setup(x => x.ClientIdentifiers).Returns(identifierRepo.Object);
        uow.Setup(x => x.SaveChangesAsync()).ReturnsAsync(1);

        return uow;
    }
}
