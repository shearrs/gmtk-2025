using UnityEngine;

public class GateAnimation : MonoBehaviour
{
    public Animator animator;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        animator.speed = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void playAnimation()
    {
        animator.speed = 1;
        Debug.Log("Called");
    }
}
