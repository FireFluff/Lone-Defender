using UnityEngine;
using UnityEngine.Serialization;

namespace _Scripts
{
    public class EnemySpawner : MonoBehaviour
    {
        [SerializeField] private GameObject enemyPrefab;
        [SerializeField] private float distanceOffScreen = 2f;
        [SerializeField] private float startSpawnRatePS = 2f;
        [SerializeField] private float spawnRateIncrease = 0.1f;
        [SerializeField] private int waveDuration = 30;
        
        private float _timer;
        private Vector3 _screenBounds;
        private float _waveTimer;
        private int _waveNumber = 1;
        private float _currentSpawnRatePS;
        private Camera _mainCamera;
        
        void Start()
        {
            _mainCamera = Camera.main;
            _screenBounds = _mainCamera.ScreenToWorldPoint(new 
                Vector3(Screen.width, Screen.height, _mainCamera.transform.position.z));
            _currentSpawnRatePS = startSpawnRatePS;
        }

        // Update is called once per frame
        void Update()
        {
            /*_waveTimer += Time.deltaTime;
            
            if (_waveTimer >= waveDuration)
            {
                StartNewWave();
            }*/
            
            if (_timer < 1 / startSpawnRatePS)
            {
                _timer += Time.deltaTime;
            }
            else
            {
                SpawnEnemy();
                _timer = 0;
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
            GameObject enemy = ObjectPool.ShearedInstance.GetPooledObject();
            enemy.transform.position = spawnPosition;
            enemy.SetActive(true);
        }
        
        private void StartNewWave()
        {
            _waveNumber++;
            _currentSpawnRatePS += spawnRateIncrease;
            _waveTimer = 0;
            Debug.Log("Wave " + _waveNumber + " started! Spawn rate: " + 1/_currentSpawnRatePS + " enemies per second.");
        }
    }
}
