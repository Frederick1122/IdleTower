using System.Collections;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private GameObject _enemyPrefab;
    [SerializeField] private float _spawnCooldown = 3f;

    [SerializeField] private Transform _enemyParent;
    [SerializeField] private SphereCollider _sphereCollider;

    private void Start()
    {
        StartCoroutine(SpawnRoutine());
    }

    [ContextMenu("Spawn")]
    public void SpawnEnemy()
    {
        var randomAngle = _sphereCollider.transform.rotation;
        randomAngle.eulerAngles = new Vector3(0, Random.Range(1, 359), 0);
        _sphereCollider.transform.rotation = randomAngle;
        var enemyPosition = _sphereCollider.transform.position + _sphereCollider.transform.forward * _sphereCollider.radius;
        var newEnemy = Instantiate(_enemyPrefab, enemyPosition,quaternion.identity);
        newEnemy.transform.parent = _enemyParent;
    }

    public IEnumerator SpawnRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(_spawnCooldown);
            SpawnEnemy();
        }
    }
}
