using UnityEngine;

public class ResortLocation : MonoBehaviour
{
   [SerializeField] private LocationType locationType;
   
   public LocationType GetLocationType()
    {
        return locationType;
    }

}
public enum LocationType{
   Lobby,
   Villas,
   Beach,
   Restaurants
}
