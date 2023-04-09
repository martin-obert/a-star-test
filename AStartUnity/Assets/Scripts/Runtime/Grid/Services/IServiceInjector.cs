using System;
using Runtime.Gameplay;
using Runtime.Grid.Presenters;
using Runtime.Inputs;
using Runtime.Messaging;
using Runtime.Services;
using Runtime.Terrains;
using Runtime.Ui;

namespace Runtime.Grid.Services
{
    public interface IServiceInjector
    {
        ISceneContextManager SceneContextManager { get; }
        IGridService GridService { get; }
        IUserInputService UserInputService { get; }
        IGridLayoutRepository GridLayoutRepository { get; }
        EventSubscriber EventSubscriber { get; }
        EventPublisher EventPublisher { get; }
        IAddressableManager AddressableManager { get; }
        ISceneManagementService SceneManagementService { get; }
        IPrefabInstantiator PrefabInstantiator { get; }
        IDisposable RegisterService<T>(T instance);
        IDisposable RegisterService<T>(Func<IServiceProvider, T> callback);
        
    }
}