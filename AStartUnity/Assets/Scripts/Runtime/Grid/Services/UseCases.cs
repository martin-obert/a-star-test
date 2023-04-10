using System;
using Cysharp.Threading.Tasks;
using Runtime.Messaging;

namespace Runtime.Grid.Services
{
    public static class UseCases
    {
        public static async UniTask GridInitialization(
            SceneContext context,
            IGridService gridService,
            EventPublisher eventPublisher)
        {
            try
            {
                await UniTask.SwitchToMainThread();
                if (!context.HasCells())
                {
                    gridService.CreateNewGrid(context.RowCount, context.ColCount);
                }
                else if (context.HasCells())
                {
                    gridService.InstantiateGrid(context.RowCount, context.ColCount, context.Cells);
                }

                eventPublisher.OnGridInstantiated();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}