using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

[RequireComponent(typeof(XRGrabInteractable))]
public class PlayEffect : MonoBehaviour
{
    XRGrabInteractable GrabInteractable;

    [SerializeField] ParticleSystem Particle = null;

    const float k_HeldThreshold = 0.1f;


    float TriggerHoldTime;
    bool triggerDown;

    void Start()
    {
        GrabInteractable = GetComponent<XRGrabInteractable>();
        GrabInteractable.selectExited.AddListener(DroppedCube);
        GrabInteractable.activated.AddListener(TriggerPulled);
        GrabInteractable.deactivated.AddListener(TriggerReleased);

    }

    void Update()
    {
        if (triggerDown)
        {
            TriggerHoldTime += Time.deltaTime;

            if(TriggerHoldTime >= k_HeldThreshold)
            {
                if (!Particle.isPlaying)
                {
                    Particle.Play();
                }
            }
        }
    }

    void TriggerReleased(DeactivateEventArgs args)
    {
        triggerDown = false;
        TriggerHoldTime = 0f;
        Particle.Stop();
    }

    void TriggerPulled(ActivateEventArgs args)
    {
        triggerDown = true;
    }

    void DroppedCube(SelectExitEventArgs args)
    {
        Particle.Stop();
    }

    void ShootEvent()
    {
        Particle.Emit(1);
    }
}
