using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldManager : MonoBehaviour
{
    [SerializeField]
    private GameObject tilePrefab;

    private List<GameObject> tileList = new List<GameObject>();

    [SerializeField]
    private Vector2 tileSize;

    private Vector2 tileSpriteSize = new Vector2(0, 0);


    // Start is called before the first frame update
    void Start()
    {
        tileSpriteSize = tilePrefab.GetComponent<SpriteRenderer>().bounds.size;
        tileSpriteSize.x *= 0.7f;
        PlaceTiles(transform.position, tileSize, tileSpriteSize);
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonUp(0))
        {
            Ray2D ray = MouseManager.Instance.GetMouseRay2D();
            RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction);

            if(hit.collider != null)
                Debug.Log(hit.collider.gameObject);
        }

    }

    void PlaceTiles(Vector3 pivot, Vector2 size, Vector2 spriteSize)
    {
        for (int x = 0; x < size.x; x++)
        {
            for (int y = 0; y < size.x; y++)
            {
                GameObject obj = Instantiate(tilePrefab, transform);
                Vector2 firstPosition = size * spriteSize;

                firstPosition = Vector2.zero - (firstPosition * 0.5f);

                float width  = spriteSize.x * x;
                float height = spriteSize.y * y;
                if (x % 2 == 1)
                    height = height - spriteSize.y * 0.5f;

                obj.transform.position = new Vector3(firstPosition.x + width,
                    firstPosition.y + height, transform.position.z);

            }
        }
    }
}
