using UnityEngine;
using System.Collections;

public class Scroll : MonoBehaviour
{

    Vector3 lastMousePos = Vector3.zero;
    Vector3 touchDelta = new Vector3(0f, 0f, 0f);
    bool isScrolling = false;
    public Vector2 maxXAndY;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            {
                isScrolling = true;
                lastMousePos = Input.mousePosition;
                touchDelta = Vector3.zero;
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            isScrolling = false;
            touchDelta = Vector3.zero;
            lastMousePos = Vector3.zero;
        }

        Vector3 newMousePos = Input.mousePosition;
        if (isScrolling)
        {
            touchDelta = (newMousePos - lastMousePos) * 0.01f;
            lastMousePos = newMousePos;
        }

        Camera.main.transform.position -= new Vector3(touchDelta.x, 0f, 0f);  // (touchDelta.x, touchDelta.y, 0f) if we need to scroll by y

        if (Camera.main.transform.position.x > maxXAndY.x + 10)
        {
            Camera.main.transform.position = new Vector3(maxXAndY.x + 10, Camera.main.transform.position.y, Camera.main.transform.position.z);
        }
        else if (Camera.main.transform.position.x < maxXAndY.x)
        {
            Camera.main.transform.position = new Vector3(maxXAndY.x, Camera.main.transform.position.y, Camera.main.transform.position.z);
        }
    }
}