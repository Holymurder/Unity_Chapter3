using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class AddressablePrefabLoader : MonoBehaviour
{
    [Header("Drag Addressable prefab here")]
    [SerializeField]
    private AssetReferenceGameObject _prefabRef;

    private AsyncOperationHandle<GameObject> _instHandle;
    private bool _spawned;

    public async void Spawn()
    {
        if (_spawned || _prefabRef == null || !_prefabRef.RuntimeKeyIsValid())
        {
            Debug.LogWarning("[Addressables] Cannot spawn: invalid reference or already spawned.");
            return;
        }

        _instHandle = _prefabRef.InstantiateAsync(Vector3.zero, Quaternion.identity, null);
        var instance = await _instHandle.Task;

        if (instance == null)
        {
            Debug.LogError("[Addressables] InstantiateAsync returned null.");
            return;
        }

        _spawned = true;
    }

    public void Despawn()
    {
        if (_spawned && _instHandle.IsValid())
        {
            Addressables.ReleaseInstance(_instHandle);
        }

        _spawned = false;
    }

    private void OnDestroy()
    {
        Despawn();
    }
}
