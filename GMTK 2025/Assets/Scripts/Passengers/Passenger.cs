using LostResort.Score;
using LostResort.SignalShuttles;
using Shears;
using System.Collections;
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
        [SerializeField] private SkinnedMeshRenderer mesh;

        [Header("Settings")]
        [SerializeField] private bool isMale = true;
        [SerializeField] private float waitingTime = 30.0f;
        [SerializeField] private int scoreValue = 100;

        [Header("Skin Tones")]
        [SerializeField] private Material[] skinMaterials;

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
        public ResortLocation TargetLocation => targetLocation;

        public static Passenger Create(Passenger prefab, ResortLocation originLocation, ResortLocation targetLocation)
        {
            var passenger = Instantiate(prefab, originLocation.GetPickupPosition(), Quaternion.identity);
            passenger.originLocation = originLocation;
            passenger.targetLocation = targetLocation;
            passenger.SetAccessories();
            passenger.SetMaterials();

            passenger.agent.SetDestination(originLocation.GetPickupPosition());
            passenger.BeginWaitingTime();

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

        private void SetMaterials()
        {
            var materialList = isMale ? targetLocation.MaleMaterials : targetLocation.FemaleMaterials;
            var materials = mesh.materials;
            
            int clothesRandom = Random.Range(0, materialList.Count);
            materials[0] = materialList[clothesRandom];

            int skinRandom = Random.Range(0, skinMaterials.Length);
            materials[1] = skinMaterials[skinRandom];

            mesh.materials = materials;
        }

        private void BeginWaitingTime()
        {
            StartCoroutine(IEWaitingTime());
        }
        
        private IEnumerator IEWaitingTime()
        {
            yield return CoroutineUtil.WaitForSeconds(waitingTime);

            agent.SetDestination(targetLocation.GetDropoffPosition());

            while (!agent.isStopped)
                yield return null;

            Destroy(gameObject);
        }

        public void SetAnchor(Joint joint)
        {
            rb.position = joint.transform.position;
            rb.rotation = joint.transform.rotation;
            rb.PublishTransform();
            joint.connectedBody = rb;
        }

        public void OnPickup()
        {
            StopAllCoroutines();

            IsPickedUp = true;
            agent.enabled = false;
            rb.useGravity = false;
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
