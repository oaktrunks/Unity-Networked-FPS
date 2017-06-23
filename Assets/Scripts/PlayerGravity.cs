using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGravity : MonoBehaviour
{

    public float maxSpeed = 0.5f;
    public LayerMask layerMask; //So that we can ignore certain layers, check editor.
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;
        //Debug.DrawRay(transform.position, -Vector3.up, Color.red, 0.25f);
        if (Physics.Raycast(transform.position, -Vector3.up, out hit, Mathf.Infinity, layerMask)) //Checking if ray to ground hit anything
        {
            //Debug.Log(hit.collider.gameObject);
            var distanceToGround = hit.distance;
            if (distanceToGround < 1 || distanceToGround > 1)
            {
                //transform.Translate(0, 1 - distanceToGround , 0); //Move so that player is 1 unit away from ground
                //deprecated in favor of slower falling speed (not instant teleportation)

                //Limit fall speed so player isn't instantly on ground when stepping over a cliff.
                float step = maxSpeed * Time.deltaTime;
                //Debug.Log(hit.distance);
                transform.position = Vector3.MoveTowards(transform.position, new Vector3(transform.position.x, hit.point.y + 1, transform.position.z), step);
            }
        }
    }
}
