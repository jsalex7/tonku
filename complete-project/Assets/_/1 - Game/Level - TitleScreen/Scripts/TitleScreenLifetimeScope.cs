namespace Tonku.Game.TitleScreen
{
    using Microsoft.Extensions.Logging;
    using Serilog;
    using Serilog.Sinks.Unity3D;
    using UnityEngine;
    using VContainer;
    using VContainer.Unity;

    public class TitleScreenLifetimeScope : LifetimeScope
    {
        [System.Serializable]
        public class Settings
        {
            public GameObject managerGO;
        }

        public Settings settings;

        protected override void Configure(IContainerBuilder builder)
        {
            // builder.Register<AudioService>(Lifetime.Singleton);
            // builder.Register<Bootstrap>(Lifetime.Singleton);
            builder.RegisterEntryPoint<Bootstrap>();

            builder.RegisterInstance(settings);

            // builder.RegisterComponentInHierarchy<Hud>();
            // builder.RegisterComponentInHierarchy<Hud>();
            // builder.RegisterComponentInNewPrefab(hudPrefab, Lifetime.Scoped);
            // builder.RegisterComponentOnNewGameObject<Hud>(Lifetime.Scoped, "Hud");
        }
    }
}
