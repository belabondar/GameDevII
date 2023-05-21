using System.Collections;
using JetBrains.Annotations;
using UnityEngine;

public class TowerController : MonoBehaviour
{
    public float baseProjectileSpeed = 10f;
    public float baseDamage = 2f;
    public float baseRange = 5f;
    public float baseRateOfFire = 1f;

    public float projectileSpeedOffset = 0.5f;
    public float damageOffset = 0.5f;
    public float rangeOffset = 2f;
    public float fireRateOffset = 0.5f;

    public int maxUpgrades = 15;
    public ProjectileController projectile;
    public Transform launchPoint;

    private GameManager _gameManager;

    private bool _hasTarget, _isPreview;

    private int _speedUpgrades, _damageUpgrades, _rangeUpgrades, _fireRateUpgrades;

    private EnemyController _target;

    // Start is called before the first frame update
    private void Start()
    {
        _gameManager = GameManager.Instance;
        if (_isPreview) return;
        _gameManager.AddTower(this);
        StartCoroutine(Launch());
    }

    private void Update()
    {
        _target = GetTarget();
    }

    public void IsPreview()
    {
        _isPreview = true;
    }

    [CanBeNull]
    private EnemyController GetTarget()
    {
        var enemies = _gameManager.GetEnemies();
        foreach (var enemy in enemies)
            if (Vector3.Distance(enemy.transform.position, transform.position) <= CalcRange())
            {
                Debug.Log("Found target");
                return enemy;
            }

        Debug.Log("Found no target");
        return null;
    }

    private float CalcRange()
    {
        return baseRange + _rangeUpgrades * rangeOffset;
    }

    //Launch an arrow every x seconds if there is targets in range
    private IEnumerator Launch()
    {
        while (true)
        {
            if (_target != null)
            {
                var position = launchPoint.position;
                var dir = _target.transform.position - position;
                var rotation = Quaternion.LookRotation(dir);
                var projectileInstance = Instantiate(projectile, position, rotation);
                //Make Instance a child of the tower
                projectileInstance.transform.parent = gameObject.transform;

                //Settings
                projectileInstance.speed = baseProjectileSpeed + _speedUpgrades * projectileSpeedOffset;
                projectileInstance.damage = baseDamage + _damageUpgrades * damageOffset;
                projectileInstance.distanceCull = CalcRange();

                //Set Target
                projectileInstance.Target = _target;
            }


            yield return new WaitForSeconds(1f / (baseRateOfFire + _fireRateUpgrades * fireRateOffset));
        }
    }

    public float GetRange()
    {
        return baseRange + _rangeUpgrades * rangeOffset;
    }

    public void RemoveTarget()
    {
        _hasTarget = false;
    }

    public void UpgradeFireRate()
    {
        if (_fireRateUpgrades < maxUpgrades)
            _fireRateUpgrades += 1;
    }

    public void UpgradeRange()
    {
        if (_rangeUpgrades < maxUpgrades)
            _rangeUpgrades += 1;
    }

    public void UpgradeDamage()
    {
        if (_damageUpgrades < maxUpgrades)
            _damageUpgrades += 1;
    }

    public void UpgradeProjectileSpeed()
    {
        if (_speedUpgrades < maxUpgrades)
            _speedUpgrades += 1;
    }
}