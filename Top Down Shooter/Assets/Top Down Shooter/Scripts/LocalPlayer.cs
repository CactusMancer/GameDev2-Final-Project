using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class LocalPlayer : NetworkBehaviour {

    [SyncVar]
    public string playerName = "Player";

    [SyncVar]
    public Color playerTeam = Color.white;

    //[SyncVar(hook = "OnChangeHP")]
    [SyncVar]
    public int hp = 50;

    public int maxHp = 50;

    [SyncVar]
    public float deathTimer = 5;

    public float maxDeathTimer = 5;

    [SerializeField]
    private RectTransform hpBar;

    [SerializeField]
    private Text nameText;

    [SyncVar]
    public bool isDead = false;

    private NetworkStartPosition[] spawnPoints;

    public override void OnStartLocalPlayer() {
        Camera.main.GetComponent<CamFollow>().target = this.gameObject.transform;
    }

    private void Start() {
        this.gameObject.GetComponent<Renderer>().material.color = playerTeam;
        this.gameObject.GetComponent<MovementController>().SetTeamColor(playerTeam);
        spawnPoints = FindObjectsOfType<NetworkStartPosition>();
        nameText.text = playerName;
    }

    private void Update() {
        if (hp <= 0) {
            isDead = true;
            hp = maxHp;
            ScoreUpdate();
            CmdDespawn();
        }
        if (isDead == true) {
            deathTimer -= Time.deltaTime;
        }
        if (deathTimer <= 0) {
            isDead = false;

            deathTimer = maxDeathTimer;

            CmdRespawn();
        }

        OnChangeHP();
    }

    private void OnCollisionEnter(Collision collision) {
        if (collision.gameObject.tag == "Stray Bullet") {
            BulletController bullet = collision.gameObject.GetComponent<BulletController>();

            if (bullet.color != playerTeam && isDead == false) {
                hp -= bullet.damage;
            }
        }
    }

    private void ScoreUpdate() {
        ScoreManager scoreboard = FindObjectOfType<ScoreManager>();

        if (playerTeam == Color.red) {
            scoreboard.updateBlue(5);
        }

        if (playerTeam == Color.blue) {
            scoreboard.updateRed(5);
        }
    }

    private void OnChangeHP() {
        hpBar.sizeDelta = new Vector2(hp * 4, hpBar.sizeDelta.y);
    }

    [Command]
    private void CmdDespawn() {
        this.gameObject.transform.Rotate(0, 0, 90);

        RpcDespawn();
    }

    [ClientRpc]
    private void RpcDespawn() {
        this.gameObject.transform.Rotate(0, 0, 90);
    }

    [Command]
    private void CmdRespawn() {
        Vector3 spawnPoint = new Vector3(0, 3.5f, 0);

        if (spawnPoints != null && spawnPoints.Length > 0) {
            spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)].transform.position;
        }

        transform.position = spawnPoint;
        this.gameObject.transform.Rotate(0, 0, -90);
        RpcRespawn(spawnPoint);
    }

    [ClientRpc]
    private void RpcRespawn(Vector3 spawnPoint) {
        transform.position = spawnPoint;
        this.gameObject.transform.Rotate(0, 0, -90);
    }
}