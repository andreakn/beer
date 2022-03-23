using Beer.Contracts;

namespace Beer.Core;

public interface IBottleShopGateway
{
    Task<(bool, ProblemDetailsDto)> Recycle(string bottleId);
    Task<(ShipOperationResultDto, ProblemDetailsDto)> Ship(string bottleId);
    Task<(ShipOperationResultDto, ProblemDetailsDto)> ShipCase(CaseDto caseDto);
}

public interface IConveyorBeltGateway
{
    Task<(bool, ProblemDetailsDto)> Start();
    Task<(bool, ProblemDetailsDto)> Stop();
    Task<(BottleDto, NotBottleDto,ProblemDetailsDto)> Step();
    Task<(ConveyorBeltState, ProblemDetailsDto)> GetState();
    Task<(bool, ProblemDetailsDto)> Fix();
}

public interface IBrewingGateway
{
    Task<(BottleDto, ProblemDetailsDto)> FillBottle(string bottleId);
    Task<(int, ProblemDetailsDto)> GetLevel(string beerType);
    Task<(IEnumerable<(string,int)>, ProblemDetailsDto)> GetLevels();
    Task<(bool,ProblemDetailsDto)> FillContainer(string beerType);
}