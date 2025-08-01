using UnityEngine;

namespace LostResort.Passengers
{
    public class ResortLocation : MonoBehaviour
    {
        [SerializeField] private LocationType locationType;

        public LocationType GetLocationType()
        {
            return locationType;
        }

    }

    public enum LocationType
    {
        Hotel,
        Conference,
        Beach

    }
}