using UnityEngine;
using Mirror;

[RequireComponent(typeof(CharacterController))]
public class NetworkControls : NetworkBehaviour
{
    /// <summary>
    /// Character controller to move character
    /// </summary>
    private CharacterController characterController;

    /// <summary>
    /// Transform holding camera
    /// </summary>
    public Transform cameraPosition;

    /// <summary>
    /// Movement speed (in units per second)
    /// </summary>
    public float movementSpeed = 2.0f;

    /// <summary>
    /// Direction and magnitude of gravity
    /// </summary>
    public Vector3 gravity = new Vector3(0, -9.8f, 0);

    /// <summary>
    /// Current player velocity
    /// </summary>
    private Vector3 velocity = Vector3.zero;

    /// <summary>
    /// Velocity of player jump in units per second
    /// </summary>
    public float jumpVelocity;

    /// <summary>
    /// Maximum pitch for rotating character camera in degrees
    /// </summary>
    public float maxPitch = 90;

    /// <summary>
    /// Minimum pitch for rotating character camera in degrees
    /// </summary>
    public float minPitch = -90;

    /// <summary>
    /// Pitch of the player
    /// </summary>
    [SyncVar]
    public float pitch;

    /// <summary>
    /// Rotation rate of camera in degrees per second per one unit of axis movement
    /// </summary>
    public float rotationRate = 180;

    /// <summary>
    /// How the character intended to move this frame
    /// </summary>
    /// <value>The direction the character tried to move this frame</value>
    public Vector3 moveDirection;

    void Start()
    {
        this.characterController = this.GetComponent<CharacterController>();
    }

    void Update()
    {
        if (!isLocalPlayer)
        {
            // exit from update if this is not the local player
            return;
        }

        // If the player is not grounded, increment velocity by acceleration due to gravity
        if (!characterController.isGrounded)
        {
            velocity += gravity * Time.deltaTime;
        }
        // If the character is grounded, set velocity to zero
        else
        {
            velocity = Vector3.zero;
        }

        float yaw = transform.rotation.eulerAngles.y;
        yaw += rotationRate * Time.deltaTime * Input.GetAxis("Mouse X");
        pitch += rotationRate * Time.deltaTime * -1 * Input.GetAxis("Mouse Y");
        // Clamp rotation of camera between minimum and maximum specified pitch
        pitch = Mathf.Clamp(pitch, minPitch, maxPitch);

        // Set the player's rotation to be that of the camera's yaw
        transform.rotation = Quaternion.Euler(0, yaw, 0);
        // Set pitch to be camera's rotation
        cameraPosition.rotation = Quaternion.Euler(pitch, yaw, 0);

        if (this.characterController.isGrounded && Input.GetButton("Jump"))
        {
            velocity = new Vector3(0, this.jumpVelocity, 0);
        }

        // handle player input for movement (but only on local player)
        // Setup a movement vector
        // Get user input and move player if moving
        Vector3 movement = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));

        // Rotate movement vector by player yaw (rotation about vertical axis)
        Quaternion horizPlaneView = transform.rotation;
        Vector3 movementVelocity = horizPlaneView * movement * movementSpeed;

        // Normalize movement vector to be a max of 1 if greater than one
        movement = movement.magnitude > 1 ? movement / movement.magnitude : movement;
        // Set how this character intended to move this frame
        this.moveDirection = (movementVelocity + velocity);
        // Move player by displacement
        this.characterController.Move(this.moveDirection * Time.deltaTime);

    }
}