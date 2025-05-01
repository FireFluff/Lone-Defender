using UnityEngine;
using UnityEngine.Serialization;

public class Player : MonoBehaviour
{
    [SerializeField] private float _hp;
    [SerializeField] public float maxHp;
    
    
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public void TakeDamage(float damage)
    {
        _hp -= damage;
        if (_hp <= 0)
        {
            OnDeath();
        }
    }
    
    public void Heal(float healAmount)
    {
        _hp += healAmount;
        if (_hp > maxHp)
        {
            _hp = maxHp;
        }
    }
    
    private void OnDeath()
    {
        Application.Quit();
    }
}
