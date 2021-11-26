namespace Tonku.Game.App
{
    using Microsoft.Extensions.Logging;
    using Serilog;
    using Serilog.Sinks.Unity3D;
    using UnityEngine;
    using VContainer;
    using VContainer.Unity;

    public class AppLifetimeScope : LifetimeScope
    {
        [System.Serializable]
        public class Settings
        {
            public GameObject managerGO;
        }

        public Settings settings;

        //
        private Serilog.Core.Logger log = new LoggerConfiguration()
            .Enrich.FromLogContext()
            .MinimumLevel.Debug()
            // .WriteTo.Unity3D(outputTemplate: "{Timestamp:HH:mm} [{Level}] ({Name:l}) {Message}\n")
            .WriteTo.Unity3D(outputTemplate: "[{Level}] ({Name:l}) {Message}\n")
            // .WriteTo.Seq("")
            // .WriteTo.Console(outputTemplate: "{Timestamp:HH:mm} [{Level}] ({Name:l}) {Message}\n")
            .CreateLogger();

        //
        protected override void Configure(IContainerBuilder builder)
        {
            // builder.Register<AudioService>(Lifetime.Singleton);
            // builder.Register<Bootstrap>(Lifetime.Singleton);
            builder.RegisterEntryPoint<Bootstrap>();

            var factory = new LoggerFactory();
            factory.AddSerilog(log);
            var logger = factory.CreateLogger("test");

            builder.RegisterInstance(logger);
            builder.RegisterInstance(settings);

            // builder.RegisterComponentInHierarchy<Hud>();
            // builder.RegisterComponentInHierarchy<Hud>();
            // builder.RegisterComponentInNewPrefab(hudPrefab, Lifetime.Scoped);
            // builder.RegisterComponentOnNewGameObject<Hud>(Lifetime.Scoped, "Hud");
        }
    }
}
