using UnityEngine;

public class UiScript : MonoBehaviour
{
    private GameManager _gameManager;

    private void Start()
    {
        _gameManager = GameManager.Instance;
    }

    public void RegenerateMap()
    {
        _gameManager.RegenerateMap();
    }
}