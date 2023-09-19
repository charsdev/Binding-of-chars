using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstantiateSFX : MonoBehaviour
{
    public GameObject sfx;

    public void Instatiate()
    {
        Instantiate(sfx, transform.position, Quaternion.identity);
    }
}
