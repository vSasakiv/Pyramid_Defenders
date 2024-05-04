using System;
using UnityEngine;

public class MeleeEnemy : BaseEnemyBehaviour, IDamageable
{
    [SerializeField] private float healthPoints = 8;
    private WanderingStrategy enemyStrategy;
    
    public float Health
    {
        get
        {
            return healthPoints;
        }
        
        set
        {
            healthPoints = value;
        }
    }

    public bool TakeDamage(float damageAmount)
    {
        healthPoints -= damageAmount;
        return healthPoints <= 0;
    }

    public void Awake()
    {
        enemyStrategy = gameObject.AddComponent<WanderingStrategy>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!IsServer) return;
        enemyStrategy.UpdateStrategy();
    }
}
