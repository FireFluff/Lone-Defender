using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Serialization;

namespace _Scripts
{
    public class EnemySpawner : MonoBehaviour
    {
        [ReorderableList]
        [SerializeField] private List<GameObject> enemyPrefabs;
        
        [Foldout("Spawn\\Wave Modifiers")]
        [SerializeField] private float distanceOffScreen = 2f;
        /*[Foldout("Spawn\\Wave Modifiers")]
        [SerializeField] private float startSpawnRatePS = 0.2f;*/
        [Foldout("Spawn\\Wave Modifiers")]
        [SerializeField] private float spawnRateIncrease = 0.2f;
        [Foldout("Spawn\\Wave Modifiers")]
        [SerializeField] private int waveNumber = 1;
        [FormerlySerializedAs("enemyPerWave")]
        [Foldout("Spawn\\Wave Modifiers")]
        [SerializeField] private int enemiesPerWave = 1;
        
        private Vector3 _screenBounds;
        [Foldout("Spawn\\Wave Modifiers"), ShowNonSerializedField]
        private float _currentSpawnRatePS;
        private int _currentWaveEnemiesSpawned;
        private Camera _mainCamera;
        
        void Start()
        {
            _mainCamera = Camera.main;
            UpdateScreenBounds();
            /*_currentSpawnRatePS = startSpawnRatePS;*/
            StartCoroutine(SpawnEnemiesCoroutine());
        }
        
        private void UpdateScreenBounds()
        {
            _screenBounds = _mainCamera.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, _mainCamera.transform.position.z));
        }
        
        private IEnumerator SpawnEnemiesCoroutine()
        {
            while (true)
            {
                if (_currentWaveEnemiesSpawned < enemiesPerWave)
                {
                    SpawnEnemy();
                    _currentWaveEnemiesSpawned++;
                    yield return new WaitForSeconds(1f / enemiesPerWave);
                }
                else
                {
                    StartNewWave();
                    yield return new WaitForSeconds(10f); // Delay before the next wave
                }
            }
        }
    
        private Vector3 GetOffScreenPosition()
        {
            // Randomly choose an edge (top, bottom, left, right)
            int edge = Random.Range(0, 4);
            Vector3 position = Vector3.zero;

            switch (edge)
            {
                case 0: // Top
                    position = new Vector3(Random.Range(-_screenBounds.x, _screenBounds.x), _screenBounds.y + distanceOffScreen, 0);
                    break;
                case 1: // Bottom
                    position = new Vector3(Random.Range(-_screenBounds.x, _screenBounds.x), -_screenBounds.y - distanceOffScreen, 0);
                    break;
                case 2: // Left
                    position = new Vector3(-_screenBounds.x - distanceOffScreen, Random.Range(-_screenBounds.y, _screenBounds.y), 0);
                    break;
                case 3: // Right
                    position = new Vector3(_screenBounds.x + distanceOffScreen, Random.Range(-_screenBounds.y, _screenBounds.y), 0);
                    break;
            }

            return position;
        }
        
        private void SpawnEnemy()
        {
            Vector3 spawnPosition = GetOffScreenPosition();
            int enemyIndex = Mathf.Min(waveNumber - 1, enemyPrefabs.Count - 1);
            GameObject enemyToSpawn = enemyPrefabs[enemyIndex];
            GameObject enemy = ObjectPoolManager.SpawnFromPool(
                enemyToSpawn, spawnPosition, Quaternion.identity, ObjectPoolManager.PoolType.Enemies);
            if (enemy.TryGetComponent<Enemy>(out var enemyComponent))
            {
                enemyComponent.SetWaveStats(waveNumber);
            }
            _currentWaveEnemiesSpawned++;
        }
        
        private void SpawnEnemies(int count)
        {
            for (int i = 0; i < count; i++)
            {
                Vector3 spawnPosition = GetOffScreenPosition();
                int enemyIndex = Mathf.Min(waveNumber - 1, enemyPrefabs.Count - 1);
                GameObject enemyToSpawn = enemyPrefabs[enemyIndex];
                GameObject enemy = ObjectPoolManager.SpawnFromPool(enemyToSpawn, spawnPosition, Quaternion.identity, ObjectPoolManager.PoolType.Enemies);
                enemy.GetComponent<Enemy>().SetWaveStats(waveNumber);
                _currentWaveEnemiesSpawned++;
            }
        }
        
        private void StartNewWave()
        {
            waveNumber++;
            _currentSpawnRatePS += spawnRateIncrease;
            enemiesPerWave += 2; // Increase enemies per wave
            _currentWaveEnemiesSpawned = 0; // Reset counter
            Debug.Log("Wave " + waveNumber + " started! Spawn rate: " + 1 / enemiesPerWave  + " enemies per second.");
        }
    }
}
