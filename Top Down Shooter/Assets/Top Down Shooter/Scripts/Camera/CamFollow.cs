using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class CamFollow : NetworkBehaviour {

    [HideInInspector]
    public Transform target;

    [SerializeField]
    private float smoothing;

    [SerializeField]
    private Vector3 offset;

    private void FixedUpdate() {
        Vector3 desiredPos = target.position + offset;
        Vector3 smoothedPos = Vector3.Lerp(transform.position, desiredPos, smoothing);
        transform.position = smoothedPos;
    }
}
