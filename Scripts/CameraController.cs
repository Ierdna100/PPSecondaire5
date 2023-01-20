using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SelectionBaseAttribute]
public class CameraController : MonoBehaviour
{
    public Camera camera;
    public float cameraZoom;
    public float cameraSpeed = 3f;
    public float cameraZoomStep = 2f;
    public float maxCameraZoom = 32f;
    public float minCameraZoom = 1f;
    public float defaultZoom = 16f;
    private float timeAtLastMouseInput;
    private float horizontalInput = 0;
    private float verticalInput = 0;

    public float cameraMaxXPos;
    public float cameraMinXPos;
    public float cameraMaxYPos;
    public float cameraMinYPos;

    private void Awake()
    {
        camera.orthographicSize = defaultZoom;
    }

    private void Update()
    {
        //sets vertical speed
        if (Input.GetKey("w") && !Input.GetKey("s"))
        { //when W is pressed
            verticalInput += 0.05f;
            Mathf.Min(verticalInput, 1f);
        }
        else if (Input.GetKey("s") && !Input.GetKey("w"))
        { //when S is pressed
            verticalInput -= 0.05f;
            Mathf.Max(verticalInput, -1f);
        }
        else if ((transform.position.y <= cameraMinYPos && verticalInput < 0f) || (transform.position.y >= cameraMaxYPos && verticalInput > 0f))
        { //when clipping the world edges
            verticalInput = 0f;
        }
        else
        { //everytime else
            verticalInput += 0.05f * -Mathf.Sign(verticalInput);
            if (Mathf.Abs(verticalInput) < 0.07f)
                verticalInput = 0f;
        }

        //sets horizontal speed
        if (Input.GetKey("d") && !Input.GetKey("a"))
        { //when D is pressed
            horizontalInput += 0.05f;
            Mathf.Min(horizontalInput, 1f);
        } 
        else if (Input.GetKey("a") && !Input.GetKey("d"))
        { //when A is pressed
            horizontalInput -= 0.05f;
            Mathf.Max(horizontalInput, -1f);
        }
        else if ((transform.position.x <= cameraMinXPos && horizontalInput < 0f) || (transform.position.x >= cameraMaxXPos && horizontalInput > 0f))
        { //when clipping the world edges
            horizontalInput = 0f;
        }
        else
        { //everytime else
            horizontalInput += 0.05f * -Mathf.Sign(horizontalInput);
            if (Mathf.Abs(horizontalInput) < 0.07f)
                horizontalInput = 0f;
        }

        transform.position += new Vector3(horizontalInput * cameraSpeed * camera.orthographicSize, verticalInput * cameraSpeed * camera.orthographicSize, 0) * Time.deltaTime;
        transform.position = new Vector3(Mathf.Clamp(transform.position.x, cameraMinXPos, cameraMaxXPos), Mathf.Clamp(transform.position.y, cameraMinYPos, cameraMaxYPos), transform.position.z);
        //W = up, S = down, D = right, A = left

        float mouseWheel = Input.GetAxis("Mouse ScrollWheel");
        if (mouseWheel != 0 && Time.time - timeAtLastMouseInput > 0.05f)
        {
            if (mouseWheel > 0)
            {
                camera.orthographicSize = Mathf.Clamp(camera.orthographicSize / cameraZoomStep, minCameraZoom, maxCameraZoom);
            } else if (mouseWheel < 0)
            {
                camera.orthographicSize = Mathf.Clamp(camera.orthographicSize * cameraZoomStep, minCameraZoom, maxCameraZoom);
            }
            
            timeAtLastMouseInput = Time.time;
        }
    }
}
