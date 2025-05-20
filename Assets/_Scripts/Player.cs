using UnityEngine;
using UnityEngine.Serialization;

public class Player : MonoBehaviour
{
    Camera mainCamera;
    [SerializeField] private float _hp;
    [SerializeField] public float maxHp;
    [SerializeField] public float maxShield;
    [SerializeField] public float maxAttack;
    
    
   
    void Start()
    {
        mainCamera = Camera.main;
    }

   
    void Update()
    {
         if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0); // Only using the first touch

            
            Vector3 touchPos = mainCamera.ScreenToWorldPoint(touch.position);
            touchPos.z = 0f; 

            
            Vector2 direction = (touchPos - transform.position).normalized;

            
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, angle - 90f);
        }
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
