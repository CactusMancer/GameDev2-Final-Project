using System.Collections;
using System.Collections.Generic;
using Prototype.NetworkLobby;
using UnityEngine;
using UnityEngine.Networking;

public class LobbyPlayerHook : LobbyHook {
    public override void OnLobbyServerSceneLoadedForPlayer(NetworkManager manager, GameObject lobbyPlayer, GameObject gamePlayer) {
        LobbyPlayer lobby = lobbyPlayer.GetComponent<LobbyPlayer>();
        LocalPlayer player = gamePlayer.GetComponent<LocalPlayer>();

        player.playerName = lobby.playerName;
        player.playerTeam = lobby.playerColor;
    }
}
