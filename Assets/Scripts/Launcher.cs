using System.Collections;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEngine;

public class Launcher : MonoBehaviour
{
    [SerializeField] private GameObject[] _prefabs;
    [SerializeField] private Transform _launchPoint;
    [SerializeField] private float _delay;
    [SerializeField] private float _xDitection;
    [SerializeField] private float _yDitection;
    [SerializeField] private Vector2 _forceXRange;
    [SerializeField] private Vector2 _forceYRange;
    void Start()
    {
        StartCoroutine(LaunchProcess());
    }

    private IEnumerator LaunchProcess()
    {
        while (true)
        {
            var newDynamite = Instantiate(_prefabs[Random.Range(0,_prefabs.Length)], _launchPoint);
            var randomForce = new Vector2();
            randomForce.x = Random.Range(_forceXRange.x, _forceXRange.y) * _xDitection;
            randomForce.y = Random.Range(_forceYRange.x, _forceYRange.y) * _yDitection;
            newDynamite.GetComponent<Rigidbody2D>().velocity = randomForce;
            yield return new WaitForSeconds(_delay);
        }
    }
    
}
