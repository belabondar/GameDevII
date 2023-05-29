using UnityEngine;

public class ProjectileTower : MonoBehaviour
{
    public ProjectileController projectile;
    public Transform launchPoint;

    public Building.Trait range;
    public Building.Trait strength;
    public Building.Trait projectileSpeed;
    public Building.Trait fireRateInShotsPerSecond;

    public int maxUpgrades;

    private Building _building;
    private GameManager _gameManager;

    private float _lastShot;
    private EnemyController _target;

    // Start is called before the first frame update
    private void Start()
    {
        _gameManager = GameManager.Instance;
        _building = gameObject.GetComponent<Building>();
    }

    private void Update()
    {
        //Dont shoot if not enough time has passed to the last shot or its a preview
        if (Time.time - _lastShot < 1f / fireRateInShotsPerSecond.Value || _building.isPreview) return;
        var hasTarget = GetTarget();
        if (hasTarget)
        {
            var position = launchPoint.position;
            var dir = _target.transform.position - position;
            var rotation = Quaternion.LookRotation(dir);
            var projectileInstance = Instantiate(projectile, position, rotation);

            _lastShot = Time.time;

            //Make Instance a child of the tower
            projectileInstance.transform.parent = gameObject.transform;

            //Settings
            projectileInstance.speed = projectileSpeed.Value;
            projectileInstance.damage = strength.Value;
            projectileInstance.distanceCull = range.Value;

            //Set Target
            projectileInstance.Target = _target;
        }
    }

    private bool GetTarget()
    {
        var enemies = _gameManager.GetEnemies();
        foreach (var enemy in enemies)
            if (Vector3.Distance(enemy.transform.position, transform.position) <= range.Value)
            {
                _target = enemy;
                return true;
            }

        return false;
    }
}