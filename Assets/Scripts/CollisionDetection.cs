using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionDetection : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        // Get Rigidbody
        var rb = GetComponent<Rigidbody>();
 
        // Freeze PositionXYZ and RotationXYZ of user
        rb.constraints = RigidbodyConstraints.FreezePosition;
        rb.constraints = RigidbodyConstraints.FreezeRotation;
        //rb.constraints = RigidbodyConstraints.FreezeAll;

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        this.gameObject.transform.Translate (0, 0, 0);
    }
}