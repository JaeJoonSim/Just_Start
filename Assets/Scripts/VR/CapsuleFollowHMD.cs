using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;
using Unity.XR.CoreUtils;

public class CapsuleFollowHMD : MonoBehaviour
{
    public float additionalHeight = 0.2f;

    private CharacterController character;
    private XROrigin xrOrigin;

    // Start is called before the first frame update
    void Start()
    {
        character = GetComponent<CharacterController>();
        xrOrigin = GetComponent<XROrigin>();
    }

    void FixedUpdate()
    {
        CapsuleFollowHeadset();
    }

    void CapsuleFollowHeadset()
    {
        character.height = xrOrigin.CameraInOriginSpaceHeight + additionalHeight;
        Vector3 capsuleCenter = transform.InverseTransformPoint(xrOrigin.Camera.transform.position);
        character.center = new Vector3(capsuleCenter.x, character.height / 2 + character.skinWidth, capsuleCenter.z);
    }
}
