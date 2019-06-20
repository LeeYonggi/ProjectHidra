using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardUI : MonoBehaviour
{
    private Button button = null;

    private bool isEnterCard = false;

    // 자신 이미지
    private Image image = null;

    // 캔버스
    [SerializeField]
    private Canvas canvas = null;

    // 타겟
    [SerializeField]
    private GameObject targetPrefab;
    private Sprite targetSprite = null;

    // 잔상
    private SpriteRenderer afterRenderer = null;
    private GameObject afterImageObject = null;

    private Vector2 basicPosition;

    // 건물
    private GameObject structure = null;

    // Start is called before the first frame update
    void Start()
    {
        button = GetComponent<Button>();
        image = GetComponent<Image>();
        targetSprite = targetPrefab.GetComponent<SpriteRenderer>().sprite;
        afterImageObject = GameObject.FindGameObjectsWithTag("AfterImage")[0];
        afterRenderer = afterImageObject.GetComponent<SpriteRenderer>();

        basicPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (isEnterCard)
        {
            Debug.Log(Input.mousePosition);
            transform.position = Vector3.Lerp(transform.position, Input.mousePosition, 5 * Time.deltaTime);
            float distance = Vector2.Distance(new Vector2(basicPosition.x, transform.position.y), basicPosition);
            transform.localScale = new Vector2(Mathf.Max(0, 1 - distance * 0.004f), Mathf.Max(0, 1 - distance * 0.004f));

            if (Input.GetMouseButtonUp(0))
            {
                OnPointerUp();
            }

            PointPress();
        }
        else
        {
            transform.localScale = Vector3.Lerp(transform.localScale, new Vector3(1, 1), 5 * Time.deltaTime);
            transform.position = Vector3.Lerp(transform.position, basicPosition, 5 * Time.deltaTime);
        }
    }

    public void OnPointerDown()
    {
        isEnterCard = true;
    }

    public void OnPointerUp()
    {
        isEnterCard = false;
        if (structure)
            structure.GetComponent<Tile>().structure = Instantiate(targetPrefab, structure.transform);
    }

    void PointPress()
    {
        // 타일 충돌
        if (Input.GetMouseButton(0) && transform.position.y > 100)
        {
            Ray2D ray = MouseManager.Instance.GetMouseRay2D();
            RaycastHit2D[] hit = Physics2D.RaycastAll(ray.origin, ray.direction);

            for (int i = 0; i < hit.Length; i++)
            {
                if (hit[i].collider != null && hit[i].collider.tag == "Tile")
                {
                    afterRenderer.sprite = targetSprite;
                    afterImageObject.transform.position = hit[i].collider.transform.position;

                    image.enabled = false;
                    afterImageObject.SetActive(true);

                    structure = hit[i].collider.gameObject;
                }
            }
        }
        else
        {
            image.enabled = true;
            afterImageObject.SetActive(false);
            structure = null;
        }
    }
}
