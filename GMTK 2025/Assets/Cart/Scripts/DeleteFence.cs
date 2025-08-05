using UnityEngine;

public class DeleteFence : MonoBehaviour
{
    private bool hit = false;

    private void OnJointBreak(float breakForce)
    {
        if (!hit)
        {
            Destroy(gameObject, Random.Range(4f, 8f));
            hit = true;
        }
    }
}
