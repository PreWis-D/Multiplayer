using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Photon.Pun;

public class Manager : MonoBehaviourPunCallbacks
{
    [SerializeField] private PlayerView _playerPrefab;
    [SerializeField] private Transform _spawnPoint;
    [SerializeField] private GameMenu _gameMenu;

    private List<PlayerView> _players = new List<PlayerView>();
    private bool _inJoinedRoom = false;
    private List<Camera> _cameras = new List<Camera>();

    public IEnumerable<PlayerView> Players => _players;
    public PlayerView PlayerView { get; private set; }

    private new void OnEnable()
    {
        base.OnEnable();
        _gameMenu.PanelHided += OnPanelHided;
        _gameMenu.GameExit += OnGameExit;
    }

    private new void OnDisable()
    {
        base.OnDisable();
        _gameMenu.PanelHided -= OnPanelHided;
        _gameMenu.GameExit -= OnGameExit;
    }

    private void Start()
    {
        StartCoroutine(JoinedRoom());
    }

    private void Spawn()
    {
        GameObject playerView = PhotonNetwork.Instantiate(_playerPrefab.name, _spawnPoint.position, _spawnPoint.rotation);
        PlayerView = playerView.GetComponent<PlayerView>();
        _players.Add(playerView.GetComponent<PlayerView>());
        _cameras.Add(_players[_players.Count - 1].Camera);
        
        foreach (var player in _players)
            player.PlayerDeath.SetCameras(_cameras);
        
        

        int random = Random.Range(0, 4);
        PlayerView.ShowSkin(random);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            _gameMenu.ShowGameMenu();

            foreach (var player in _players)
                player.GetComponent<PlayerController>().ChangeMoveCamera(false);
        }
    }

    private void OnPanelHided()
    {
        foreach (var player in _players)
            player.GetComponent<PlayerController>().ChangeMoveCamera(true);
    }

    private void OnGameExit()
    {
        PhotonNetwork.LeaveRoom();
    }

    public override void OnLeftRoom()
    {
        PhotonNetwork.LoadLevel("Menu");
    }

    private IEnumerator JoinedRoom()
    {
        while(_inJoinedRoom == false)
        {
            if (PhotonNetwork.InRoom == true)
            {
                Spawn();
                _inJoinedRoom = true;
            }

            yield return new WaitForEndOfFrame();
        }
    }
}
