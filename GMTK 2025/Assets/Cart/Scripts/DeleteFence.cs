using UnityEngine;

public class DeleteFence : MonoBehaviour
{
    private float deleteTimer;
    private bool hit;
    private float deleteTime;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        hit = false;
        deleteTimer = 0;
        deleteTime = Random.Range(4f, 8f);
    }

    // Update is called once per frame
    void Update()
    {
        if(hit)
        {
            deleteTimer += Time.deltaTime;
        }

        if(deleteTimer > deleteTime)
        {
            Destroy(gameObject);
        }
    }

    private void OnJointBreak(float breakForce)
    {
        hit = true;
    }
}
