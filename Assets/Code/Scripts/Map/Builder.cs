using UnityEngine;

public class Builder : MonoBehaviour
{
    private BuildManager _buildManager;
    private GameObject _turret;
    private GameObject _turretPreview;

    private void Start()
    {
        _buildManager = BuildManager.Instance;
    }

    private void OnMouseDown()
    {
        if (CanBuild()) BuildTurret();
    }

    private void OnMouseEnter()
    {
        Debug.Log("Enter");
        BuildPreviewTurret();
    }

    private void OnMouseExit()
    {
        DestroyPreviewTurret();
    }

    private bool CanBuild()
    {
        return _buildManager.GetTurretToBuild() != null && _turret == null &&
               !gameObject.GetComponent<Tile>().IsBlocked();
    }

    private void BuildPreviewTurret()
    {
        _turretPreview = Instantiate(_buildManager.GetTurretToBuild(), transform.position, transform.rotation);
        _turretPreview.GetComponent<TowerController>().IsPreview();
        _turretPreview.GetComponentInChildren<Renderer>().material =
            CanBuild() ? _buildManager.GetAllowedMaterial() : _buildManager.GetDisallowedMaterial();
    }

    private void DestroyPreviewTurret()
    {
        if (_turretPreview != null)
            Destroy(_turretPreview);
        _turretPreview = null;
    }

    private void BuildTurret()
    {
        if (_turretPreview != null) Destroy(_turretPreview);

        _turret = Instantiate(_buildManager.GetTurretToBuild(), transform.position, transform.rotation);
    }
}