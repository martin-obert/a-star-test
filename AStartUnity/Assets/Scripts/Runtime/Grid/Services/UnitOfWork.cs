using System;
using System.Collections;
using System.ComponentModel.Design;
using System.Threading;
using Cysharp.Threading.Tasks;
using Runtime.Gameplay;
using Runtime.Grid.Presenters;
using Runtime.Inputs;
using Runtime.Messaging;
using Runtime.Terrains;
using UnityEngine;

namespace Runtime.Grid.Services
{
    public class UnitOfWork : MonoBehaviour
    {
        [SerializeField] private GridCellRepository gridCellRepository;

        public static UnitOfWork Instance { get; private set; }

        public ISceneContextManager SceneContextManager => GetService<ISceneContextManager>();
        public EventPublisher EventPublisher => GetService<EventPublisher>();
        public IGameManager GameManager => GetService<IGameManager>();
        public EventSubscriber EventSubscriber => GetService<EventSubscriber>();
        public IGridManager GridManager => GetService<IGridManager>();
        public IUserInputManager UserInputManager => GetService<IUserInputManager>();

        public IGridCellRepository GridCellRepository => gridCellRepository;

        public ITerrainVariantRepository TerrainVariantRepository => gridCellRepository;

        public IGridRaycaster GridRaycaster => GetService<IGridRaycaster>();

        public IGridLayoutRepository GridLayoutRepository => GetService<IGridLayoutRepository>();

        private ServiceContainer _serviceContainer;

        private T GetService<T>() => GetService<T>(_serviceContainer);
        private static T GetService<T>(IServiceProvider sc) => (T)sc.GetService(typeof(T));

        private void Awake()
        {
            if (Instance != null && !ReferenceEquals(Instance, this))
            {
                Destroy(this);
                return;
            }

            Instance = this;

            _serviceContainer = new ServiceContainer();

            RegisterService<ITerrainVariantRepository>(gridCellRepository);
            RegisterService<IGridLayoutRepository>(c =>
                new GridLayoutRepository(GetService<ITerrainVariantRepository>(c)));
            
            RegisterService(new EventSubscriber());
            RegisterService(new EventPublisher());
            RegisterService<IGridRaycaster>((_) => new GridRaycaster());
            RegisterService<ISceneContextManager>((_) => new SceneContextManager());
            DontDestroyOnLoad(gameObject);
        }

        private void OnDestroy()
        {
            Dispose();
        }

        private void Dispose()
        {
            _serviceContainer?.Dispose();
            Instance = null;
        }

        public void RegisterService<T>(Func<IServiceContainer, T> creation)
        {
            _serviceContainer.AddService(typeof(T), (c, t) => creation(c));
        }

        public void RegisterService<T>(T instance)
        {
            _serviceContainer.AddService(typeof(T), instance);
        }

        public void RemoveService<T>()
        {
            _serviceContainer.RemoveService(typeof(T));
        }
    }
}