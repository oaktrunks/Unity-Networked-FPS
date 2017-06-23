using UnityEngine;
using UnityEngine.Networking;

public class PlayerController : NetworkBehaviour {
    public Camera Camera;
    public GameObject PlayerModel;
    public GameObject BulletPrefab;
    public Transform BulletSpawn;

    public float mouseSensitivity = 100.0f;
    public float clampAngle = 180.0f;
    public float bulletSpeed = 10.0f;
    public float bulletTimeOut = 5.0f;

    private float rotY = 0.0f; // rotation around the up/y axis
    private float rotX = 0.0f; // rotation around the right/x axis

    private int targetsKilled = 0;
    private int playersKilled = 0;

    void Start()
    {
        if(!isLocalPlayer)
        {
            Camera.GetComponent<AudioListener>().enabled = false;
            Camera.enabled = false;
            return;
        }

        Cursor.visible = false;  //Hiding cursor. Implement way to bring it back? for networking selection and menus
        Cursor.lockState = CursorLockMode.Locked; //Locking cursor to center of screen
        Vector3 rot = PlayerModel.transform.localRotation.eulerAngles;
        rotY = rot.y;
        rotX = rot.x;
    }

    void Update()
    {
        
        if (!isLocalPlayer)
        {
            return;
        }

        //Getting forwards and sideways movement
        var x = Input.GetAxis("Horizontal") * Time.deltaTime * 5.0f;
        var z = Input.GetAxis("Vertical") * Time.deltaTime * 5.0f;

        //Check for wall collission = stop movement.
        if (x > 0)
        {
            //Debug.DrawRay(transform.position, transform.right, Color.red , 0.25f);
            if (Physics.Raycast(transform.position, transform.right, 1.75f))
            {
                    x = 0;
            }
        }
        else if(x < 0)
        {
            //Debug.DrawRay(transform.position, -transform.right, Color.red , 0.25f);
            if (Physics.Raycast(transform.position, -transform.right, 1.75f)) 
            {
                x = 0;
            }
        }

        if(z > 0)
        {
            //Debug.DrawRay(transform.position, transform.forward, Color.red , 0.25f);
            if (Physics.Raycast(transform.position, transform.forward, 1.75f)) 
            {
                z = 0;
            }
        }
        else if(z < 0)
        {
            //Debug.DrawRay(transform.position, -transform.forward, Color.red , 0.25f);
            if (Physics.Raycast(transform.position, -transform.forward, 1.75f)) 
            {
                z = 0;
            }
        }

        transform.Translate(x, 0, 0);
        transform.Translate(0, 0, z);

        //Getting mouse movements
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = -Input.GetAxis("Mouse Y");

        rotY += mouseX * mouseSensitivity * Time.deltaTime;
        rotX += mouseY * mouseSensitivity * Time.deltaTime;

        rotX = Mathf.Clamp(rotX, -clampAngle, clampAngle);

        //Used for horizontal rotation
        Quaternion baseRotation = Quaternion.Euler(0.0f, rotY, 0.0f);
        transform.rotation = baseRotation;

        //Used for vertical rotation
        //Decided to seperate the two so moving forwards and backwards feels correct and doesn't make you fly.
        //Parent rotates sideways, child(playermodel) rotates upwards and downwards.
        Quaternion localRotation = Quaternion.Euler(rotX, 0.0f, 0.0f);
        PlayerModel.transform.localRotation = localRotation;

        //Shooting
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            CmdFire();
        }

    }

    void LateUpdate()
    {   

    }

    [Command]
    void CmdFire()
    {
        // Create the Bullet from the Bullet Prefab

        var bullet = (GameObject)Instantiate(
            BulletPrefab,
            BulletSpawn.position,
            BulletSpawn.rotation* Quaternion.Euler(0,0,0));

        //Make it fly towards reticle if player is looking at an object. Else it flies to maximum range (?)
        var reticleRay = Camera.ViewportPointToRay(new Vector3(0.5F, 0.5F, 0));
        RaycastHit hit;
        if (Physics.Raycast(reticleRay, out hit))
        {
            bullet.transform.LookAt(hit.point);
            //Debug.Log("pointing bullet at");
            //Debug.Log(hit.transform.gameObject);

            // Add velocity to the bullet towards reticicle
            bullet.GetComponent<Rigidbody>().velocity = bullet.transform.forward * bulletSpeed;
        }
        else //looking at air, shoot to max range (?)
            bullet.GetComponent<Rigidbody>().velocity = BulletSpawn.forward * bulletSpeed;


        //Used for point counting. bullet needs to hold reference to shooter
        //bullet.GetComponent<BulletDestructor>().setShooter(gameObject);
        //Currently not working

        //Spawning it on every client.
        NetworkServer.Spawn(bullet);

        // Destroy the bullet after bulletTimeOut seconds
        Destroy(bullet, bulletTimeOut);
    }

    public void KilledTarget()
    {
        targetsKilled++;
        Debug.Log("target killed");
    }

    public void KilledPlayer()
    {
        playersKilled++;
        Debug.Log("player killed");
    }
}