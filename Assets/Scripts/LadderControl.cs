using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LadderControl : MonoBehaviour
{
    [SerializeField] private Transform chController;
    [SerializeField] private PlayerController pControl;
    private float climbSpeed = 3f;
    bool climbMode = false;

    // Start is called before the first frame update
    void Start()
    {
        pControl = GetComponent<PlayerController>();
        climbMode = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Ladder")
        {
            pControl.enabled = false;
            climbMode = !climbMode;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Ladder")
        {
            pControl.enabled = true;
            climbMode = !climbMode;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (climbMode && Input.GetKey(KeyCode.W))
        {
            pControl.transform.position += Vector3.up * climbSpeed * Time.deltaTime;
        }
        if (climbMode && Input.GetKey(KeyCode.S))
        {
            pControl.transform.position += Vector3.down * climbSpeed * Time.deltaTime;
        }
    }
}
