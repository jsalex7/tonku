namespace Tonku.Game.App
{
    using System.Threading;
    using Cysharp.Threading.Tasks;
    using Microsoft.Extensions.Logging;
    using UniRx;
    using UnityEngine;
    using UnityEngine.AddressableAssets;
    using UnityEngine.SceneManagement;
    using VContainer.Unity;

    public partial class Bootstrap :
        IInitializable,
        System.IDisposable,

        IAsyncStartable
    {
        private readonly CompositeDisposable _compositeDisposable = new CompositeDisposable();

        private Microsoft.Extensions.Logging.ILogger _logger;

        private Tonku.Game.Resource.IResourceService _resourceService;

        private AppLifetimeScope.Settings _settings;

        public Bootstrap(
            Microsoft.Extensions.Logging.ILogger logger,
            AppLifetimeScope.Settings settings)
        {
            _logger = logger;
            _settings = settings;
        }

        void IInitializable.Initialize()
        {
            _logger.LogInformation("Initialize");
            _resourceService = new Tonku.Game.Resource.ResourceService(this);

            Setup();
        }

        void System.IDisposable.Dispose()
        {
            _logger.LogInformation("Dispose");
            _compositeDisposable?.Dispose();
        }

        public async UniTask StartAsync(CancellationToken cancellation)
        {
            _logger.LogInformation("StartAsync");
            await Setup();

            // var asyncOp = SceneManager.LoadSceneAsync("Stage - Entry", LoadSceneMode.Additive);
            // await asyncOp.ToUniTask();

            // var asyncLoad = Addressables.LoadSceneAsync("Stage - Entry", LoadSceneMode.Additive);
            // _resourceServiceProvider?.AddSceneHandler(key, asyncLoad);

            //
            // var result = await asyncLoad.Task;
        }
    }
}
