using UnityEngine;

public class Teleporter : MonoBehaviour
{
    public Transform teleportLocation;

    public void OnTriggerEnter(Collider other)
    {
        CharacterController cc = other.gameObject.GetComponent<CharacterController>();

        if (cc != null)
            cc.enabled = false;

        other.gameObject.transform.position = teleportLocation.position;
        other.gameObject.transform.rotation = teleportLocation.rotation;

        if (cc != null)
            cc.enabled = true;
    }
}