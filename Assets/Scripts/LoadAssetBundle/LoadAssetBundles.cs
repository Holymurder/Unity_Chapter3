using UnityEngine;
using System.Collections;

public class BundlePrefabLoaderUI : MonoBehaviour
{
    [Header("Bundle Settings")]
    [SerializeField]
    private string _bundlePath = "Assets/AssetBundles/mybundle";

    [SerializeField]
    private string _prefabName = "MyCube";

    private GameObject _instance;
    private AssetBundle _bundle;

    public void LoadPrefab()
    {
        if (_instance == null)
        {
            StartCoroutine(LoadPrefabFromBundle());
        }
        else
        {
            Debug.LogWarning("������ ��� ������������!");
        }
    }

    public void UnloadPrefab()
    {
        if (_instance != null)
        {
            Destroy(_instance);
            _instance = null;
        }

        if (_bundle != null)
        {
            _bundle.Unload(false);
            _bundle = null;
        }

        Debug.Log("������ � AssetBundle ����������.");
    }

    private IEnumerator LoadPrefabFromBundle()
    {
        var bundleLoadRequest = AssetBundle.LoadFromFileAsync(_bundlePath);
        yield return bundleLoadRequest;

        _bundle = bundleLoadRequest.assetBundle;
        if (_bundle == null)
        {
            Debug.LogError("�� ������� ����������� AssetBundle!");
            yield break;
        }

        var assetLoadRequest = _bundle.LoadAssetAsync<GameObject>(_prefabName);
        yield return assetLoadRequest;

        var prefab = assetLoadRequest.asset as GameObject;
        if (prefab != null)
        {
            _instance = Instantiate(prefab, Vector3.zero, Quaternion.identity);
        }
        else
        {
            Debug.LogError("������ �� �������� � �����!");
        }
    }
}
