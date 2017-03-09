using UnityEngine;
using System.Collections;

public class CameraScript : MonoBehaviour
{
    // двигаем камеру
    private bool drag = false;

    // экранные координаты начальной точки касания
    private Vector3 initialTouchPosition;
    // мировые координаты камеры при инициировании
    // перемещения/масштабирования
    private Vector3 initialCameraPosition;

    // экранные координаты начальной точки первого касания
    private Vector3 initialTouch0Position;
    public Vector2 maxXAndY;

    void Update()
    {

        if (Input.touchCount == 1)
        {
            Touch touch0 = Input.GetTouch(0);

            if (IsTouching(touch0))
            {
                if (!drag)
                {
                    initialTouchPosition = touch0.position;
                    initialCameraPosition = this.transform.position;

                    drag = true;
                }
                else
                {
                    Vector2 delta = Camera.main.ScreenToWorldPoint(touch0.position) -
                                    Camera.main.ScreenToWorldPoint(initialTouchPosition);

                    Vector3 newPos = initialCameraPosition;
                    newPos.x -= delta.x;

                    this.transform.position = newPos;
                }
            }

            if (!IsTouching(touch0))
            {
                drag = false;
            }
        }
        else
        {
            drag = false;
        }

        if (Camera.main.transform.position.x > maxXAndY.x + 10)
        {
            Camera.main.transform.position = new Vector3(maxXAndY.x + 10, Camera.main.transform.position.y, Camera.main.transform.position.z);
        }
        else if (Camera.main.transform.position.x < maxXAndY.x)
        {
            Camera.main.transform.position = new Vector3(maxXAndY.x, Camera.main.transform.position.y, Camera.main.transform.position.z);
        }
    }

    static bool IsTouching(Touch touch)
    {
        return touch.phase == TouchPhase.Began ||
                touch.phase == TouchPhase.Moved ||
                touch.phase == TouchPhase.Stationary;
    }
}