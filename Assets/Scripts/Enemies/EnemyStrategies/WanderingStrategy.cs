using Unity.Netcode;
using UnityEngine;

public class WanderingStrategy : BaseStrategy, IEnemyStrategy
{
    // Enemy Strategy. Wanders around spawn position
    enum WanderingState
    {
        wandering,
        waiting
    }
    
    [SerializeField]
    private NetworkVariable<float> wanderingDelay =
        new NetworkVariable<float>(4f, NetworkVariableReadPermission.Everyone);
    [SerializeField] private float wanderRadius = 12f;
    [SerializeField] private float wanderSpeed = 3f;
    [SerializeField] private float stoppingDistance = 0.3f;
    
    private WanderingState currentState = WanderingState.waiting;
    private float currentWanderDelay = 2f;
    private Vector3 spawnPosition;
    private Vector3 targetPosition;
    

    private void Awake()
    {
        spawnPosition = transform.position;
    }
    
    public override void UpdateStrategy()
    {
        if (!IsServer) return;
        
        switch (currentState)
        {

            case WanderingState.waiting:
                UpdateWaiting();
                break;
            case WanderingState.wandering:
                UpdateWandering();
                break;
            
        }
    }


    void UpdateWaiting()
    {
        // Waits before deciding where to go next
        currentWanderDelay += Time.deltaTime;

        if (currentWanderDelay >= wanderingDelay.Value)
        {
            DecideNextTargetPosition();
            currentState = WanderingState.wandering;
            currentWanderDelay = 0f;
        }
    }
    
    void DecideNextTargetPosition()
    {
        targetPosition = GetRandomPositionWithinRadius(spawnPosition, wanderRadius);
    }

    void UpdateWandering()
    {
        if ((transform.position - targetPosition).magnitude < stoppingDistance)
        {
            // If close enough to the target, starts waiting
            currentState = WanderingState.waiting;
            return;
        }
        
        // Else, moves!
        Move();
    }

    void Move()
    {
        Vector3 movementVector = (transform.position - targetPosition).normalized  * (wanderSpeed * Time.deltaTime);
        transform.Translate(movementVector);
    }

    Vector3 GetRandomPositionWithinRadius(Vector3 startingPoint, float radius)
    {
        Vector2 randomOffset = Random.insideUnitCircle;
        Vector3 randomPosition =  new Vector3(randomOffset.x * radius, 0f, randomOffset.y * radius);
        
        return startingPoint + randomPosition;
    }
    
    
}
