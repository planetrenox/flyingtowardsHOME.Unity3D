using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class ObstacleSpawner : MonoBehaviour
{
    public GameObject[] obstaclePrefab;
    public int numberOfObstacles = 2000;
    public int numberOfObstaclesClose = 100; // Number of obstacles to spawn closer
    public float spawnDistance = 5000.0f;
    public float recycleDistance = 7000.0f;

    private Transform _playerTransform;
    private readonly List<GameObject> _obstaclePool = new List<GameObject>();

    private void Start()
    {
        _playerTransform = Camera.main.transform;

        // Spawn the desired number of obstacles close to the player
        for (var i = 0; i < numberOfObstaclesClose; i++)
        {
            SpawnObstacle(true); // true means spawning close
        }

        // Fill the remaining obstacles in the surrounding area
        for (var i = numberOfObstaclesClose; i < numberOfObstacles; i++)
        {
            SpawnObstacle(false); // false means spawning in the surrounding area
        }
    }

    private void Update()
    {
        RecycleObstacles();
    }

    void SpawnObstacle(bool close)
    {
        // Choose a random obstacle prefab from the array
        var obstacle = obstaclePrefab[Random.Range(0, obstaclePrefab.Length)];

        // Calculate a random position within the spawnDistance or closer
        var randomPosition = _playerTransform.position +
                             (close ? Random.onUnitSphere * spawnDistance * 0.5f : Random.onUnitSphere * spawnDistance);

        // Instantiate the obstacle at the random position
        var newObstacle = Instantiate(obstacle, randomPosition, Quaternion.identity);
        _obstaclePool.Add(newObstacle);
    }

    private void RecycleObstacles()
    {
        foreach (var obstacle in _obstaclePool.Where(obstacle =>
                     Vector3.Distance(obstacle.transform.position, _playerTransform.position) > recycleDistance))
        {
            // Move the obstacle back within the spawnDistance
            obstacle.transform.position = _playerTransform.position +
                                          Random.onUnitSphere * spawnDistance;
        }
    }
}