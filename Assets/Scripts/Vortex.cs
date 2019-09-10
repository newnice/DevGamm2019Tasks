using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Vortex component. Changes gravity and velocity for caught objects so that object slows down required times.
/// </summary>
public class Vortex : MonoBehaviour
{
    private class AffectedObject
    {
        private readonly Rigidbody2D _rb;
        private readonly float _slowDownCoeff;

        public AffectedObject(Rigidbody2D rb, float slowDownCoeff)
        {
            _rb = rb;
            _slowDownCoeff = slowDownCoeff;
        }


        /// <summary>
        ///  Slow down object required times
        /// </summary>
        public void SlowDown()
        {
            _rb.velocity /= _slowDownCoeff;
            _rb.gravityScale /= _slowDownCoeff * _slowDownCoeff;
        }


        /// <summary>
        /// Restore physics data for values before interaction
        /// </summary>
        public void RestorePhysics()
        {
            _rb.velocity *= _slowDownCoeff;
            _rb.gravityScale *= _slowDownCoeff * _slowDownCoeff;
        }
    }

    [SerializeField] private float slowDownCoeff = 4f;
    private Dictionary<Collider2D, AffectedObject> _affectedObjects;

    private void Start()
    {
        _affectedObjects = new Dictionary<Collider2D, AffectedObject>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        var rb = other.attachedRigidbody;
        var dd = new AffectedObject(rb, slowDownCoeff);
        _affectedObjects.Add(other, dd);
        StartCoroutine(SlowDown(dd));
    }

    /// <summary>
    /// Slows down object in the end of fixed update loop for correct work of vortex debug component
    /// </summary>
    /// <param name="ao"> affected object</param>
    /// <returns> IEnumerator object for Unity coroutine that slows down affected object at the end of fixed update iteration</returns>
    private IEnumerator SlowDown(AffectedObject ao)
    {
        yield return new WaitForFixedUpdate();
        ao.SlowDown();
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!_affectedObjects.TryGetValue(other, out var objectData)) return;

        objectData.RestorePhysics();
        _affectedObjects.Remove(other);
    }
}