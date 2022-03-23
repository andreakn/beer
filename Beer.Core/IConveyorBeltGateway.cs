using Beer.Contracts;

namespace Beer.Core;

public interface IConveyorBeltGateway
{
    Task<(bool, ProblemDetailsDto)> Start();
    Task<(bool, ProblemDetailsDto)> Stop();
    Task<(BottleDto, NotBottleDto,ProblemDetailsDto)> Step();
    Task<(ConveyorBeltState, ProblemDetailsDto)> GetState();
    Task<(bool, ProblemDetailsDto)> Fix();
}