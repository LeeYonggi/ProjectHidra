using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MouseManager : MonoBehaviour
{
    private static MouseManager instance = null;

    [SerializeField]
    private Camera mainCamera = null;

    // 스크롤 관련
    bool isDown = false;
    Vector3 startMousePosition = Vector3.zero;
    Vector3 startCameraPosition;
    

    public static MouseManager Instance
    {
        get
        {
            if (!instance)
                return null;
            return instance;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        BackGroundMouseScroll();
    }

    public Ray2D GetTouchRay2D(Vector2 position)
    {
        Vector3 screenPos = mainCamera.ScreenToWorldPoint(position);
        Ray2D hit = new Ray2D(screenPos, Vector2.zero);
        return hit;
    }

    public Ray2D GetMouseRay2D(Vector2 position)
    {
        Vector2 worldPoint = mainCamera.ScreenToWorldPoint(position);
        Ray2D hit = new Ray2D(worldPoint, Vector2.zero);
        return hit;
    }

    public void BackGroundMouseScroll()
    {
        if(Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
        {
            isDown = true;
            startMousePosition = Input.mousePosition;
            startCameraPosition = mainCamera.transform.position;
        }
        if(Input.GetMouseButtonUp(0) && isDown == true)
        {
            isDown = false;
        }
        if(isDown == true)
        {
            Vector3 movePosition = Input.mousePosition - startMousePosition;
            movePosition = startCameraPosition - movePosition * 0.01f;
            mainCamera.transform.position = movePosition;
        }
    }
}
