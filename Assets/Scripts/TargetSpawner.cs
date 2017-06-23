/*

    Used for spawning targets at the start of the game.
    Point counting and respawning targets is part of target's script.

*/
using UnityEngine;
using UnityEngine.Networking;


public class TargetSpawner : NetworkBehaviour
{

    public GameObject TargetPrefab;
    public int NumberOfTargets;

    public override void OnStartServer()
    {
        for (int i = 0; i < NumberOfTargets; i++)
        {
            var spawnPosition = new Vector3(
                Random.Range(-25.0f, 25.0f),
                Random.Range(5.0f, 10.0f),
                Random.Range(-25.0f, 25.0f));

            var spawnRotation = Quaternion.Euler(
                Random.Range(0, 180),
                Random.Range(0, 180),
                Random.Range(0, 180));

            var enemy = (GameObject)Instantiate(TargetPrefab, spawnPosition, spawnRotation);
            NetworkServer.Spawn(enemy);
        }
    }
}
