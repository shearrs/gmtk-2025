using UnityEngine;

public class GateAnimation : MonoBehaviour
{
    public Animator animator;
    public bool toggleable;
    private int speed;
    private int dir;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        speed = 1;
        animator.speed = 0;
        dir = -1;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void playAnimation()
    {
        animator.speed = speed;
    }
}
