using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class MovementController : NetworkBehaviour {

    public float moveSpeed;
    public GunController gun;

    private Rigidbody rb;
    private Vector3 moveDirection, moveVelocity;
    private Camera cam;

    private void Start() {
        rb = GetComponent<Rigidbody>();
        cam = FindObjectOfType<Camera>();
    }

    private void Update() {
        if (isLocalPlayer) {
            bool dead = this.gameObject.GetComponent<LocalPlayer>().isDead;

            if (!dead) {
                float hAxis = Input.GetAxisRaw("Horizontal");
                float vAxis = Input.GetAxisRaw("Vertical");
                float rayLength;

                moveDirection = new Vector3(hAxis, 0f, vAxis);
                moveVelocity = moveDirection * moveSpeed;

                Ray camRay = cam.ScreenPointToRay(Input.mousePosition);
                Plane ground = new Plane(Vector3.up, Vector3.zero);

                if (ground.Raycast(camRay, out rayLength)) {
                    Vector3 lookPoint = camRay.GetPoint(rayLength);
                    Debug.DrawLine(camRay.origin, lookPoint, Color.blue);

                    CmdLookAtCursor(new Vector3(lookPoint.x, transform.position.y, lookPoint.z));
                }

                if (Input.GetMouseButtonDown(0)) {
                    gun.isFiring = true;
                }
                if (Input.GetMouseButtonUp(0)) {
                    gun.isFiring = false;
                }
            }
        }
    }

    private void FixedUpdate() {
        rb.velocity = moveVelocity;
    }

    [Command]
    private void CmdLookAtCursor(Vector3 point) {
        transform.LookAt(point);
        RpcLookAtCursor(point);
    }

    [ClientRpc]
    private void RpcLookAtCursor(Vector3 point) {
        transform.LookAt(point);
    }

    public void SetTeamColor(Color color) {
        gun.bulletColor = color;
    }
}
