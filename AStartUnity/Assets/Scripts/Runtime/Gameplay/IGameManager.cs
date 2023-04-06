using System.Threading;
using Cysharp.Threading.Tasks;
using Runtime.Grid.Services;

namespace Runtime.Gameplay
{
    public interface IGameManager
    {
        UniTask LoadHexWorldAsync(CancellationToken cancellationToken);
        GridSetup GridSetup { get; set; }
    }
}