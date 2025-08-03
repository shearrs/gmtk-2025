using LostResort.Interaction;
using UnityEngine;

public class PowerPole : MonoBehaviour
{
    public Interactable interactScript;
    
    public GameObject thingToTrigger1;
    public GameObject thingToTrigger2;
    public GameObject thingToTrigger3;
    public GameObject thingToTrigger4;
    public GameObject thingToTrigger5;
    public GameObject thingToTrigger6;

    public bool bridge;
    private bool toggle = false;

    private void OnEnable()
    {
        interactScript.Interacted += OnInteracted;
    }

    private void OnDisable()
    {
        interactScript.Interacted -= OnInteracted;
    }

    private void OnInteracted()
    {
        if (thingToTrigger1 != null)
        {
            thingToTrigger1.GetComponent<GateAnimation>().playAnimation();
        }
        if (thingToTrigger2 != null)
        {
            thingToTrigger2.GetComponent<GateAnimation>().playAnimation();
        }
        if (thingToTrigger3 != null)
        {
            thingToTrigger3.GetComponent<GateAnimation>().playAnimation();
        }
        if (thingToTrigger4 != null)
        {
            thingToTrigger4.GetComponent<GateAnimation>().playAnimation();
        }
        if (thingToTrigger5 != null)
        {
            thingToTrigger5.GetComponent<GateAnimation>().playAnimation();
        }
        if (thingToTrigger6 != null)
        {
            thingToTrigger6.GetComponent<GateAnimation>().playAnimation();
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created

}
