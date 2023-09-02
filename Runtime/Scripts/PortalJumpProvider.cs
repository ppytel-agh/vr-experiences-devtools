using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class PortalJumpProvider : LocomotionProvider
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        switch(locomotionPhase)
        {
            case LocomotionPhase.Idle:
                //warunki triggeru: select/focus portalu

                break;
            case LocomotionPhase.Started:
                //wywo³anie asynchronicznego loada
            case LocomotionPhase.Moving:
                //odczyt stanu Async
            case LocomotionPhase.Done:
                EndLocomotion();
                locomotionPhase = LocomotionPhase.Idle;
                break;
        }
    }
}
