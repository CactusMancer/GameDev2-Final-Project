using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Objective : NetworkBehaviour {

    [SyncVar]
    public string currentCapture = "None";

    [SyncVar]
    public float captureTimer = 5;

    public float maxCaptureTimer = 5;

    [SyncVar]
    public float rewardTimer = 5;

    public float maxRewardTimer = 5;

    [SyncVar]
    public bool contested = false;

    [SyncVar]
    public bool capturing = false;

    private List<GameObject> collisions = new List<GameObject>();

    private Color previousColor = Color.white;

    private void Update() {
        CheckCapture();

        if (currentCapture != "None" && contested == false && capturing == false) {
            rewardTimer -= Time.deltaTime;

            if (rewardTimer <= 0) {
                rewardTimer = maxRewardTimer;
                RewardTeam();
            }
        }
        else {
            rewardTimer = maxRewardTimer;
        }
    }

    private void CheckCapture() {
        int red = 0;
        int blue = 0;

        foreach (GameObject c in collisions) {
            if (c.GetComponent<Renderer>().material.color == Color.red) {
                red++;
            }
            if (c.GetComponent<Renderer>().material.color == Color.blue) {
                blue++;
            }
        }

        if (red > 0 && blue == 0 && currentCapture != "Red") {
            capturing = true;
            contested = false;
            captureTimer -= Time.deltaTime;
            CmdRecolor(Color.white);

            if (captureTimer <= 0) {
                currentCapture = "Red";
                previousColor = Color.red;
            }
        }
        else if (blue > 0 && red == 0 && currentCapture != "Blue") {
            capturing = true;
            contested = false;
            captureTimer -= Time.deltaTime;
            CmdRecolor(Color.white);

            if (captureTimer <= 0) {
                currentCapture = "Blue";
                previousColor = Color.blue;
            }
        }
        else {
            capturing = false;
            captureTimer = maxCaptureTimer;
            if (red > 0 && blue > 0) {
                contested = true;
                CmdRecolor(Color.black);
            }
            else {
                contested = false;
                CmdRecolor(previousColor);
            }
        }
    }

    private void RewardTeam() {
        ScoreManager scoreboard = FindObjectOfType<ScoreManager>();

        if (currentCapture == "Blue") {
            scoreboard.updateBlue(1);
        }

        if (currentCapture == "Red") {
            scoreboard.updateRed(1);
        }
    }

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.tag == "Player") {
            collisions.Add(other.gameObject);
        }
    }

    private void OnTriggerExit(Collider other) {
        if (other.gameObject.tag == "Player") {
            collisions.Remove(other.gameObject);
        }
    }

    [Command]
    private void CmdRecolor(Color color) {
        this.gameObject.GetComponent<Renderer>().material.color = color;
        RpcRecolor(color);
    }

    [ClientRpc]
    private void RpcRecolor(Color color) {
        this.gameObject.GetComponent<Renderer>().material.color = color;
    }
}
