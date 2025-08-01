using UnityEngine;
using UnityEngine.AI;

namespace LostResort.Passengers
{
    public class PassengerNavigation : MonoBehaviour
    {


        private float wanderRadius = 1f;
        private float wanderTimer = 5f;

        [SerializeField]
        private float wanderRadiusMin;
        [SerializeField]
        private float wanderRadiusMax;
        
        [SerializeField]
        private float wanderTimerMin;
        [SerializeField]
        private float wanderTimerMax;

        private NavMeshAgent agent;
        private float timer;

        void Start()
        {
            
            agent = GetComponent<NavMeshAgent>();
            timer = wanderTimer;
            wanderRadius = Random.Range(wanderRadiusMin, wanderRadiusMax);
            wanderTimer = Random.Range(wanderTimerMin, wanderTimerMax);
        }

        void Update()
        {
            timer += Time.deltaTime;

            if (timer >= wanderTimer)
            {
                Vector3 newPos = GetRandomNavMeshLocation(wanderRadius);
                agent.SetDestination(newPos);
                timer = 0;
            }
        }

        Vector3 GetRandomNavMeshLocation(float radius)
        {
            Vector3 randomDirection = Random.insideUnitSphere * radius;
            randomDirection += transform.position;
            if (NavMesh.SamplePosition(randomDirection, out NavMeshHit hit, radius, NavMesh.AllAreas))
            {
                return hit.position;
            }
            return transform.position;
        }
    }
}
