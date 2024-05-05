using System;
using Enemies.EnemyStrategies;
using Interfaces;
using ScriptableObjects;
using Unity.Netcode;
using UnityEngine;

namespace Enemies
{
    public class MeleeEnemy : NetworkBehaviour, IDamageable
    {
        [SerializeField] private EnemyScriptableObject enemyStats;
        
        private IEnemyStrategy _enemyStrategy;
        private float _currentHealth;

        public float GetHealth()
        {
            return _currentHealth;
        }

        public bool TakeDamage(float damageAmount)
        {
            _currentHealth -= damageAmount;
            return _currentHealth <= 0;
        }

        public void Awake()
        {
            switch (enemyStats.strategy)
            {
                case Strategy.Wandering:
                    _enemyStrategy = gameObject.AddComponent<WanderingStrategy>();
                    break;
                case Strategy.Attacking:
                    break;
                case Strategy.Defending:
                    break;
                case Strategy.Ranged:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            _currentHealth = enemyStats.baseHealth;
        }

        // Update is called once per frame
        private void Update()
        {
            if (!IsServer) return;
            _enemyStrategy.UpdateStrategy();
        }
    }
}
