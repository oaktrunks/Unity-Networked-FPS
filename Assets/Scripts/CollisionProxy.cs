using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionProxy : MonoBehaviour {

    public GameObject getParent()
    {
        return transform.parent.gameObject;
    }

}
