using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class Projectile : NetworkBehaviour
{
    public float timeoutInSeconds = 1f, currTime = 0;
    public GameObject owner;
    private void Start() 
    {
        StartCoroutine(ProjectileTimeout());
    }

    public IEnumerator ProjectileTimeout()
    {
        yield return new WaitForSeconds(timeoutInSeconds);
        owner.GetComponent<CreateProjectile>().DeleteProjectile(this);
    }

    [Command]
    public void DeleteObject()
    {
        print("Trying");
        if(!isServer && GetComponent<NetworkIdentity>().isLocalPlayer == true)
        {
            print("Success");
            NetworkServer.Destroy(gameObject);
            Destroy(gameObject);
        }
    }
}
