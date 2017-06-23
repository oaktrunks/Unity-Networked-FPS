using UnityEngine;
using UnityEngine.Networking;

public class TargetController : NetworkBehaviour
{

    void OnCollisionEnter(Collision other)
    {
        if(other.gameObject.tag == "Bullet")
        {
            gameObject.transform.position = new Vector3(
                Random.Range(-25.0f, 25.0f),
                Random.Range(5.0f, 10.0f),
                Random.Range(-25.0f, 25.0f));

            gameObject.transform.rotation = Quaternion.Euler(
                Random.Range(0, 180),
                Random.Range(0, 180),
                Random.Range(0, 180));

            //other.gameObject.GetComponent<BulletDestructor>().getShooter().GetComponent<PlayerController>().KilledTarget();
        }
    }
}
