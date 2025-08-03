using LostResort.Interaction;
using UnityEngine;

public class PowerPole : MonoBehaviour
{
    public Interactable interactScript;
    
    public GameObject thingToTrigger1;
    public GameObject thingToTrigger2;
    public GameObject thingToTrigger3;

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
        Debug.Log("DO SOMETHING");
        if(thingToTrigger1 != null)
        {
            thingToTrigger1.GetComponent<GateAnimation>().playAnimation();
        }
        if (thingToTrigger2 != null)
        {
            thingToTrigger2.GetComponent<GateAnimation>().playAnimation();
        }
        if (thingToTrigger3 != null)
        {

        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created

}
