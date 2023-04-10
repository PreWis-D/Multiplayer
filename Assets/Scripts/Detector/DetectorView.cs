using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

public class DetectorView : MonoBehaviour
{
    [SerializeField] private DetectorCollectable _detector;
    [SerializeField] private Vector2 _noEnemyRange = new (1, 6);
    [SerializeField] private TMP_Text _text;
    [SerializeField] private int _minDetectorValue = 1;
    [SerializeField] private int _maxDetectorValue = 45;
    [SerializeField] private AudioSource _audioSource;


    private float _detectorRadius;
    private float _multiplier;

    private void Awake()
    {
        _detectorRadius = _detector.Radius;
        _multiplier = _maxDetectorValue / _detectorRadius;
        _audioSource.volume = 0.1f;
    }

    private void OnEnable()
    {
        _detector.DistanceCalculated += OnValueCalculated;
    }

    private void OnDisable()
    {
        _detector.DistanceCalculated -= OnValueCalculated;
    }

    private void OnValueCalculated(float value)
    {
        if (value < 0)
        {
            _text.text = "0.00" + Mathf.RoundToInt(Random.Range(_noEnemyRange.x, _noEnemyRange.y));
            _audioSource.volume = 0.1f;
            return;
        }

        int result = _maxDetectorValue - Mathf.RoundToInt(Mathf.Clamp(value * _multiplier + Random.Range(-4, 4), _minDetectorValue, _maxDetectorValue));

        _audioSource.volume = Mathf.Clamp((float) result / (float)_maxDetectorValue, 0.1f, 1f);
        
        _text.text = (result / 10) + "." + result % 10 + "00";
    }
}
