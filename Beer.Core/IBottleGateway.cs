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
}