using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldManager : MonoBehaviour
{
    [SerializeField]
    private GameObject tilePrefab;

    private List<List<GameObject>> tileList = new List<List<GameObject>>();

    [SerializeField]
    private Vector2 tileSize;

    private Vector2 tileSpriteSize = new Vector2(0, 0);

    public List<List<GameObject>> TileList { get => tileList; set => tileList = value; }


    // Start is called before the first frame update
    void Awake()
    {
        tileSpriteSize = tilePrefab.GetComponent<SpriteRenderer>().bounds.size;
        tileSpriteSize.x *= 0.7f;
        PlaceTiles(transform.position, tileSize, tileSpriteSize);
        tileList[3][3].GetComponent<Tile>().IsWall = true;
        tileList[3][1].GetComponent<Tile>().IsWall = true;
    }

    // Update is called once per frame
    void Update()
    {
    }

    // 타일 배치
    void PlaceTiles(Vector3 pivot, Vector2 size, Vector2 spriteSize)
    {
        for (int x = 0; x < size.x; x++)
        {
            int sizeY = (int)(size.y) - Mathf.Abs((int)(size.y * 0.5f) - x);
            tileList.Add(new List<GameObject>());

            int emptyNum = (int)size.y - sizeY;
            int empty = emptyNum;
            for (empty = emptyNum; empty > emptyNum * 0.5f; empty--)
            {
                tileList[x].Add(null);
            }
            for (int y = 0; y < sizeY; y++)
            {
                GameObject obj = Instantiate(tilePrefab, transform);
                float firstPosition = 0 - (size.x * spriteSize.x * 0.4f);

                float width  = spriteSize.x * x;
                float height = spriteSize.y * y;
                
                height = height - spriteSize.y * sizeY * 0.5f;

                obj.transform.position = new Vector3(firstPosition + width,
                    spriteSize.y * 0.5f + height, transform.position.z);

                tileList[x].Add(obj);
            }
            for (empty = emptyNum; empty > emptyNum * 0.5f; empty--)
            {
                tileList[x].Add(null);
            }
        }
    }
    
}
