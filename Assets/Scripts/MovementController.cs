using Unity.VisualScripting;
using UnityEngine;

public class MovementController : MonoBehaviour
{
    public Vector2 startPos;
    public Vector2 endPos;
    public bool fingerHold = false;
    public float speed = 4;
    Vector2 f0start;

    Vector2 f1start;
    private Vector3 dragOrigin;
    // 1,8:4
    // За 1 size лимит изменяется на (0,45, 1)
    public float CameraYLimitUp = 1.6f;
    public float CameraYLimitDown = -9.2f; // -5,2 при 7
    public float CameraXLimitRight = 5.65f; // 3,85 при 7 
    public float CameraXLimitLeft = -6.3f;
    Camera camera;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Input.simulateMouseWithTouches = true;
        camera = GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0)) { 
            dragOrigin = camera.ScreenToWorldPoint(Input.mousePosition);
        }
        if (Input.GetMouseButton(0))
        {
            Vector3 delta = dragOrigin - camera.ScreenToWorldPoint(Input.mousePosition);
            if (delta.x < 0 && transform.position.x >= CameraXLimitLeft + 0.45f * (Mathf.RoundToInt(camera.orthographicSize) - 3))
            {
                transform.position += new Vector3(delta.x, 0) * Time.deltaTime * speed;
            }
            else if (delta.x > 0 && transform.position.x <= CameraXLimitRight - 0.45f * (Mathf.RoundToInt(camera.orthographicSize) - 3))
            {
                transform.position += new Vector3(delta.x, 0) * Time.deltaTime * speed;
            }
            if (delta.y < 0 && transform.position.y >= CameraYLimitDown + 0.75f * (Mathf.RoundToInt(camera.orthographicSize) - 3))
            {
                transform.position += new Vector3(0, delta.y) * Time.deltaTime * speed;
            }
            else if (delta.y > 0 && transform.position.y <= CameraYLimitUp - (Mathf.RoundToInt(camera.orthographicSize) - 3))
            {
                transform.position += new Vector3(0, delta.y) * Time.deltaTime * speed;
            }
        }


        if (Input.touchCount > 0)
        {

            print("touch");
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began)
            {
                startPos = touch.position;
                endPos = touch.position;
                fingerHold = true;
            }
            else if (touch.phase == TouchPhase.Moved)
            {
                endPos = touch.position;
            }
            else if(touch.phase == TouchPhase.Ended)
            {
                fingerHold = false;
            }
        }
        if (fingerHold)
        {
            Vector2 delta = startPos - endPos;
            delta.x = delta.x / (delta.x != 0 ? Mathf.Abs(delta.x) : 1);
            delta.y = delta.y / (delta.y != 0 ? Mathf.Abs(delta.y) : 1);
            print(delta);

            if(delta.x < 0 && transform.position.x >= CameraXLimitLeft + 0.45f * (Mathf.RoundToInt(camera.orthographicSize) - 3))
            {
                transform.position += new Vector3(delta.x, 0) * Time.deltaTime * speed;
            }
            else if(delta.x > 0 && transform.position.x <= CameraXLimitRight - 0.45f * (Mathf.RoundToInt(camera.orthographicSize) - 3))
            {
                transform.position += new Vector3(delta.x, 0) * Time.deltaTime * speed;
            }
            if (delta.y < 0 && transform.position.y >= CameraYLimitDown + 0.75f * (Mathf.RoundToInt(camera.orthographicSize) - 3))
            {
                transform.position += new Vector3(0, delta.y) * Time.deltaTime * speed;
            }
            else if (delta.y > 0 && transform.position.y <= CameraYLimitUp - (Mathf.RoundToInt(camera.orthographicSize) - 3))
            {
                transform.position += new Vector3(0, delta.y) * Time.deltaTime * speed;
            }


        }
        if (Input.mouseScrollDelta != Vector2.zero)
        {
            float y = -Input.mouseScrollDelta.y;
            if (y < 0 && camera.orthographicSize +  y >= 3)
                camera.orthographicSize += y;
            else if( y< 0) camera.orthographicSize = 3;
            if (y > 0 && camera.orthographicSize + y <= 7)
            {
                camera.orthographicSize +=y;
            }
            else if(y> 0) { camera.orthographicSize = 7; }
        }
        if (Input.touchCount < 2)
        {
            f0start = Vector2.zero;

            f1start = Vector2.zero;

        }

        if (Input.touchCount == 2) Zoom();
    }
    void Zoom()

    {

        if (f0start == Vector2.zero && f1start == Vector2.zero)

        {

            f0start = Input.GetTouch(0).position;

            f1start = Input.GetTouch(1).position;

        }

        Vector2 f0position = Input.GetTouch(0).position;

        Vector2 f1position = Input.GetTouch(1).position;

        float dir = Mathf.Sign(Vector2.Distance(f1start, f0start) - Vector2.Distance(f0position, f1position));
        dir *= Time.deltaTime;

        if (dir < 0 && camera.orthographicSize + dir >= 3)
        {
            camera.orthographicSize += dir;
        }
        else if(dir < 0)
        {
            camera.orthographicSize = 3;
        }
        if(dir > 0 && camera.orthographicSize + dir <= 7)
        {
            camera.orthographicSize += dir;
        }
        else if (dir > 0)
        {
            camera.orthographicSize = 7;
        }
        GetComponent<Camera>().orthographicSize += dir;

    }

}
