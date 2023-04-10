using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ProBuilder.Shapes;

public class TestGrenade : MonoBehaviour
{
    [SerializeField] float _delay = 3f; // Задержка перед взрывом
    [SerializeField] float _force = 700f; // Сила взрыва
    [SerializeField] float _radius = 5f; // Радиус взрыва

    [SerializeField] private AudioClip _launch; // звук запуска
    [SerializeField] private AudioClip _explosionSound; // звук взрыва
    private AudioSource _audioSouce; // аудио соурс

    private GameObject _explosion; // Эффект
    private GameObject _explosiveObject; // обект который показывает эффект при взрыве

    private void Start()
    {
        gameObject.AddComponent<AudioSource>(); // Добавляем аудио соурс
        _audioSouce = gameObject.GetComponent<AudioSource>(); // Присваеваем аудио соурс из игры в наш код
        _audioSouce.PlayOneShot(_launch); //звук запуска
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

        //здесь нужна логика ослепляющего взрыва
        Destroy(gameObject);
    }
}
