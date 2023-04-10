using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private TMP_InputField _inputField;
    [SerializeField] private Transform _content;
    [SerializeField] private ListItem _itemPrefab;

    [Header("Buttons")]
    [SerializeField] private Button _createRoomButton;
    [SerializeField] private Button _randomRoomButton;
    [SerializeField] private Button _joinRoomButton;

    public ListItem ItemPrefab => _itemPrefab;
    public Transform Content => _content;
    public TMP_InputField InputField => _inputField;

    public event UnityAction CreateRoomClicked;
    public event UnityAction RandomRoomClicked;
    public event UnityAction JoinRoomClicked;

    private void OnEnable()
    {
        _createRoomButton.onClick.AddListener(OnClickedCreateRoomButton);
        _randomRoomButton.onClick.AddListener(OnClickedRandomRoomButton);
        _joinRoomButton.onClick.AddListener(OnClickedJoinRoomButton);
    }

    private void OnDisable()
    {
        _createRoomButton.onClick.RemoveListener(OnClickedCreateRoomButton);
        _randomRoomButton.onClick.RemoveListener(OnClickedRandomRoomButton);
        _joinRoomButton.onClick.RemoveListener(OnClickedJoinRoomButton);
    }

    private void OnClickedJoinRoomButton()
    {
        JoinRoomClicked?.Invoke();
    }
    
    private void OnClickedRandomRoomButton()
    {
        RandomRoomClicked?.Invoke();
    }
    
    private void OnClickedCreateRoomButton()
    {
        CreateRoomClicked?.Invoke();
    }
}
