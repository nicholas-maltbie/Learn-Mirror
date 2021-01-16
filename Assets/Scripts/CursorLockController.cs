using UnityEngine;

public class CursorLockController : MonoBehaviour
{
    public bool lockedState = false;

    void Update()
    {
        if (Input.GetButtonDown("Cancel"))
        {
            // Toggle state
            lockedState = !lockedState;

            if (lockedState)
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
            else
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
        }
    }
}