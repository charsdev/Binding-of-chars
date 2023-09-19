using UnityEngine;

public class ShadowScript : MonoBehaviour
{
    public Transform objectToFollow; // Reference to the object casting the shadow
    public float shadowHeight = 0.1f; // Height of the shadow from the ground

    private void LateUpdate()
    {
        if (objectToFollow != null)
        {
            // Update the shadow position based on the object's position
            Vector3 newPosition = objectToFollow.position;
            newPosition.y = shadowHeight;
            transform.position = newPosition;

            // Update the shadow scale based on the object's height
            float objectHeight = objectToFollow.position.y;
            float scaleMultiplier = Mathf.Clamp01(objectHeight / shadowHeight);
            transform.localScale = new Vector3(scaleMultiplier, scaleMultiplier, 1f);
        }
    }
}
