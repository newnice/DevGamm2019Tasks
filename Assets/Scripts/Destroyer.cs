using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destroyer : MonoBehaviour
{
    private void Update()
    {
        if (transform.position.y < -200f)
        {
            Destroy(gameObject);
        }
    }
}
