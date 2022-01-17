using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cam : MonoBehaviour
{
    public int cameraSpeed = 10;
    public int scrollSpeed = 20;
    public int sizeMultiplier = 2;

    void Update() {
        
        float xAxisValue = Input.GetAxis("Horizontal") * cameraSpeed / 1000 * GetComponent<Camera>().orthographicSize;
        float yAxisValue = Input.GetAxis("Vertical") * cameraSpeed / 1000 * GetComponent<Camera>().orthographicSize;
        float zAxisValue = Input.GetAxis("Mouse ScrollWheel") * scrollSpeed;

        if(GetComponent<Camera>().orthographicSize - zAxisValue < 10f * sizeMultiplier || GetComponent<Camera>().orthographicSize - zAxisValue > 100f * sizeMultiplier) {
            zAxisValue = 0;
        }

        GetComponent<Camera>().orthographicSize -= zAxisValue;
        transform.position = new Vector3(transform.position.x + xAxisValue, transform.position.y + yAxisValue, transform.position.z);
    }
}
