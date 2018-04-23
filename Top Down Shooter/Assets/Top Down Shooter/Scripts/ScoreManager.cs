using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class ScoreManager : NetworkBehaviour {

    //[SyncVar (hook = "OnChangeScore")]
    [SyncVar]
    public int redScore = 0;

    //[SyncVar (hook = "OnChangeScore")]
    [SyncVar]
    public int blueScore = 0;

    [SerializeField]
    private int winningScore = 100;

    [SyncVar]
    private bool winner = false;

    [SerializeField]
    private Text redText;

    [SerializeField]
    private Text blueText;

    [SerializeField]
    private RectTransform winGUI;

    [SerializeField]
    private Text winText;

    private void Start() {
        redText.text = redScore + "";
        blueText.text = blueScore + "";
    }

    private void Update() {
        if (redScore >= winningScore && redScore != blueScore && winner == false) {
            winner = true;
            CmdAnnounceWinners("Red");
        }
        if (blueScore >= winningScore && redScore != blueScore && winner == false) {
            winner = true;
            CmdAnnounceWinners("Blue");
        }

        OnChangeScore();
    }

    public void updateBlue(int score) {
        if (winner == false) {
            blueScore += score;
        }
    }

    public void updateRed(int score) {
        if (winner == false) {
            redScore += score;
        }
    }

    private void OnChangeScore() {
        redText.text = redScore + "";
        blueText.text = blueScore + "";
    }

    [Command]
    public void CmdAnnounceWinners(string winner) {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");

        winText.text = winner + " Team Wins!";
        winGUI.gameObject.SetActive(true);

        foreach (GameObject p in players) {
            p.gameObject.SetActive(false);
        }

        RpcAnnounceWinners(winner, players);
    }

    [ClientRpc]
    public void RpcAnnounceWinners(string winner, GameObject[] players) {
        winText.text = winner + " Team Wins!";
        winGUI.gameObject.SetActive(true);

        foreach (GameObject p in players) {
            p.gameObject.SetActive(false);
        }
    }
}
