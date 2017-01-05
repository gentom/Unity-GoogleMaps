using UnityEngine;

public class PinchZoom : MonoBehaviour
{
    public float perspectiveZoomSpeed = 0.5f;        // The rate of change of the field of view in perspective mode.
    public float orthoZoomSpeed = 0.5f;        // The rate of change of the orthographic size in orthographic mode.
    Vector3 cekPosMouse;
    Vector2 realtimeCheck;
    int firstTouch = 0;
    
    void Update()
    {
       
        
        // If there are two touches on the device...
        // Store both touches.
       

    }
    void Moving() {
        if (Input.GetMouseButton(0))
        {

            firstTouch++;
            if (firstTouch == 0)
            {

                cekPosMouse = Input.mousePosition;
            }
            Vector2 distance = Input.mousePosition - cekPosMouse;
            if (realtimeCheck == distance)
            {
                Debug.Log("asd");
                return;
            }
            if (distance.y > 20)
            {
                GoogleMap.Instance.Move(distance);

            }
            if (distance.y < -20)
            {
                GoogleMap.Instance.Move(distance);

            }
            if (distance.x > 20)
            {
                GoogleMap.Instance.Move(distance);

            }
            if (distance.x < -20)
            {
                GoogleMap.Instance.Move(distance);

            }
            realtimeCheck = distance;
        }
        else
        {
            firstTouch = 0;
        }
    }
    void touchGeser() {
        if (Input.touchCount == 1)
        {
            Touch touchZero = Input.GetTouch(0);


            // Find the position in the previous frame of each touch.
            Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;

            if (firstTouch == 0)
            {
                firstTouch++;
                realtimeCheck = touchZeroPrevPos;
            }
            Vector2 checkedVector;
            checkedVector = touchZeroPrevPos - realtimeCheck;
            GetComponent<TextMesh>().text = checkedVector + "";

        }
        else
        {
            firstTouch = 0;
        }
    }
    void zoom() {
        if (Input.touchCount == 2)
        {
            // Store both touches.
            Touch touchZero = Input.GetTouch(0);
            Touch touchOne = Input.GetTouch(1);

            // Find the position in the previous frame of each touch.
            Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
            Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;

            // Find the magnitude of the vector (the distance) between the touches in each frame.
            float prevTouchDeltaMag = (touchZeroPrevPos - touchOnePrevPos).magnitude;
            float touchDeltaMag = (touchZero.position - touchOne.position).magnitude;

            // Find the difference in the distances between each frame.
            float deltaMagnitudeDiff = prevTouchDeltaMag - touchDeltaMag;
            float zoom = deltaMagnitudeDiff * orthoZoomSpeed;
            Debug.Log(zoom);
            GetComponent<TextMesh>().text = zoom + "";


        }
    }
}