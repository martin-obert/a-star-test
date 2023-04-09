﻿using System.Threading;
using Cysharp.Threading.Tasks;
using Runtime.Grid.Data;

namespace Runtime
{
    public interface ISceneManagementService
    {
        UniTask LoadWorldAsync(int rowsCount, int colsCount, CancellationToken token = default);
        UniTask LoadLayoutAsync(IGridCellViewModel[] cells, CancellationToken token = default);
    }
}