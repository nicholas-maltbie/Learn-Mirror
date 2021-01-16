using Mirror;
using UnityEngine;

/// <summary>
/// Have a character controller push any dynamic rigidbody it hits
/// </summary>
[RequireComponent(typeof(NetworkControls))]
public class CharacterControllerPush : NetworkBehaviour
{
    /// <summary>
    /// Power of the player push
    /// </summary>
    public float pushPower = 2.0f;

    /// <summary>
    /// Force of pushing down when standing on objects
    /// </summary>    
    public float weight = 10.0f;

    /// <summary>
    /// Network controls for this player, things like gravity and controls
    /// </summary>
    private NetworkControls movementSettings;

    void Start()
    {
        this.movementSettings = this.GetComponent<NetworkControls>();
    }

    /// <summary>
    /// Push a hit game object
    /// </summary>
    /// <param name="hit">Object hit</param>
    /// <param name="force">Force of push</param>
    /// <param name="point">Point of hit on the game object</param>
    [Command]
    void CmdPushWithForce(GameObject hit, Vector3 force, Vector3 point)
    {
        hit.GetComponent<Rigidbody>().AddForceAtPosition(force, point);
    }

    void OnControllerColliderHit (ControllerColliderHit hit)
    {
        if (!isLocalPlayer)
        {
            // exit from update if this is not the local player
            return;
        }

        Rigidbody body = hit.collider.attachedRigidbody;

        // Do nothing if the object does not have a rigidbody or if
        //   the rigidbody is kinematic
        if (body == null || body.isKinematic)
        {
            return;
        }

        UnityEngine.Debug.Log($"Hit something {hit.collider.gameObject}, {movementSettings.moveDirection * 1000}");
 
        Vector3 force = Vector3.zero;
        // We use gravity and weight to push things down, we use
        // our velocity and push power to push things other directions
        if (hit.moveDirection.y < -0.3) {
            // If below us, push down
            // Only take the movement component associated with gravity
            force = Vector3.Scale(movementSettings.gravity.normalized, movementSettings.moveDirection) * pushPower;
            // Also add some force from gravity in case we're not moving down now
            force += movementSettings.gravity.normalized * weight;
        } else {
            // If to the side, use the controller velocity
            // Project movement vector onto plane defined by gravity normal (horizontal plane)
            force = Vector3.ProjectOnPlane(movementSettings.moveDirection, movementSettings.gravity) * pushPower;
        }
    
        // Apply the push
        CmdPushWithForce(hit.collider.gameObject, force, hit.point);
    }
}