using System.Collections;
using UnityEngine;

public class Launcher : MonoBehaviour
{
    [SerializeField] private GameObject[] _prefabs = null;
    [SerializeField] private Transform _launchPoint = null;
    [SerializeField] private float _delay = 0f;
    [SerializeField] private float _xDirection = 0f;
    [SerializeField] private float _yDirection = 0f;
    [SerializeField] private Vector2 _forceXRange = Vector2.zero;
    [SerializeField] private Vector2 _forceYRange = Vector2.zero;

    private Coroutine _activeCoroutine;

    void Start()
    {
        StartLaunch();
    }

    private IEnumerator LaunchProcess()
    {
        while (true)
        {
            var newDynamite = Instantiate(_prefabs[Random.Range(0, _prefabs.Length)], _launchPoint);
            var randomForce = new Vector2
            {
                x = Random.Range(_forceXRange.x, _forceXRange.y) * _xDirection,
                y = Random.Range(_forceYRange.x, _forceYRange.y) * _yDirection
            };
            newDynamite.GetComponent<Rigidbody2D>().velocity = randomForce;
            yield return new WaitForSeconds(_delay);
        }
    }

    public void EnableLaunch(bool enable)
    {
        if (_activeCoroutine != null && !enable)
        {
            StopCoroutine(_activeCoroutine);
            _activeCoroutine = null;
            KillChildren();
        }
        else if (enable)
            StartLaunch();
    }

    private void StartLaunch()
    {
        _activeCoroutine = StartCoroutine(LaunchProcess());
    }

    private void KillChildren()
    {
        foreach (var child in transform)
        {
            var tr = child as Transform;
            if (tr != null)
                tr.GetComponent<Destroyer>().Destroy();
        }
    }
}