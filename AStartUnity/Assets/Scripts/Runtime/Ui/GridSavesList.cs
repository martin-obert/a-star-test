using System.Linq;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using Runtime.Grid.Services;
using UnityEngine;
using UnityEngine.Events;

namespace Runtime.Ui
{
    public sealed class GridSavesList : MonoBehaviour
    {
        [SerializeField] private GridSavesListItem itemPrefab;

        [SerializeField] private Transform container;

        private void Awake()
        {
            if (!container)
                container = transform;
        }

        private void Start()
        {
            var saves = UnitOfWork.Instance.GridLayoutRepository.ListSaves();
            foreach (var save in saves)
            {
                Instantiate(itemPrefab, container).Bind(save, (s) =>
                {
                    UniTask.Void(async () =>
                    {
                        var cells = await UnitOfWork.Instance.GridLayoutRepository.LoadAsync(s);
                        UnitOfWork.Instance.SceneContextManager.SetContext(new SceneContext
                        {
                            Cells = cells,
                            ColCount = cells.Max(x => x.ColIndex),
                            RowCount = cells.Max(x => x.RowIndex)
                        });
                        await UnitOfWork.Instance.GameManager.LoadHexWorldAsync(default);
                    });
                });
            }
        }
    }
}