using UnityEngine;

public class BuildManager : MonoBehaviour
{
    public static BuildManager Instance;

    public Building archerTower;

    [HideInInspector] public bool hasBuilding;

    private Building _activeBuilding;
    private Bank _bank;

    [HideInInspector] public bool CanAfford => _bank.CanPay(_activeBuilding.cost);


    private void Awake()
    {
        if (Instance != null) // If there is already an instance and it's not `this` instance
        {
            Debug.LogError(
                "More than one BuildManager in scene!"); // Destroy the GameObject, this component is attached to
            return;
        }

        Instance = this;
    }

    private void Start()
    {
        _bank = Bank.Instance;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(1)) RemoveBuilding();
    }


    public Building GetBuilding()
    {
        return _activeBuilding;
    }

    public void SetBuildingType(BuildingType type)
    {
        Debug.Log("Set Active Building");
        _activeBuilding = type switch
        {
            BuildingType.ArcherTower => archerTower
        };
        hasBuilding = true;
    }

    public Building GetBuildingFromType(BuildingType type)
    {
        return type switch
        {
            BuildingType.ArcherTower => archerTower
        };
    }

    public void RemoveBuilding()
    {
        hasBuilding = false;
    }
}