namespace Tonku.Game.Stage
{
    using Cysharp.Threading.Tasks;
    using Microsoft.Extensions.Logging;
    using Unity.VisualScripting;
    using UnityEngine.AddressableAssets;
    using UnityEngine.SceneManagement;

    public partial class Bootstrap
    {
        private async UniTask Setup()
        {
            _logger.LogInformation("Setup");

            //
            var variablesComp = _settings.managerGO.GetComponent<Variables>();
            if (variablesComp != null)
            {
                _logger.LogDebug("Inject reference to UVS variables");
                // variablesComp.declarations.Set("|o| resourceService", _resourceService);

                variablesComp.declarations.Set("rank1ManagerGO", _appSettings.managerGO);
            }

            // var asyncLoad = Addressables.LoadSceneAsync("Location - 001", LoadSceneMode.Additive);
            // var result = await asyncLoad.Task;
        }
    }
}
