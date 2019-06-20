using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardUI : MonoBehaviour
{
    private Button button = null;

    private bool isEnterCard = false;

    private Image image = null;

    // 자신 이미지
    private Sprite uiSprite = null;

    // 캔버스
    [SerializeField]
    private Canvas canvas = null;

    // 타겟
    [SerializeField]
    private GameObject targetObj;
    private Sprite targetSprite = null;

    // 잔상
    private SpriteRenderer afterRenderer = null;
    private GameObject afterImageObject = null;

    // Start is called before the first frame update
    void Start()
    {
        button = GetComponent<Button>();
        image = GetComponent<Image>();
        targetSprite = targetObj.GetComponent<SpriteRenderer>().sprite;
        uiSprite = image.sprite;
        afterImageObject = GameObject.FindGameObjectsWithTag("AfterImage")[0];
        afterRenderer = afterImageObject.GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isEnterCard)
        {
            Debug.Log(Input.mousePosition);
            transform.position = Vector3.Lerp(transform.position, Input.mousePosition, 0.1f);
            if(Input.GetMouseButton(0) && transform.position.y > 100)
            {
                Ray2D ray = MouseManager.Instance.GetMouseRay2D();
                RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction);
                if (hit.collider != null && hit.collider.tag == "Tile")
                {
                    image.sprite = targetSprite;
                    afterRenderer.sprite = targetSprite;
                    afterImageObject.transform.position = hit.collider.transform.position;
                    afterImageObject.SetActive(true);
                }
            }
            if(Input.GetMouseButtonUp(0))
            {
                OnPointerUp();
            }
        }
    }

    public void OnPointerDown()
    {
        isEnterCard = true;

    }

    public void OnPointerUp()
    {
        isEnterCard = false;

    }
}
