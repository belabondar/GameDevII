using UnityEngine;

public class Building : MonoBehaviour
{
    public int cost;
    public bool isPreview;

    public float range;
    public float strength;
    public float speed;
    public float fireRate;

    public float rangeUpgradeStrength;
    public float strengthUpgradeStrength;
    public float speedUpgradeStrength;
    public float fireRateUpgradeStrength;

    public int maxUpgrades;

    private int _fireRateUpgrades;

    private GameManager _gameManager;
    private int _rangeUpgrades;
    private int _speedUpgrades;
    private int _strengthUpgrades;

    private void Start()
    {
        _gameManager = GameManager.Instance;
        //Register Tower
        _gameManager.RegisterTower(this);
    }

    public float GetRange()
    {
        return range + rangeUpgradeStrength * _rangeUpgrades;
    }

    public float GetStrength()
    {
        return strength + strengthUpgradeStrength * _strengthUpgrades;
    }

    public float GetSpeed()
    {
        return speed + speedUpgradeStrength * _speedUpgrades;
    }

    public float GetTimeDelay()
    {
        return 1f / fireRate + fireRateUpgradeStrength * _fireRateUpgrades;
    }
}