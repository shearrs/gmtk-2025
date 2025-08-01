using UnityEngine;
using UnityEngine.AI;

namespace LostResort.Passengers
{
    public class PassengerNavigation : MonoBehaviour
    {


        public float wanderRadius = 1f;
        public float wanderTimer = 5f;

        private NavMeshAgent agent;
        private float timer;

        void Start()
        {
            agent = GetComponent<NavMeshAgent>();
            timer = wanderTimer;
        }

        void Update()
        {
            timer += Time.deltaTime;

            if (timer >= wanderTimer)
            {
                Vector3 newPos = GetRandomNavMeshLocation(wanderRadius);
                //agent.SetDestination(newPos);
                timer = 0;
            }
        }

        public void GoToLocation(Vector3 location)
        {
            
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
