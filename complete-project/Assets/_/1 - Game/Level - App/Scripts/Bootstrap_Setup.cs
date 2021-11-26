namespace Tonku.Game.App
{
    using Cysharp.Threading.Tasks;
    using Microsoft.Extensions.Logging;
    using Unity.VisualScripting;

    public partial class Bootstrap
    {
        private async UniTask Setup()
        {
            _logger.LogInformation("Setup");

            await SetupAddressable();

            //
            var variablesComp = _settings.managerGO.GetComponent<Variables>();
            if (variablesComp != null)
            {
                _logger.LogDebug("Inject reference to UVS variables");
                variablesComp.declarations.Set("|o| resourceService", _resourceService);
            }
        }
    }
}
