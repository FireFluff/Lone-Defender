using UnityEngine;
using UnityEngine.Serialization;

namespace _Scripts
{
    [CreateAssetMenu(fileName = "EnemyStats", menuName = "Scriptable Objects/EnemyStats")]
    public class EnemyStats : ScriptableObject
    {
        public float HP;
        public float speed;
        public float AD;
    }
}
