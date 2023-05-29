using System.Collections.Generic;
using Code.Scripts.Types;
using UnityEngine;

public class ResourceTile : MonoBehaviour
{
    public ResourceType resourceType;
    public int resourceAmount = 5000;
    public GameObject[] envAssets;

    private ResourceManager _resourceManager;
    private List<GameObject> _resources;

    private void Start()
    {
        _resourceManager = ResourceManager.Instance;
        _resourceManager.AddResourceTile(this);
        _resources = new List<GameObject>();
        BuildEnv();
    }

    public int Harvest(int strength)
    {
        if (resourceAmount - strength > 0) return strength;

        _resourceManager.RemoveResourceTile(this);
        RemoveResources();
        gameObject.GetComponent<Tile>().UnBlock();
        return resourceAmount;
    }

    private void RemoveResources()
    {
        foreach (var obj in _resources) Destroy(obj);
    }


    private void BuildEnv()
    {
        float[] xMinMax = { -2f, 2f };
        float[] zMinMax = { -2f, 2f };
        var assetCount = resourceType == ResourceType.Wood ? Random.Range(10, 20) : Random.Range(3, 8);
        if (envAssets.Length == 0) return;
        for (var i = 0; i < assetCount; i++)
        {
            var asset = envAssets[Random.Range(0, envAssets.Length - 1)];
            var position = transform.position + new Vector3(Random.Range(xMinMax[0], xMinMax[1]), 0f,
                Random.Range(zMinMax[0], zMinMax[1]));
            var obj = Instantiate(asset, position, new Quaternion(0, Random.Range(0f, 360f), 0, 0));
            obj.transform.parent = transform;
            _resources.Add(obj);
        }
    }
}