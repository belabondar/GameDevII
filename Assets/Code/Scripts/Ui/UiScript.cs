using UnityEngine;

public class UiScript : MonoBehaviour
{
    private BuildManager _buildManager;
    private GameManager _gameManager;

    private void Start()
    {
        _gameManager = GameManager.Instance;
        _buildManager = BuildManager.Instance;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(1)) RemoveBuilding();
    }

    public void BuyArcherTower()
    {
        _buildManager.SetBuildingType(BuildingType.ArcherTower);
    }

    private void RemoveBuilding()
    {
        _buildManager.SetBuildingType(BuildingType.None);
    }

    public void RegenerateMap()
    {
        _gameManager.RegenerateMap();
    }

    public void SetTowerArcher()
    {
        _buildManager.SetBuildingType(BuildingType.ArcherTower);
    }
}