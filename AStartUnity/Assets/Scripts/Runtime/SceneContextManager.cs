using System.Threading;
using Cysharp.Threading.Tasks;
using Runtime.Grid.Data;
using UnityEngine.AddressableAssets;

namespace Runtime
{
    public interface ISceneManagementService
    {
        UniTask LoadWorldAsync(int rowsCount, int colsCount, CancellationToken token = default);
        UniTask LoadLayoutAsync(IGridCell[] cells, CancellationToken token = default);
    }

    public sealed class SceneContext
    {
        public int RowCount { get; set; }
        public int ColCount { get; set; }
        public IGridCell[] Cells { get; set; }
    }

    public interface ISceneContextManager
    {
        void SetContext(SceneContext value);
        SceneContext GetContext();
    }

    public sealed class SceneContextManager : ISceneContextManager
    {
        private SceneContext _context;

        public void SetContext(SceneContext value)
        {
            _context = value;
        }

        public SceneContext GetContext()
        {
            return _context;
        }
    }
}