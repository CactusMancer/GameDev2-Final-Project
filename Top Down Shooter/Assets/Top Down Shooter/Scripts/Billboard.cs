using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billboard : MonoBehaviour {
    Quaternion rot;

    private void Awake() {
        rot = transform.rotation;
    }

    private void LateUpdate() {
        transform.rotation = Quaternion.Euler(75f, 0f, 0f);
    }
}
