using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

public class TowerController : MonoBehaviour
{
    public float projectileSpeed = 10f;
    public int damage = 2;
    public float range = 5f;
    public float rateOfFire = 0.2f;
    public GameObject projectile;
    public Transform launchPoint;

    private GameManager _gameManager;

    private Boolean _allowFire = true;

    private EnemyController _target;

    private Boolean _hasTarget;
    // Start is called before the first frame update
    void Start()
    {
        _gameManager = GameManager.Instance;
    }

    // Update is called once per frame
    void Update()
    {
        if (!_hasTarget)
        {
            _target = _gameManager.GetTarget(transform, range);
            _hasTarget = !(_target != null);
        }
        else if (_allowFire)
        {
            StartCoroutine(Launch());
        }
    }

    private IEnumerator Launch()
    {
        _allowFire = false;
        var position = launchPoint.position;
        var dir = _target.transform.position - position;
        var rotation = Quaternion.LookRotation(dir);
        Instantiate(projectile, position, rotation);
        yield return new WaitForSeconds(rateOfFire);
        _allowFire = true;
    }
}
