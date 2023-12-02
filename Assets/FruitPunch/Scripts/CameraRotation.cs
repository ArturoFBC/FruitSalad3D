using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRotation : MonoBehaviour
{
    [SerializeField] private Vector2 maxAngle;
    [SerializeField] private Vector2 minAngle;
    [SerializeField] private Vector2 moveSpeed;

    private Vector2 previousPointerPosition;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetAxis("Fire2") > 0)
        {
            if (previousPointerPosition != Vector2.zero)
            {
                Vector2 pointerDeltaPosition = (Vector2)Input.mousePosition - previousPointerPosition;
                ManageMovement(pointerDeltaPosition);
            }
            previousPointerPosition = Input.mousePosition;
        }
        else if (previousPointerPosition != Vector2.zero)
            previousPointerPosition = Vector2.zero;
    }

    private void ManageMovement(Vector2 moveDirection)
    {
        Vector2 movement = Vector2.zero;

        movement.y = moveDirection.y * moveSpeed.y;
        movement.x = moveDirection.x * moveSpeed.x;

        Vector3 currentRotation = transform.rotation.eulerAngles;
        if (currentRotation.y > maxAngle.x + 1)
            currentRotation.y -= 360;
        if (currentRotation.x > maxAngle.y + 1)
            currentRotation.x -= 360;
        Vector3 targetRotation = new Vector3(currentRotation.x + movement.y, currentRotation.y + movement.x, 0f);

        targetRotation.y = Mathf.Clamp(targetRotation.y, minAngle.x, maxAngle.x);
        targetRotation.x = Mathf.Clamp(targetRotation.x, minAngle.y, maxAngle.y);

        Quaternion targetQuaternion = new Quaternion();
        targetQuaternion.eulerAngles = targetRotation;
        transform.rotation = targetQuaternion;
    }
}
