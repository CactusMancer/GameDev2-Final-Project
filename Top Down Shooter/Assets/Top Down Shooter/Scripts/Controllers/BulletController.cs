using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class BulletController : NetworkBehaviour {

    [SyncVar]
    public int damage;

    [SyncVar]
    public float lifetime;

    [SyncVar]
    public float speed;

    [SyncVar]
    public Color color = Color.white;

    private void Awake() {
        // Todo
    }

    private void Update() {
        if (hasAuthority) {
            CmdChangeColor();
            CmdUpdatePos();
        }

        lifetime -= Time.deltaTime;

        if (lifetime <= 0) {
            CmdDestroyBullet();
        }
    }

    private void OnCollisionEnter(Collision collision) {
        if (!(collision.gameObject.GetComponent<Renderer>().material.color == color)) {
            CmdDestroyBullet();
        }
    }

    [Command]
    private void CmdDestroyBullet() {
        NetworkServer.Destroy(gameObject);
    }

    [Command]
    private void CmdUpdatePos() {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }

    [Command]
    private void CmdChangeColor() {
        this.gameObject.GetComponent<Renderer>().material.color = color;
        RpcChangeColor();
    }

    [ClientRpc]
    private void RpcChangeColor() {
        this.gameObject.GetComponent<Renderer>().material.color = color;
    }
}
