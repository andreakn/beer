using Beer.Contracts;

namespace Beer.Core;

public interface IBrewingGateway
{
    Task<(bool,BottleDto, ProblemDetailsDto)> FillBottle(string bottleId);
    Task<(int, ProblemDetailsDto)> GetLevel(string beerType);
    Task<(IEnumerable<(string,int)>, ProblemDetailsDto)> GetLevels();
    Task<(bool,ProblemDetailsDto)> FillContainer(string beerType);
}