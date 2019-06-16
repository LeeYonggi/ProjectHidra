using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseManager : MonoBehaviour
{
    private static MouseManager instance = null;

    [SerializeField]
    private Camera mainCamera = null;
    

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

    }

    public Ray2D GetTouchRay2D()
    {
        Vector3 screenPos = mainCamera.ScreenToWorldPoint(Input.GetTouch(0).position);
        Ray2D hit = new Ray2D(screenPos, Vector2.zero);
        return hit;
    }
    public Ray2D GetMouseRay2D()
    {
        Vector2 worldPoint = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        Ray2D hit = new Ray2D(worldPoint, Vector2.zero);
        return hit;
    }
}
