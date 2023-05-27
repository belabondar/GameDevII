using UnityEngine;
using UnityEngine.Rendering;

public class BuildingFactory : MonoBehaviour
{
    public static BuildingFactory Instance;

    public GameObject archerTower;
    public GameObject mageTower;
    public GameObject woodCollector;
    public GameObject stoneCollector;

    private void Awake()
    {
        if (Instance != null) // If there is already an instance and it's not `this` instance
        {
            Debug.LogError(
                "More than one BuildingFactory in scene!"); // Destroy the GameObject, this component is attached to
            return;
        }

        Instance = this;
    }

    public GameObject InstancePreviewBuilding(BuildingType type, Material material, Vector3 pos, Quaternion rot)
    {
        var building = Instantiate(GetBuildingFromType(type), pos, rot);
        var buildingScript = building.GetComponent<Building>();
        buildingScript.isPreview = true;
        var renderer = buildingScript.GetComponentInChildren<Renderer>();
        renderer.material = material;
        renderer.shadowCastingMode = ShadowCastingMode.Off;

        return building;
    }

    public GameObject InstanceBuilding(BuildingType type, Vector3 pos, Quaternion rot)
    {
        return Instantiate(GetBuildingFromType(type), pos, rot);
    }

    private GameObject GetBuildingFromType(BuildingType type)
    {
        return type switch
        {
            BuildingType.ArcherTower => archerTower,
            BuildingType.MageTower => mageTower,
            BuildingType.WoodCollector => woodCollector,
            BuildingType.StoneCollector => stoneCollector
        };
    }
}