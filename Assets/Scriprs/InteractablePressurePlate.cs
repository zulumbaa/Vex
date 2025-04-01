using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InteractablePressurePlate : MonoBehaviour
{
    [Header("Options")]
    public float delayPressing = 0.1f;
    public bool resetOnLeaving;
    public float delayResettingOnLeaving = 0.1f; //Only works when LEAVING the pressure plate 
    public Sprite notPressedPlate;
    public Sprite pressedPlate;

    internal bool gettingPressed = false;

    bool pressed = false;
    bool enabledCol = true;
    float activatingTimer = 0;
    float disablingTimer = 0;

    [Header("Events")]

    public UnityEvent<bool> onGotPressed = new();
    public UnityEvent onCompletelyPressed = new();

    public void ResetPressurePlate()
    {
        bool lastgettingPressed = gettingPressed;
        OnDisable();
        gettingPressed = lastgettingPressed; //Prevent bug that reseting a pressure plate doesnt callback onGettingPressed(false) because gettingPressed is false when resetting
       
    }

    IEnumerator ResettingOnLeaving()
    {
        while (gettingPressed)
        {
            yield return null;
        }

        yield return new WaitForSeconds(delayResettingOnLeaving);

        if (gettingPressed) //Still pressing
        {
            yield return ResettingOnLeaving();
        }
        else
        {
            ResetPressurePlate();
        }
    }

    public void OnTriggerEnter2D(Collider2D collider2D)
    {
        
        if (!gettingPressed)
        {
            gettingPressed = true;
            if (enabledCol)
                onGotPressed.Invoke(gettingPressed);
        }  
    }


    public void OnTriggerStay2D(Collider2D collision)
    {
        if (!enabledCol) return;

        activatingTimer += Time.fixedDeltaTime;
        
        if(!pressed && activatingTimer >= delayPressing)
        {
            pressed = true;
            GetComponent<SpriteRenderer>().sprite = pressedPlate;
            onCompletelyPressed.Invoke();
        }
    }

    public void OnTriggerExit2D(Collider2D collision)
    {

        if (gettingPressed)
        {
            gettingPressed = false;

            if (enabledCol)
            {
                onGotPressed.Invoke(gettingPressed);

                if (resetOnLeaving)
                {
                    enabledCol = false;
                    StartCoroutine(ResettingOnLeaving());
                }
            }

            activatingTimer = 0;



        }
    }

    private void OnDisable()
    {
        activatingTimer = 0;
        pressed = false;
        gettingPressed = false;
        enabledCol = true;
        GetComponent<SpriteRenderer>().sprite = notPressedPlate;
    }
}
