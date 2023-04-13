using Cysharp.Threading.Tasks;
using Runtime.DependencyInjection;
using Runtime.Grid.Services;
using UnityEngine;

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
            var layoutRepository = ServiceInjector.Instance.GridLayoutRepository;
            var addressable = ServiceInjector.Instance.AddressableManager;
            var gameManager = ServiceInjector.Instance.SceneManagementService;
            var listSaves = layoutRepository.ListSaves();
            
            foreach (var save in listSaves)
            {
                Instantiate(itemPrefab, container)
                    .Bind(save, filename => UniTask.Void(async () =>
                    {
                        var cells = await layoutRepository.LoadAsync(filename, addressable.GetTerrainVariants());                        
                        await gameManager.LoadLayoutAsync(cells);
                    }));
            }
        }
    }
}