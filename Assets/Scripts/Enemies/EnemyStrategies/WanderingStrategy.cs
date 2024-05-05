using Interfaces;
using Unity.Netcode;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Enemies.EnemyStrategies
{
    public class WanderingStrategy : NetworkBehaviour, IEnemyStrategy
    {
        // Enemy Strategy. Wanders around spawn position
        private enum WanderingState
        {
            Wandering,
            Waiting
        }
    
        [SerializeField]
        private NetworkVariable<float> wanderingDelay =
            new NetworkVariable<float>(4f, NetworkVariableReadPermission.Everyone);
        [SerializeField] private float wanderRadius = 12f;
        [SerializeField] private float wanderSpeed = 3f;
        [SerializeField] private float stoppingDistance = 0.3f;
    
        private WanderingState _currentState = WanderingState.Waiting;
        private float _currentWanderDelay = 2f;
        private Vector3 _spawnPosition;
        private Vector3 _targetPosition;
    
        private void Awake()
        {
            _spawnPosition = transform.position;
        }
    
        public void UpdateStrategy()
        {
            if (!IsServer) return;
        
            switch (_currentState)
            {

                case WanderingState.Waiting:
                    UpdateWaiting();
                    break;
                case WanderingState.Wandering:
                    UpdateWandering();
                    break;
            
            }
        }


        void UpdateWaiting()
        {
            // Waits before deciding where to go next
            _currentWanderDelay += Time.deltaTime;

            if (_currentWanderDelay >= wanderingDelay.Value)
            {
                DecideNextTargetPosition();
                _currentState = WanderingState.Wandering;
                _currentWanderDelay = 0f;
            }
        }
    
        void DecideNextTargetPosition()
        {
            _targetPosition = GetRandomPositionWithinRadius(_spawnPosition, wanderRadius);
        }

        private void UpdateWandering()
        {
            if ((transform.position - _targetPosition).magnitude < stoppingDistance)
            {
                // If close enough to the target, starts waiting
                _currentState = WanderingState.Waiting;
                return;
            }
        
            // Else, moves!
            Move();
        }

        private void Move()
        {
            Vector3 movementVector = (transform.position - _targetPosition).normalized  * (wanderSpeed * Time.deltaTime);
            transform.Translate(movementVector);
        }

        private Vector3 GetRandomPositionWithinRadius(Vector3 startingPoint, float radius)
        {
            Vector2 randomOffset = Random.insideUnitCircle;
            Vector3 randomPosition =  new Vector3(randomOffset.x * radius, 0f, randomOffset.y * radius);
        
            return startingPoint + randomPosition;
        }
    }
}
