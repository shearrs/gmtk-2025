using LostResort.Score;
using LostResort.SignalShuttles;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace LostResort.Passengers
{
    public class Passenger : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private NavMeshAgent agent;
        [SerializeField] private Rigidbody rb;
        [SerializeField] private Collider col;

        [Header("Settings")]
        [SerializeField] private float waitingTime = 30.0f;
        [SerializeField] private int scoreValue = 100;

        [Header("Clothes Slots")]
        [SerializeField] private Transform hatSlot;
        [SerializeField] private Transform faceSlot;
        [SerializeField] private Transform neckSlot;
        [SerializeField] private Transform handSlot;
        [SerializeField] private Transform[] wristSlots;
        [SerializeField] private Transform waistSlot;
        [SerializeField] private Transform feetSlot;

        private ResortLocation originLocation;
        private ResortLocation targetLocation;

        public bool IsPickedUp { get; private set; } = false;

        public static Passenger Create(Passenger prefab, ResortLocation originLocation, ResortLocation targetLocation)
        {
            var passenger = Instantiate(prefab, originLocation.GetPickupPosition(), Quaternion.identity);
            passenger.originLocation = originLocation;
            passenger.targetLocation = targetLocation;
            passenger.SetAccessories();

            passenger.agent.SetDestination(originLocation.GetPickupPosition());

            return passenger;
        }

        private void SetAccessories()
        {
            int random = Random.Range(1, targetLocation.Accessories.Count);

            for (int i = 0; i < random; i++)
            {
                int accessoryIndex = Random.Range(0, targetLocation.Accessories.Count);

                var accessory = targetLocation.Accessories[accessoryIndex];
                var instance = Instantiate(accessory);
                var slot = GetSlotForAccessory(instance);
                instance.transform.parent = slot;
                instance.transform.localPosition = Vector3.zero;

                if (instance.Type == Accessory.AccessoryType.Wrist)
                {
                    var secondInstance = Instantiate(accessory);
                    var secondSlot = wristSlots[1];
                    secondInstance.transform.parent = secondSlot;
                    secondInstance.transform.localPosition = Vector3.zero;
                }
            }
        }

        public void SetAnchor(FixedJoint joint)
        {
            rb.position = joint.transform.position;
            rb.rotation = joint.transform.rotation;
            rb.PublishTransform();
            joint.connectedBody = rb;
        }

        public void OnPickup()
        {
            IsPickedUp = true;
            agent.enabled = false;
            rb.useGravity = false;
            rb.interpolation = RigidbodyInterpolation.Interpolate;
            col.enabled = false;
        }

        public void OnDropoff()
        {
            IsPickedUp = false;
            SignalShuttle.Emit(new AddScoreSignal(scoreValue));
            Destroy(gameObject);
        }

        public Transform GetSlotForAccessory(Accessory accessory)
        {
            return (accessory.Type) switch
            {
                Accessory.AccessoryType.Hat => hatSlot,
                Accessory.AccessoryType.Face => faceSlot,
                Accessory.AccessoryType.Neck => neckSlot,
                Accessory.AccessoryType.Hand => handSlot,
                Accessory.AccessoryType.Wrist => wristSlots[0],
                Accessory.AccessoryType.Waist => waistSlot,
                Accessory.AccessoryType.Feet => feetSlot,
                _ => null
            };
        }
    }
}
