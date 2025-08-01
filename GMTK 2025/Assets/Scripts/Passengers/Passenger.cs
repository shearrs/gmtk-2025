using LostResort.Score;
using LostResort.SignalShuttles;
using UnityEngine;
using UnityEngine.AI;

namespace LostResort.Passengers
{
    public class Passenger : MonoBehaviour
    {
        public PassengerData passengerData { get; private set; }

        private bool inShuttle;
        private MeshRenderer meshRenderer; 
        private CapsuleCollider capsuleCollider;
        private NavMeshAgent navMeshAgent;


        [SerializeField] private ExclamationMark exclamationMark;

        //should be changed to use the event bus (signal shuttle)
        private Score.Score score;

        [SerializeField] private float startingScoreWhenDroppedOff;

        [SerializeField] private float scoreWhenDroppedOffReductionRatePerSecond;

        [SerializeField] private float scoreWhenDroppedOff;
        
        [SerializeField] private float minimumScoreWhenDroppedOff;


        [SerializeField] private Material[] dropOffLocationBasedMaterials;

        private void Awake()
        {
            navMeshAgent = GetComponent<NavMeshAgent>();
            score = FindAnyObjectByType<Score.Score>();
            capsuleCollider = GetComponent<CapsuleCollider>();
            meshRenderer = GetComponent<MeshRenderer>();

            scoreWhenDroppedOff = startingScoreWhenDroppedOff;
            exclamationMark.InitializeStartingScoreWhenDroppedOff(startingScoreWhenDroppedOff);
        }

        void Update()
        {
            if (inShuttle)
            {
                return;
            }
            AdjustScoreWhenDroppedOff();
        }


        /// <summary>
        /// Adjusts the scoreWhenDroppedOffVariable based on how much time has elapsed during the last frame.
        /// Also sends this information to the child exclamationMark object.
        /// </summary>
        private void AdjustScoreWhenDroppedOff()
        {
            float secondsPassedSinceLastFrame = Time.deltaTime;
            scoreWhenDroppedOff -= secondsPassedSinceLastFrame * scoreWhenDroppedOffReductionRatePerSecond;
            scoreWhenDroppedOff = Mathf.Clamp(scoreWhenDroppedOff, minimumScoreWhenDroppedOff, scoreWhenDroppedOff);
            exclamationMark.ReceiveScoreWhenDroppedOff(scoreWhenDroppedOff);
        }


        public void InitializeLocation(LocationType locationType)
        {
            passengerData = new PassengerData(locationType);
            meshRenderer.material = dropOffLocationBasedMaterials[(int)passengerData.dropOffLocation];
        }

        /// <summary>
        /// Picks up the passenger. This is called from some player behavior script. The passenger should be saved in a list of passengers which is stored in the player.
        /// This script, or where its called, should hide the passenger (or have them join the shuttle).
        /// It also changed the boolean 'inShuttle' to false.
        /// </summary>
        public void PickUp()
        {
            //Debug.Log($"Picked up a player from {passengerData.startingLocation} who intends to travel to {passengerData.dropOffLocation}!");
            meshRenderer.enabled = false;
            exclamationMark.DisableExclamationMark();
            capsuleCollider.enabled = false;
            navMeshAgent.enabled = true;
            inShuttle = true;
        }


        public void DropOff()
        {
            Debug.Log($"Dropped off a player at {passengerData.dropOffLocation} who originally came from {passengerData.startingLocation}!");
            inShuttle = false;
            IncrementScore((int)scoreWhenDroppedOff);
            Destroy(gameObject);

            //increase score
        }

        private void IncrementScore(int additionalScore)
        {
            SignalShuttle.Emit(new AddScoreSignal(additionalScore));
        }
    }
}