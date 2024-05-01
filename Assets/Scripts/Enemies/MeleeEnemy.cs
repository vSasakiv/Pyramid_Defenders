using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeEnemy : BaseEnemyBehaviour, IDamageable
{
    [SerializeField] private float healthPoints = 8;

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
    
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
