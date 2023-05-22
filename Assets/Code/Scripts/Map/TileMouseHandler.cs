using UnityEngine;

public class TileMouseHandler : MonoBehaviour
{
    private BuildingFactory _buildingFactory;
    private BuildManager _buildManager;

    private bool _hasPreview;
    private GameObject _turret;
    private GameObject _turretPreview;

    private void Start()
    {
        _buildManager = BuildManager.Instance;
        _buildingFactory = BuildingFactory.Instance;
        if (_buildingFactory == null) Debug.Log("Error");
    }

    private void Update()
    {
        if (_buildManager.GetBuildingType() == BuildingType.None) DestroyPreview();
    }

    private void OnMouseDown()
    {
        if (CanBuild()) Build();
    }

    private void OnMouseEnter()
    {
        BuildPreview();
    }

    private void OnMouseExit()
    {
        DestroyPreview();
    }

    private bool CanBuild()
    {
        return _buildManager.GetBuildingType() != BuildingType.None && _turret == null &&
               !gameObject.GetComponent<Tile>().IsBlocked();
    }

    private void BuildPreview()
    {
        if (_buildManager.GetBuildingType() == BuildingType.None) return;
        _turretPreview = _buildingFactory.InstancePreviewBuilding(_buildManager.GetBuildingType(),
            CanBuild() ? _buildManager.allowedMaterial : _buildManager.disallowedMaterial, transform.position,
            transform.rotation);
        _hasPreview = true;
    }

    private void DestroyPreview()
    {
        if (_hasPreview)
            Destroy(_turretPreview);
        _turretPreview = null;
        _hasPreview = false;
    }

    private void Build()
    {
        //Only build if preview exists -> Only when building is selected
        if (_hasPreview) Destroy(_turretPreview);

        _hasPreview = false;
        _turret = _buildingFactory.InstanceBuilding(_buildManager.GetBuildingType(), transform.position,
            transform.rotation);
    }
}