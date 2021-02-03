using UnityEngine.Events;
using UnityEngine.AddressableAssets;
using UnityEngine;

namespace Pattern
{
    public class ResourceManager : SingletonCommon<ResourceManager>
    {
        public void LoadAssetAsyn<T>(string key, UnityAction<T> onCompleted)
        {
            var loader = Addressables.LoadAssetAsync<T>(key);
            if (!loader.IsValid())
            {
                Debug.LogError($"Error: failed at loading {key}");
                return;
            }
            loader.Completed += handle =>
            {
                var result = handle.Result;
                onCompleted?.Invoke(result);
            };
        }

        public void Unload<T>(T obj)
        {
            Addressables.Release(obj);
        }
    }
}
