using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PalaceController : MonoBehaviour {

    // Returns the x-y-z coordinates of a random point that exists inside the box collider of this GameObject
    // Used for getting points to spawn explosions at if this object is to have a fiery death
    public Vector3 GetPositionInsideObject() {

        // A ""fun"" note on how Unity is handling the transform of this object
        // This object's transform's anchor point is the centerpoint of its bottom plane.
        // BUT to get this function to accurately return things inside its collider,
        // We assume its anchored from the center of the collider.
        // Somehow this made sense to someone.

        float xRange = GetComponent<BoxCollider>().size.x;
        xRange /= 2f;
        float yRange = GetComponent<BoxCollider>().size.y;
        yRange /= 2f;
        float zRange = GetComponent<BoxCollider>().size.z;
        zRange /= 2f;

        return new Vector3(transform.position.x + Random.Range(-xRange, xRange),
            transform.position.z + Random.Range(-zRange, zRange),
                            transform.position.y + Random.Range(-yRange, yRange) 
                            );
    }

    // a "shrinkrement" is a shrink increment. Pardon the pun.
    // One shrinkrement reduces the scale of the object by shrinkAmount.
    public IEnumerator GetSmaller(int shrinkrements, float shrinkAmount) {
        yield return new WaitForSeconds(0.4f);
        for (int i = 0; i < shrinkrements; i++) {
            transform.localScale -= new Vector3(shrinkAmount, shrinkAmount, shrinkAmount);
            yield return null;
        }
    }
}
