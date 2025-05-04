using System;
using UnityEngine;

namespace _Scripts
{
    public class Enemy : MonoBehaviour
    {
        [SerializeField] private EnemyStats enemyStats;
        private EnemyStats _currentStats;
    
        private Transform _playerTransform;
    
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        private void OnEnable()
        {
            _playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        
            _currentStats = ScriptableObject.CreateInstance<EnemyStats>();
            _currentStats.HP = enemyStats.HP;
            _currentStats.speed = enemyStats.speed;
            _currentStats.AD = enemyStats.AD;
        }

        // Update is called once per frame
        private void Update()
        {
            if (_currentStats.HP > 0)
            {
                MoveTowardsPlayer();
                RotateTowardsPlayer();
            }
            else
            {
                OnDeath();
            }
        }
        
        private void RotateTowardsPlayer()
        {
            Vector3 direction = _playerTransform.position - transform.position;
            direction.Normalize();
        
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            Quaternion rotation = Quaternion.Euler(new Vector3(0, 0, angle - 90));
        
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * _currentStats.speed);
        }
        
        private void MoveTowardsPlayer()
        {
            Vector3 direction = _playerTransform.position - transform.position;
            direction.Normalize();
        
            transform.position += direction * (_currentStats.speed * Time.deltaTime);
        }
        
        public void TakeDamage(float damage)
        {
            _currentStats.HP -= damage;
            if (_currentStats.HP <= 0)
            {
                OnDeath();
            }
        }

        private void OnDeath()
        {
            ObjectPoolManager.ReturnToPool(gameObject);
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                other.GetComponent<Player>().TakeDamage(_currentStats.AD);
                OnDeath();
            }
        }
    }
}
