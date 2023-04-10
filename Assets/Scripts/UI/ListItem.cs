using TMPro;
using UnityEngine;
using Photon.Realtime;
using Photon.Pun;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class ListItem : MonoBehaviour
{
    [SerializeField] private TMP_Text _nickText;
    [SerializeField] private TMP_Text _playerCountText;

    private Button _button;

    private void Awake()
    {
        _button = GetComponent<Button>();
    }

    private void OnEnable()
    {
        _button.onClick.AddListener(JoinToListRoom);
    }

    private void OnDisable()
    {
        _button.onClick.RemoveListener(JoinToListRoom);
    }

    public void SetInfo(RoomInfo roomInfo)
    {
        _nickText.text = roomInfo.Name;
        _playerCountText.text = roomInfo.PlayerCount + " / " + roomInfo.MaxPlayers;
    }

    public void JoinToListRoom()
    {
        PhotonNetwork.JoinRoom(_nickText.text);
    }
}
