using System;
using UnityEngine;
using UnityEngine.Rendering;

public class Building : MonoBehaviour
{
    public bool isPreview;
    public Cost cost;
    public BuildingType type;

    public Renderer turretRenderer;
    public Material allowedMaterial;
    public Material disallowedMaterial;


    private GameManager _gameManager;

    private void Start()
    {
        _gameManager = GameManager.Instance;
        //Register Tower
        _gameManager.RegisterTower(this);
    }

    public void SetPreview(bool canBuild)
    {
        turretRenderer.material = canBuild ? allowedMaterial : disallowedMaterial;
        turretRenderer.shadowCastingMode = ShadowCastingMode.Off;

        isPreview = true;
    }

    [Serializable]
    public class Cost
    {
        public int gold;
        public int wood;
        public int stone;
    }

    [Serializable]
    public class Trait
    {
        public float baseValue;
        public float upgradeStrength;
        public int maxUpgrades;
        public Cost upgradeCost;
        private int _upgrades;

        public bool CanUpgrade => Bank.Instance.CanPay(upgradeCost) && _upgrades < maxUpgrades;
        public float Value => baseValue + upgradeStrength * _upgrades;

        public void Upgrade()
        {
            if (!CanUpgrade) return;
            _upgrades++;
            Bank.Instance.Pay(upgradeCost);
        }
    }
}