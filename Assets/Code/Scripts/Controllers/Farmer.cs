using UnityEngine;

public class Farmer : MonoBehaviour
{
    private bool _isLocked;

    private ResourceTile _target;

    // Start is called before the first frame update
    private void Start()
    {
        _isLocked = true;
    }

    // Update is called once per frame
    private void Update()
    {
        if (_isLocked) return;
    }

    public void SendTo(ResourceTile tile)
    {
        _target = tile;
        _isLocked = false;
    }
}