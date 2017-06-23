using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class BulletDestructor : NetworkBehaviour
{
    public int damagePerBullet = 15;

    //[SyncVar]
    //private GameObject shooter;

    void OnCollisionEnter(Collision collision)
    {
        var hit = collision.gameObject;
        if (hit.tag == "Player")
        {
            //var health = hit.GetComponent<Health>();
            var health = hit.GetComponent<CollisionProxy>().getParent().GetComponent<Health>();
            if (health != null)
            {
                //health.TakeDamage(damagePerBullet, shooter);
                health.TakeDamage(damagePerBullet);
            }
        }
        Destroy(gameObject);
    }

    //public void setShooter(GameObject shotfirer)
    //{
    //    shooter = shotfirer;
    //}

    //public GameObject getShooter()
    //{
    //    return shooter;
    //}
}