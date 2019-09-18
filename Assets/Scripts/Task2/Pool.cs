using UnityEngine;

public class Pool : MonoBehaviour
{
    [SerializeField] private int maxPoolSize = Task2.PlayersWithMaximumHpCount;
    [SerializeField] private GameObject poolObjectPrefab = null;
    private GameObject[] _pool;
    private int _size;

    private void Awake()
    {
        _pool = new GameObject[maxPoolSize];
    }

    public GameObject Pop()
    {
        var obj = FindFirstNotActive();

        if (obj == null && _size < maxPoolSize)
        {
            obj = Instantiate(poolObjectPrefab, transform);
            _pool[_size] = obj;
            _size++;
        }

        if (obj != null)
            obj.SetActive(true);
        return obj;
    }


    private GameObject FindFirstNotActive()
    {
        for (var i = 0; i < _size; i++)
        {
            if (_pool[i] != null && !_pool[i].activeSelf)
            {
                return _pool[i];
            }
        }

        return null;
    }

    public void Push(GameObject obj)
    {
        obj.SetActive(false);
    }

    public void ReturnObjects()
    {
        for (var i = 0; i < _size; i++)
        {
            _pool[i].SetActive(false);
        }
    }
}