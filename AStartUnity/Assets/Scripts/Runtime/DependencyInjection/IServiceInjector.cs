using System;
using System.ComponentModel.Design;
using Runtime.Grid.Services;
using Runtime.Inputs;
using Runtime.Messaging;
using Runtime.Services;

namespace Runtime.DependencyInjection
{
    /// <summary>
    /// <para>
    /// Simple DI using .net <see cref="ServiceContainer"/>
    /// </para>
    /// <para>
    /// This interface brings more insight into available systems by exposing them as props</para>
    /// </summary>
    public interface IServiceInjector
    {
        IGridSetupManager GridSetupManager { get; }
        IGridService GridService { get; }
        IUserInputService UserInputService { get; }
        IGridLayoutRepository GridLayoutRepository { get; }
        EventSubscriber EventSubscriber { get; }
        EventPublisher EventPublisher { get; }
        IAddressableManager AddressableManager { get; }
        ISceneManagementService SceneManagementService { get; }
        IPrefabInstantiatorService PrefabInstantiatorService { get; }
        IDisposable RegisterService<T>(T instance);
        IDisposable RegisterService<T>(Func<IServiceProvider, T> callback);
        
    }
}