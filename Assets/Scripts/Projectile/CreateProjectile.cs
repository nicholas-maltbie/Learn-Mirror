using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

[RequireComponent(typeof(NetworkControls))]
public class CreateProjectile : NetworkBehaviour
{
    [SerializeField] private GameObject projectile;
    public Transform spawnLocation;
    public float velocity = 3f;
    void Update()
    {
        if(Input.GetButtonDown("Fire1"))
        {
            SpawnProjectile();
        }
    }

    [Command]
    public void SpawnProjectile()
    {
        var nwc = GetComponent<NetworkControls>();
        var lookDirection = nwc.cameraPosition.forward;
        var spawnPosition = spawnLocation;
        var spawnVelocity = lookDirection*velocity;
        var proj = GameObject.Instantiate(projectile, spawnPosition.position, Quaternion.identity);
        proj.GetComponent<Rigidbody>().velocity = spawnVelocity;
        if(!isServer)
        {
            proj.GetComponent<NetworkIdentity>().AssignClientAuthority(this.GetComponent<NetworkIdentity>().connectionToClient);
        }
        NetworkServer.Spawn(proj);
    }
}
