using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public GameObject[] envAssets;
    private readonly List<GameObject> envObjects = new();
    private bool _blocked;
    private GameObject _building;
    private TileType _tileType;

    public void Init(TileType tileType)
    {
        _tileType = tileType;
        if (tileType == TileType.Ground)
        {
            if (!(Random.Range(0f, 1f) < 0.2f)) return;
            _blocked = true;
            BuildEnv();
        }
        else
        {
            _blocked = true;
        }
    }

    public bool UnBlock()
    {
        if (_tileType == TileType.Ground)
            _blocked = false;

        //If successfully unblocked return true
        return !_blocked;
    }

    public bool IsBlocked()
    {
        return _blocked;
    }

    private void BuildEnv()
    {
        float[] xMinMax = { -1.5f, 1.5f };
        float[] zMinMax = { -1.5f, 1.5f };
        var assetCount = Random.Range(4, 8);
        if (envAssets.Length == 0) return;
        for (var i = 0; i < assetCount; i++)
        {
            Debug.Log("Spawn Asset");
            var asset = envAssets[Random.Range(0, envAssets.Length - 1)];
            var position = transform.position + new Vector3(Random.Range(xMinMax[0], xMinMax[1]), 0f,
                Random.Range(zMinMax[0], zMinMax[1]));
            var obj = Instantiate(asset, position, new Quaternion(0, Random.Range(0f, 360f), 0, 0));
            obj.transform.parent = transform;
            envObjects.Add(obj);
        }
    }

    public void Destroy()
    {
        foreach (var item in envObjects) Destroy(item);
        Destroy(gameObject);
    }

    public void SetBuilding(GameObject building)
    {
        _building = building;
    }
}