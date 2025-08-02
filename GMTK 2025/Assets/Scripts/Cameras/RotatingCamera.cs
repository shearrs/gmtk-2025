using UnityEngine;

namespace LostResort.Cameras
{
    public class RotatingCamera : MonoBehaviour
    {
      
        public float speed;
        // Update is called once per frame
        void Update()
        {
            transform.Rotate(0, speed * Time.deltaTime, 0);
        }
    }
}
