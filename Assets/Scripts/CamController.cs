using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamController : MonoBehaviour
{
    [SerializeField] [Range (1,10)] private int rotSpeed = 5;
    [SerializeField] private Transform body;

    private float xRot;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 angles = transform.localRotation.eulerAngles;

        float yRot = (angles.x + 180f) % 360f - 180f;

        xRot += Input.GetAxis("Mouse X") * rotSpeed;
        yRot -= Input.GetAxis("Mouse Y") * rotSpeed;
        yRot = Mathf.Clamp(yRot, -90f, 70f);
        transform.eulerAngles = new Vector3(yRot, xRot, 0);
        transform.localRotation = Quaternion.Euler(new Vector3(yRot, angles.y, angles.z));
        body.eulerAngles = new Vector3(transform.position.y, xRot, 0);

        Quaternion fixedRot = Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0);
        body.rotation = fixedRot;
    }
}
