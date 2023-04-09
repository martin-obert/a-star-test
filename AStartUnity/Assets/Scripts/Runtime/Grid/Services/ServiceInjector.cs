﻿using System;
using System.ComponentModel.Design;
using System.Linq;
using Runtime.Inputs;
using Runtime.Messaging;
using Runtime.Services;
using Runtime.Terrains;
using Runtime.Ui;
using UnityEngine;

namespace Runtime.Grid.Services
{
    public class ServiceInjector : MonoBehaviour, IServiceInjector
    {
        [SerializeField] private GameDefinitions gameDefinitions;
        [SerializeField] private TerrainVariant[] terrainVariants;

        private sealed class ServiceRegistrationHook : IDisposable
        {
            private readonly Action _onDispose;

            public ServiceRegistrationHook(Action onDispose)
            {
                _onDispose = onDispose;
            }

            public void Dispose()
            {
                _onDispose?.Invoke();
            }
        }


        public static IServiceInjector Instance { get; private set; }

        public ISceneContextManager SceneContextManager => GetService<ISceneContextManager>();
        public IGridService GridService => GetService<IGridService>();
        public IUserInputService UserInputService => GetService<IUserInputService>();
        public EventPublisher EventPublisher => GetService<EventPublisher>();
        public IAddressableManager AddressableManager => GetService<IAddressableManager>();
        public ISceneManagementService SceneManagementService => GetService<ISceneManagementService>();
        public IPrefabInstantiator PrefabInstantiator => GetService<IPrefabInstantiator>();
        
        public IGridLayoutRepository GridLayoutRepository => GetService<IGridLayoutRepository>();
        public EventSubscriber EventSubscriber => GetService<EventSubscriber>();

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

            RegisterService<IAddressableManager>(c =>
                new AddressableManager(c.GetService<GameDefinitions>(),
                    terrainVariants.OfType<ITerrainVariant>().ToArray()
                ));

            RegisterService<ISceneManagementService>(c => new SceneManagementService(
                GetService<ISceneContextManager>(c),
                GetService<IAddressableManager>(c),
                GetService<GameDefinitions>(c)
            ));

            RegisterService(gameDefinitions);

            RegisterService(new EventSubscriber());
            RegisterService(new PrefabInstantiator());
            RegisterService(new EventPublisher());

            RegisterService<IGridLayoutRepository>(new GridLayoutRepository());
            RegisterService<ISceneContextManager>(_ => new SceneContextManager());

            DontDestroyOnLoad(gameObject);
        }

        private void OnDestroy()
        {
            Dispose();
        }

        private void Dispose()
        {
            if (AddressableManager is IDisposable addressableManager) addressableManager.Dispose();
            _serviceContainer?.Dispose();
            Instance = null;
        }

        public void RegisterService<T>(Func<IServiceContainer, T> creation)
        {
            _serviceContainer.AddService(typeof(T), (c, _) => creation(c));
        }

        public IDisposable RegisterService<T>(T instance)
        {
            _serviceContainer.AddService(typeof(T), instance);

            return new ServiceRegistrationHook(() => _serviceContainer.RemoveService(typeof(T)));
        }

        public IDisposable RegisterService<T>(Func<IServiceProvider, T> callback)
        {
            _serviceContainer.AddService(typeof(T), (container, _) => callback(container));
            return new ServiceRegistrationHook(() => _serviceContainer.RemoveService(typeof(T)));
        }

        public void RemoveService<T>()
        {
            _serviceContainer.RemoveService(typeof(T));
        }
    }
}