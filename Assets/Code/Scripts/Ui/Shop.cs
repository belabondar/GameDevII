using UnityEngine;
using UnityEngine.UI;

public class Shop : MonoBehaviour
{
    public Button archerTowerButton;
    private Bank _bank;
    private BuildManager _buildManager;
    private GameManager _gameManager;

    private void Start()
    {
        _gameManager = GameManager.Instance;
        _buildManager = BuildManager.Instance;
        _bank = Bank.Instance;
    }

    private void Update()
    {
        if (_bank.CanPay(_buildManager.GetBuildingFromType(BuildingType.ArcherTower).cost))
            archerTowerButton.interactable = true;
        else archerTowerButton.interactable = false;
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