using System;
using Code.Scripts.Types;
using TMPro;
using UnityEngine;

public class Bank : MonoBehaviour
{
    public static Bank Instance;
    public TMP_Text goldText;
    public TMP_Text woodText;
    public TMP_Text stoneText;
    private int _gold;
    private int _stone;
    private int _wood;

    private void Awake()
    {
        if (Instance != null) // If there is already an instance and it's not `this` instance
        {
            Debug.LogError(
                "More than one Bank in scene!"); // Destroy the GameObject, this component is attached to
            return;
        }

        Instance = this;
    }

    public int GetResourceAmount(ResourceType type)
    {
        return type switch
        {
            ResourceType.Gold => _gold,
            ResourceType.Wood => _wood,
            ResourceType.Stone => _stone,
            _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
        };
    }

    public void DepositResource(int gold, int wood, int stone)
    {
        _gold += gold;
        _wood += wood;
        _stone += stone;

        UpdateUiValues();
    }

    public bool CanPay(int gold, int wood, int stone)
    {
        return _gold - gold >= 0 && _wood - wood >= 0 && _stone - stone >= 0;
    }

    public bool CanPay(Building.Cost cost)
    {
        return CanPay(cost.gold, cost.wood, cost.stone);
    }

    public bool Pay(int gold, int wood, int stone)
    {
        if (!CanPay(gold, wood, stone)) return false;
        _gold -= gold;
        _wood -= wood;
        _stone -= stone;
        UpdateUiValues();
        return true;
    }

    public bool Pay(Building.Cost cost)
    {
        return Pay(cost.gold, cost.wood, cost.stone);
    }

    private void UpdateUiValues()
    {
        goldText.text = _gold.ToString();
        woodText.text = _wood.ToString();
        stoneText.text = _stone.ToString();
    }
}