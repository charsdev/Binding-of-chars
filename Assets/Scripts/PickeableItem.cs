using UnityEngine;

public abstract class Pickeable : MonoBehaviour
{
    protected abstract bool Pick(Collider2D picker);
 

    private void OnTriggerEnter2D(Collider2D picker)
    {
        if (Pick(picker))
        {

        }
    }
}

