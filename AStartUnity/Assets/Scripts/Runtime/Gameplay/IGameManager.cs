using System.Threading;
using Cysharp.Threading.Tasks;

namespace Runtime.Gameplay
{
    public interface IGameManager
    {
        UniTask LoadHexWorldAsync(CancellationToken cancellationToken);
    }
}