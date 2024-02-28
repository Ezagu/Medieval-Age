using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovimientoCamara : MonoBehaviour
{
    [SerializeField]
    private Camera cam;

    Vector3 touchStart;
    public float zoomMinimo = 1;
    public float zoomMaximo = 8;

    public int edgeScrollingSize;
    public float scrollSpeedCamera;
    public float dragSpeedCamera;

    public bool arrastrandoAldeano;

    [SerializeField]
    private SpriteRenderer mapRenderer;

    private float mapMinX, mapMaxX, mapMinY, mapMaxY;

    private void Awake()
    {
        //Define limites del suelo para despues poder limitar camara
        mapMinX = mapRenderer.transform.position.x - mapRenderer.bounds.size.x / 2f;
        mapMaxX = mapRenderer.transform.position.x + mapRenderer.bounds.size.x / 2f;

        mapMinY = mapRenderer.transform.position.y - mapRenderer.bounds.size.y / 2f;
        mapMaxY = mapRenderer.transform.position.y + mapRenderer.bounds.size.y / 2f;
    }

    void Update () {

        //arrastras camara al presionar el borde de la pantalla
        Vector3 inputDir = new Vector3(0, 0, 0);

        if (Input.mousePosition.x < edgeScrollingSize)
        {
            inputDir.x = -1f;
        }
        if (Input.mousePosition.y < edgeScrollingSize)
        {
            inputDir.z = -1f;
        }
        if (Input.mousePosition.x > Screen.width - edgeScrollingSize)
        {
            inputDir.x = +1f;
        }
        if (Input.mousePosition.y > Screen.height - edgeScrollingSize)
        {
            inputDir.z = +1f;
        }

        Vector3 moveDir = transform.up * inputDir.z + transform.right * inputDir.x;

        transform.position += moveDir * scrollSpeedCamera * Time.deltaTime;

        //Agarra posicion inicial de la camara al apretar mouse izquierdo o central
        if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(2) )
        {
            touchStart = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }

        //hacer zoom arrastrando los dedos
        if(Input.touchCount == 2){
            Touch touchZero = Input.GetTouch(0);
            Touch touchOne = Input.GetTouch(1);

            Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
            Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;

            float prevMagnitude = (touchZeroPrevPos - touchOnePrevPos).magnitude;
            float currentMagnitude = (touchZero.position - touchOne.position).magnitude;

            float difference = currentMagnitude - prevMagnitude;

            zoom(difference * 0.01f);
        }

        //se moviliza la camara al arrastrar
        else if(!arrastrandoAldeano && Input.GetMouseButton(0) || Input.GetMouseButton(2))
        {
            Vector3 direction = touchStart - Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Camera.main.transform.position += direction * dragSpeedCamera * Time.deltaTime;
        }
        zoom(Input.GetAxis("Mouse ScrollWheel"));

        //Define si se sobrepasó los limites de la camara y devuelve nuevo vector dentro de los limites
        transform.position = ClampCamera(transform.position);
    }

    void zoom(float increment){
        Camera.main.orthographicSize = Mathf.Clamp(Camera.main.orthographicSize - increment, zoomMinimo, zoomMaximo);
    }

    private Vector3 ClampCamera(Vector3 targetPosition)
    {
        //limita la camara a los limites del suelo
        float camHeight = cam.orthographicSize;
        float camWidth =cam.orthographicSize * cam.aspect;

        float minX = mapMinX + camWidth;
        float maxX = mapMaxX - camWidth;
        float minY = mapMinY + camHeight;
        float maxY = mapMaxY - camHeight;

        float newX = Mathf.Clamp(targetPosition.x, minX, maxX);
        float newY = Mathf.Clamp(targetPosition.y, minY, maxY);

        return new Vector3(newX, newY, targetPosition.z);
    }
}

