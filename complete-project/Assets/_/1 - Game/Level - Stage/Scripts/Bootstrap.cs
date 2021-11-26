namespace Tonku.Game.Stage
{
    using System.Threading;
    using System.Threading.Tasks;
    using Cysharp.Threading.Tasks;
    using Microsoft.Extensions.Logging;
    using UniRx;
    using VContainer.Unity;

    public partial class Bootstrap :
        IInitializable,
        System.IDisposable,

        IAsyncStartable
    {
        private readonly CompositeDisposable _compositeDisposable = new CompositeDisposable();

        private Microsoft.Extensions.Logging.ILogger _logger;

        private Tonku.Game.App.AppLifetimeScope.Settings _appSettings;
        private StageLifetimeScope.Settings _settings;

        public Bootstrap(
            Microsoft.Extensions.Logging.ILogger logger,
            Tonku.Game.App.AppLifetimeScope.Settings appSettings,
            StageLifetimeScope.Settings settings)
        {
            _logger = logger;
            _appSettings = appSettings;
            _settings = settings;
        }

        void IInitializable.Initialize()
        {
            _logger.LogInformation("Initialize");
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
        }
    }
}
