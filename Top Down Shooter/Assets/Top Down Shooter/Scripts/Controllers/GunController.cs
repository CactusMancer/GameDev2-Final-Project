using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class GunController : NetworkBehaviour {

    public bool isFiring;
    public int bulletDamage;
    public float bulletSpeed;
    public float bulletTimer;
    public float shotTime;
    public Transform barrel;
    public GameObject bullet;
    public Color bulletColor = Color.white;

    private float shotCounter;
	
	private void Update () {
        if (isLocalPlayer) {
            if (isFiring) {
                shotCounter -= Time.deltaTime;

                if (shotCounter <= 0) {
                    shotCounter = shotTime;
                    CmdSpawnBullet();
                }
            }

            else {
                shotCounter = 0;
            }
        }
        else {
            shotCounter = 0;
        }
	}

    [Command]
    private void CmdSpawnBullet() {
        GameObject newBullet = Instantiate(bullet, barrel.position, barrel.rotation) as GameObject;
        newBullet.GetComponent<BulletController>().speed = bulletSpeed;
        newBullet.GetComponent<BulletController>().color = bulletColor;
        newBullet.GetComponent<BulletController>().damage = bulletDamage;
        newBullet.GetComponent<BulletController>().lifetime = bulletTimer;

        NetworkServer.Spawn(newBullet);
    }
}
