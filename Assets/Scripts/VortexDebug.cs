using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// VortexDebug components. Provides useful for debug functionality such as increase of vortex size and drawing path  of object such as if it didn't interact with vortex
/// </summary>
public class VortexDebug : MonoBehaviour
{
    [SerializeField] private GameObject ghostHead = null;
    [SerializeField] private GameObject ghostTailPiece = null;
    [SerializeField] private bool isDrawGhost = true;
    private bool _isDebugEnabled;
    private readonly Dictionary<Collider2D, Ghost> _ghostTails = new Dictionary<Collider2D, Ghost>();

    public bool IsDebugEnabled
    {
        private get { return _isDebugEnabled; }
        set
        {
            _isDebugEnabled = value;
            UpdateVortexSize();
        }
    }

    /// <summary>
    /// Increases vortex size for debug purposes and restore it size on default mode
    /// </summary>
    private void UpdateVortexSize()
    {
        if (IsDebugEnabled)
        {
            var transformLocal = transform;
            transformLocal.localScale *= 2;
        }
        else
        {
            var transformLocal = transform;
            transformLocal.localScale /= 2;
        }
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!IsDebugEnabled || !isDrawGhost) return;

        var ghost = new Ghost(other, ghostHead, ghostTailPiece);
        StartCoroutine(ghost.CreateGhostTail(this));
        _ghostTails.Add(other, ghost);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (_ghostTails.TryGetValue(other, out var ghost))
        {
            ghost.DestroyTail();
            _ghostTails.Remove(other);
        }
    }


    /// <summary>
    /// Ghost object that moves like an original object if it hadn't interact with vortex and leaves
    /// </summary>
    class Ghost
    {
        private readonly GameObject _ghost;
        private readonly GameObject _ghostTailPrefab;

        private bool CanDestroyTail { get; set; }

        /// <summary>
        /// Creates ghost object for original with required head and tail prefabs
        /// </summary>
        /// <param name="originalObject">Original object Collider2D component.</param>
        /// <param name="ghostHeadPrefab">Prefab for head of ghost.</param>
        /// <param name="ghostTail">Prefab for any part of ghost tail</param>
        public Ghost(Collider2D originalObject, GameObject ghostHeadPrefab, GameObject ghostTail)
        {
            _ghostTailPrefab = ghostTail;
            var originalTransform = originalObject.transform;
            _ghost = Instantiate(ghostHeadPrefab, originalTransform.position, originalTransform.rotation,
                originalTransform.parent);

            var ghostRb = _ghost.GetComponent<Rigidbody2D>();
            var originalRb = originalObject.GetComponent<Rigidbody2D>();
            ghostRb.velocity = originalRb.velocity;
        }

        /// <summary>
        /// Creates ghost tail during head object movement. Function creates and draw 1 tail object for every 2 fixed update iterations
        /// </summary>
        /// <param name="root"> Object - holder of coroutine that destroys tail </param>
        /// <returns> IEnumerator object for Unity coroutine</returns>
        public IEnumerator CreateGhostTail(MonoBehaviour root)
        {
            var ghostTransform = _ghost.transform;
            var size = 30;
            while (size > 0)
            {
                var staticGhost = Instantiate(_ghostTailPrefab, ghostTransform.position, ghostTransform.rotation,
                    ghostTransform.parent);

                var staticRb = staticGhost.GetComponent<Rigidbody2D>();
                root.StartCoroutine(GhostFallen(staticRb));

                yield return new WaitForFixedUpdate();
                yield return new WaitForFixedUpdate();
                size--;
            }
        }

        /// <summary>
        /// Destroys ghost tail after original object leaves vortex area
        /// </summary>
        /// <param name="ghostRb">Part of tail to destroy</param>
        /// <returns> IEnumerator object for Unity coroutine</returns>
        private IEnumerator GhostFallen(Rigidbody2D ghostRb)
        {
            yield return new WaitUntil(() => CanDestroyTail);
            ghostRb.simulated = true;
            ghostRb.velocity = Vector2.down;
        }

        /// <summary>
        /// Triggers tail destroying
        /// </summary>
        public void DestroyTail()
        {
            CanDestroyTail = true;
        }
    }
}