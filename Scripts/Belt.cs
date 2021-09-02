using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class Belt : MonoBehaviour
{
    public Transform BeltPos;
    public Vector3 offset;

    private void Update()
    {
        transform.position = BeltPos.position + Vector3.up * offset.y
            + Vector3.ProjectOnPlane(BeltPos.right, Vector3.up).normalized * offset.x
            + Vector3.ProjectOnPlane(BeltPos.forward, Vector3.up).normalized*offset.z;

        transform.eulerAngles = new Vector3(0, BeltPos.eulerAngles.y, 0);
    }
}
