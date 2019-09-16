using UnityEngine;

public class Destroyer : MonoBehaviour
{
    private void Update()
    {
        if (transform.position.y <= -50f)
        {
            Destroy();
        }
    }

    public void Destroy()
    {
        Destroy(gameObject);
    }
}