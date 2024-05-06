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
        // Basic attributes, health, strategy and difficulty
        public int baseHealth;
        public Strategy strategy;
        public Difficulty difficulty;

        // Physical attributes
        public float height;
        public float radius;
        public float speed;
        public float acceleration;
        
        public float jumpPower;
        public float fallSpeed;
        
    }
}