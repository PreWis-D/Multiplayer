using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flashlight : CollectableObject
{
    [SerializeField] private Transform _light;
    [SerializeField] private AudioSource _audioSource;

    private bool _isLighting;

    public Transform Light => _light;

    public void ChangeLight()
    {
        photonView.RPC(nameof(ChangeLightState), RpcTarget.All, _light.gameObject.GetPhotonView().ViewID);
    }

    [PunRPC]
    private void ChangeLightState(int objID)
    {
        if (_audioSource.isPlaying)
            _audioSource.Stop();

        if (_light.gameObject.activeSelf)
            PhotonNetwork.GetPhotonView(objID).gameObject.SetActive(false);
        else
            PhotonNetwork.GetPhotonView(objID).gameObject.SetActive(true);

        _audioSource.Play();
    }

}
