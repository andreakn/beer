using Beer.Contracts;

namespace Beer.Core;

public interface IBottleShopGateway
{
    Task<(bool, ProblemDetailsDto)> Recycle(string bottleId);
    Task<(ShipOperationResultDto, ProblemDetailsDto)> Ship(string bottleId);
    Task<(ShipOperationResultDto, ProblemDetailsDto)> ShipCase(CaseDto caseDto);
}