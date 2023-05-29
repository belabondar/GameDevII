using UnityEngine;

public class Tile : MonoBehaviour
{
    [HideInInspector] public bool isBlocked = true;
    public TileType type;
    private Building _building;
    public MapInfo mapInfo;

    public void Init(int x, int y, TileType type)
    {
        mapInfo.xCoord = x;
        mapInfo.yCoord = y;
        this.type = type;

        if (type == TileType.Ground) isBlocked = false;
    }

    public void UnBlock()
    {
        isBlocked = false;
    }


    public void Destroy()
    {
        foreach (Transform child in transform) Destroy(child.gameObject);
        Destroy(gameObject);
    }

    public void SetBuilding(Building building)
    {
        isBlocked = true;
        _building = building;
    }

    public struct MapInfo
    {
        public int xCoord;
        public int yCoord;
    }
}