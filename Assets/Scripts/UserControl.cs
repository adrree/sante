using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class UserControl : MonoBehaviour
{

    public float linearSpeed = 1;
    public float angularSpeed = 90;
    private Rigidbody rb;
    public float forceGain = 100000;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Confined;
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        /*float dx = Mathf.Clamp(Input.GetAxis("Mouse X"), -1, 1);
        float dy = Mathf.Clamp(Input.GetAxis("Mouse Y"), -1, 1);
        Vector3 moveDirection = Vector3.forward * dy + Vector3.right * dx;
        this.transform.Translate(linearSpeed * Time.deltaTime * moveDirection);
        this.transform.Rotate(Vector3.up, angularSpeed * Time.deltaTime * dx, Space.World);
        Vector3 moveDirection = Vector3.forward * dy;
        this.transform.Translate(linearSpeed * Time.deltaTime * moveDirection);*/
    }

    private void FixedUpdate()
    {
        if (rb != null)
        {
            float dx = Mathf.Clamp(Input.GetAxis("Mouse X"), -1, 1);
            float dy = Mathf.Clamp(Input.GetAxis("Mouse Y"), -1, 1);
            this.transform.Rotate(Vector3.up, angularSpeed * Time.deltaTime * dx, Space.World);
            Vector3 moveDirection = Vector3.forward * dy;
            rb.AddRelativeForce(forceGain * Time.fixedDeltaTime * moveDirection);
        }
    }

}
