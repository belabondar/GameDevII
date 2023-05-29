using System.Collections.Generic;
using Code.Scripts.Types;
using UnityEngine;

public class Farm : MonoBehaviour
{
    public ResourceType farmType;
    public Farmer farmer;
    public Building.Trait farmerAmount;
    public Building.Trait range;
    public Building.Trait gatheringStrength;
    public Building.Trait movementSpeed;
    public Building.Trait gatheringTimeInSeconds;
    private Bank _bank;
    private Building _building;
    private List<Farmer> _farmers;

    private ResourceManager _resourceManager;
    private List<ResourceTile> _targets;

    private void Start()
    {
        _resourceManager = ResourceManager.Instance;
        _building = gameObject.GetComponent<Building>();
        _targets = _resourceManager.GetResourceTilesInRadius(transform.position, range.Value);
        _bank = Bank.Instance;
    }

    private void Update()
    {
        if (_farmers.Count == 0) return;

        foreach (var farmer in _farmers)
        {
        }
    }

    private void BuildFarmerPool()
    {
        for (var i = 0; i < farmerAmount.Value; i++)
            _farmers.Add(Instantiate(farmer, transform.position, transform.rotation));
    }
}