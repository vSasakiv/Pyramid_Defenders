using UnityEngine;

namespace ScriptableObjects
{
    public enum Difficulty
    {
        Easy,
        Medium,
        Hard
    }

    public enum Strategy
    {
        Wandering,
        Attacking,
        Defending,
        Ranged
    }
    
    [CreateAssetMenu(fileName = "Enemy", menuName = "Enemy", order = 0)]
    public class EnemyScriptableObject : ScriptableObject
    {
        public int baseHealth;
        public float moveSpeed;
        public float jumpPower;
        public float fallSpeed;
        public Strategy strategy;
        public Difficulty difficulty;
    }
}