using UnityEngine;

public class BuildManager : MonoBehaviour
{
    public static BuildManager Instance;

    public GameObject turret;


    public Material allowedMaterial;
    public Material disallowedMaterial;

    private GameObject _turretToBuild;

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
        _turretToBuild = turret;
    }

    public GameObject GetTurretToBuild()
    {
        return _turretToBuild;
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