using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sway : MonoBehaviour
{
    public float amount;
    public float maxamount;
    private float realmax;
    [SerializeField] private float ADSDownscale;
    public float smoothamount;
   
    private Vector3 initialPosition;

    // Start is called before the first frame update
    void Start()
    {
        initialPosition = transform.localPosition;
    }

    private void Update()
    {
        realmax = Input.GetKey(KeyCode.Mouse1) ? maxamount / ADSDownscale : maxamount;
    }
    // Update is called once per frame
    void LateUpdate()
    {
            float movementX = Input.GetAxis("Mouse X") * amount;
            float movementY = Input.GetAxis("Mouse Y") * amount;
            movementX = Mathf.Clamp(movementX, -realmax, realmax);
            movementY = Mathf.Clamp(movementY, -realmax, realmax);

            Vector3 finalposition = new Vector3(movementX, movementY, 0);
            transform.localPosition = Vector3.Lerp(transform.localPosition, finalposition + initialPosition, Time.deltaTime * smoothamount);
    }
}
