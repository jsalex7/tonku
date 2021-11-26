namespace Tonku.Game.App
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Logging;
    using UniRx;
    using Unity.VisualScripting;
    using UnityEngine;
    using UnityEngine.AddressableAssets;
    using UnityEngine.ResourceManagement.AsyncOperations;
    using UnityEngine.ResourceManagement.ResourceProviders;

    public partial class Bootstrap :
        Tonku.Game.Resource.IResourceServiceProvider
    {
        private readonly Dictionary<string, AsyncOperationHandle> _aohSceneCollection =
            new Dictionary<string, AsyncOperationHandle>();

        public void AddSceneHandler(string key, AsyncOperationHandle asyncLoad)
        {
            // _logger
            //     .ForContext(typeof(Bootstrap))
            //     .ForContext("Method", nameof(AddSceneHandler))
            //     .Debug($"Add scene for {key}");
            _logger.LogDebug("Add scene for {0}", key);

            if (!_aohSceneCollection.ContainsKey(key))
            {
                _aohSceneCollection.Add(key, asyncLoad);
            }
        }

        public void RequestToUnloadScene(GameObject requester, string key)
        {
            // _logger
            //     .ForContext(typeof(Bootstrap))
            //     .ForContext("Method", nameof(RequestToUnloadScene))
            //     .Debug($"Start to unload scene for {key}");

            _logger.LogDebug("Start to unload scene for {0}", key);

            if (_aohSceneCollection.ContainsKey(key))
            {
                var holdingAoh = _aohSceneCollection[key];

                if (holdingAoh.IsValid())
                {
                    _RequestToUnloadScene(holdingAoh).ToObservable()
                        .ObserveOnMainThread()
                        .SubscribeOnMainThread()
                        .Subscribe(result =>
                        {
                            // _logger
                            //     .ForContext(typeof(Bootstrap))
                            //     .ForContext("Method", nameof(RequestToUnloadScene))
                            //     .Debug($"Done unloading scene for {key}");

                            _logger.LogDebug("Done unloading scene for {0}", key);


                            _aohSceneCollection.Remove(key);

                            CustomEvent.Trigger(requester, $"Unload Scene - {key}", new object[] { result });
                        })
                        .AddTo(_compositeDisposable);
                }
            }
        }

        private async Task _RequestToUnloadScene(AsyncOperationHandle aoh)
        {
            // _logger
            //     .ForContext(typeof(Bootstrap))
            //     .ForContext("Method", nameof(_RequestToUnloadScene))
            //     .Debug($"Start to unload");
            _logger.LogDebug("Start to unload");

            var asyncUnload = Addressables.UnloadSceneAsync(aoh, true);

            // _logger
            //     .ForContext(typeof(Bootstrap))
            //     .ForContext("Method", nameof(_RequestToUnloadScene))
            //     .Debug($"----------------------- 1");


            var result = await asyncUnload.Task;

            // return result;
        }

        private void ReleaseSceneCollection()
        {
            // // Need a way to dispose async
            // foreach (var aohPair in _aohSceneCollection)
            // {
            //     _logger
            //         .ForContext(typeof(Bootstrap))
            //         .ForContext("Method", nameof(ReleaseSceneCollection))
            //         .Debug($"{aohPair.Key}");
            //
            //     if (aohPair.Value.IsValid())
            //     {
            //         var asyncUnloadResult = Addressables.UnloadSceneAsync(aohPair.Value);
            //         // var result = await asyncUnload.Task;
            //     }
            // }
        }
    }
}
