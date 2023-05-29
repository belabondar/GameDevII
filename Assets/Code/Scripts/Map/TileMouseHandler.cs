using UnityEngine;

public class TileMouseHandler : MonoBehaviour
{
    private Bank _bank;
    private GameObject _buildingPreview;
    private BuildManager _buildManager;
    private bool _hasPreview;
    private Tile _tile;

    private void Start()
    {
        _buildManager = BuildManager.Instance;
        _tile = gameObject.GetComponent<Tile>();
        _bank = Bank.Instance;
    }

    private void Update()
    {
        //Destroy the preview if building is deselected
        if (!_buildManager.hasBuilding) DestroyPreview();
    }

    private void OnMouseDown()
    {
        Debug.Log(!_tile.isBlocked);
        if (_buildManager.hasBuilding && !_tile.isBlocked) Build();
    }

    private void OnMouseEnter()
    {
        if (_buildManager.hasBuilding) BuildPreview();
    }

    private void OnMouseExit()
    {
        DestroyPreview();
    }

    private void DestroyPreview()
    {
        if (_hasPreview) Destroy(_buildingPreview);
        _hasPreview = false;
    }

    private void BuildPreview()
    {
        var building = Instantiate(_buildManager.GetBuilding(), transform.position, transform.rotation);
        building.SetPreview(!_tile.isBlocked && _buildManager.CanAfford);
        _hasPreview = true;
        _buildingPreview = building.gameObject;
    }

    private void Build()
    {
        //Only build if preview exists -> Only when building is selected
        if (_hasPreview && _buildManager.CanAfford)
        {
            Destroy(_buildingPreview);
            _hasPreview = false;
            _bank.Pay(_buildManager.GetBuilding().GetComponent<Building>().cost);
            _tile.SetBuilding(Instantiate(_buildManager.GetBuilding(), transform.position, transform.rotation));
            _buildManager.RemoveBuilding();
        }
    }
}