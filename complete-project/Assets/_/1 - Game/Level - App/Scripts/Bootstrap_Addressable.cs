namespace Tonku.Game.App
{
    using System.Threading.Tasks;
    using UnityEngine.AddressableAssets;

    public partial class Bootstrap
    {
        private async Task SetupAddressable()
        {
            var addressableInitializeAsync = Addressables.InitializeAsync();

            await addressableInitializeAsync.Task;
        }
    }
}
