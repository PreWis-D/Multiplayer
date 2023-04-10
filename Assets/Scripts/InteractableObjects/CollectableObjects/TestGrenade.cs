using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ProBuilder.Shapes;

public class TestGrenade : MonoBehaviour
{
    [SerializeField] float _delay = 3f; // �������� ����� �������
    [SerializeField] float _force = 700f; // ���� ������
    [SerializeField] float _radius = 5f; // ������ ������

    [SerializeField] private AudioClip _launch; // ���� �������
    [SerializeField] private AudioClip _explosionSound; // ���� ������
    private AudioSource _audioSouce; // ����� �����

    private GameObject _explosion; // ������
    private GameObject _explosiveObject; // ����� ������� ���������� ������ ��� ������

    private void Start()
    {
        gameObject.AddComponent<AudioSource>(); // ��������� ����� �����
        _audioSouce = gameObject.GetComponent<AudioSource>(); // ����������� ����� ����� �� ���� � ��� ���
        _audioSouce.PlayOneShot(_launch); //���� �������
        StartCoroutine(DelayExplode(_delay));
    }

    private IEnumerator DelayExplode(float delay)
    {
        yield return new WaitForSeconds(delay);

        Explode();
    }

    private void Explode()
    {
        _explosiveObject = Instantiate(_explosion, transform.position, transform.rotation);
        _explosiveObject.AddComponent<AudioSource>();
        _explosiveObject.GetComponent<AudioSource>().PlayOneShot(_explosionSound);
        Debug.Log("Boom");

        //����� ����� ������ ������������ ������
        Destroy(gameObject);
    }
}
