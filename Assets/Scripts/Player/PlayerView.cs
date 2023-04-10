using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerView : MonoBehaviour
{
    [SerializeField] private GameObject[] _skins;
    [SerializeField] private PlayerDeath _playerDeath;
    [SerializeField] private Camera _camera;

    private const string SAVED_SKIN = "SkinSaveID";

    private GameObject _currentSkinActive;
    private int _showSkinIndex;

    public GameObject CurrentSkinActive => _currentSkinActive;
    public PlayerDeath PlayerDeath => _playerDeath;
    public Camera Camera => _camera;

    public event UnityAction SkinChanged;

    public int CurrentSkin
    {
        get { return PlayerPrefs.GetInt(SAVED_SKIN, 0); }
        private set { PlayerPrefs.SetInt(SAVED_SKIN, value); }
    }

    public void ShowSkin(int value)
    {
        _skins[_showSkinIndex].gameObject.SetActive(false);
        _showSkinIndex += value;

        if (_showSkinIndex > _skins.Length - 1)
        {
            _skins[0].gameObject.SetActive(true);
            _showSkinIndex = 0;
        }
        else if (_showSkinIndex < 0)
        {
            _skins[_skins.Length - 1].gameObject.SetActive(true);
            _showSkinIndex = _skins.Length - 1;
        }
        else
            _skins[_showSkinIndex].gameObject.SetActive(true);

        CurrentSkin = _showSkinIndex;

        _currentSkinActive = _skins[_showSkinIndex];
    }
}
