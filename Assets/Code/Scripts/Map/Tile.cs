using UnityEngine;

public class Tile : MonoBehaviour
{
    public GameObject pathTile;
    public GameObject groundTile;
    public GameObject endPoint;
    public GameObject startPoint;

    private GameObject _activeTile;
    private BuildManager _buildManager;
    private GameManager _gameManager;
    private int _tileId;
    private GameObject _turret;
    private GameObject _turretPreview;

    private void Start()
    {
        _gameManager = GameManager.Instance;
        _buildManager = BuildManager.Instance;
    }

    private void OnMouseDown()
    {
        if (CanBuild()) BuildTurret();
    }

    private void OnMouseEnter()
    {
        BuildPreviewTurret();
    }

    private void OnMouseExit()
    {
        DestroyPreviewTurret();
    }

    public void Delete()
    {
        Destroy(_activeTile);
        Destroy(this);
    }

    public void SetTile(int tileId)
    {
        var transformInstance = transform;
        _tileId = tileId;
        _activeTile = Instantiate(GetTileFromId(), transformInstance.position, transformInstance.rotation);
        _activeTile.transform.parent = transform;
    }

    private GameObject GetTileFromId()
    {
        return _tileId switch
        {
            1 => pathTile,
            2 => startPoint,
            3 => endPoint,
            _ => groundTile
        };
    }

    public GameObject GetActiveTile()
    {
        return _activeTile;
    }

    private bool CanBuild()
    {
        return _buildManager.GetTurretToBuild() != null && _turret == null && _tileId == 0;
    }

    private void BuildPreviewTurret()
    {
        _turretPreview = Instantiate(_buildManager.GetTurretToBuild(), transform.position, transform.rotation);
        _turretPreview.GetComponent<TowerController>().IsPreview();
        if (CanBuild())
            _turretPreview.GetComponent<Renderer>().material = _buildManager.GetAllowedMaterial();
        else
            _turretPreview.GetComponent<Renderer>().material = _buildManager.GetDisallowedMaterial();
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