using UnityEngine;

public class BuildManager : MonoBehaviour
{
    public static BuildManager Instance;


    public Material allowedMaterial;
    public Material disallowedMaterial;

    private BuildingType _buildingType;

    private void Awake()
    {
        if (Instance != null) // If there is already an instance and it's not `this` instance
        {
            Debug.LogError(
                "More than one BuildManager in scene!"); // Destroy the GameObject, this component is attached to
            return;
        }

        Instance = this;

        _buildingType = BuildingType.None;
    }

    public BuildingType GetBuildingType()
    {
        return _buildingType;
    }

    public void SetBuildingType(BuildingType type)
    {
        _buildingType = type;
    }


    public Material GetAllowedMaterial()
    {
        return allowedMaterial;
    }

    public Material GetDisallowedMaterial()
    {
        return disallowedMaterial;
    }
}