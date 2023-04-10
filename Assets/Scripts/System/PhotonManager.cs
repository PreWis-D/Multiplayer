using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using TMPro;

public class PhotonManager : MonoBehaviourPunCallbacks
{
    [SerializeField] private MainMenu _menu;
    [SerializeField] private string _region;
    [SerializeField] private byte _maxPlayerCount;

    private List<RoomInfo> _allRoomsInfo = new List<RoomInfo>();

    private new void OnEnable()
    {
        base.OnEnable();
        _menu.CreateRoomClicked += OnCreateRoomClicked;
        _menu.RandomRoomClicked += OnRandomRoomClicked;
        _menu.JoinRoomClicked += OnJoinRoomClicked;
    }

    private new void OnDisable()
    {
        base.OnDisable();
        _menu.CreateRoomClicked -= OnCreateRoomClicked;
        _menu.RandomRoomClicked -= OnRandomRoomClicked;
        _menu.JoinRoomClicked -= OnJoinRoomClicked;
    }

    private void Start()
    {
        PhotonNetwork.ConnectUsingSettings();
        PhotonNetwork.ConnectToRegion(_region);
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("Подключены " + PhotonNetwork.CloudRegion);
        PhotonNetwork.JoinLobby();
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.Log("Отключены");
    }

    public void OnCreateRoomClicked()
    {
        if (!PhotonNetwork.IsConnected)
            return;

        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = _maxPlayerCount;

        PhotonNetwork.CreateRoom(_menu.InputField.text, roomOptions, TypedLobby.Default);
        PhotonNetwork.LoadLevel("Main");
    }

    public void OnJoinRoomClicked()
    {
        PhotonNetwork.JoinRoom(_menu.InputField.text);
    }

    public void OnRandomRoomClicked()
    {
        PhotonNetwork.JoinRandomRoom();
    }

    public override void OnCreatedRoom()
    {
        Debug.Log("Комната " + PhotonNetwork.CurrentRoom.Name);
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.Log("Не удалось создать");
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        foreach (var info in roomList)
        {
            for (int i = 0; i < _allRoomsInfo.Count; i++)
            {
                Debug.Log(info.masterClientId + " /////////// " + _allRoomsInfo[i].masterClientId);
                if (_allRoomsInfo[i].masterClientId == info.masterClientId)
                {
                    return;
                }
            }

            ListItem listItem = Instantiate(_menu.ItemPrefab, _menu.Content);

            if (listItem != null)
            {
                listItem.SetInfo(info);
                _allRoomsInfo.Add(info);
            }
        }
    }

    public override void OnJoinedRoom()
    {
        PhotonNetwork.LoadLevel("Main");
    }
}
