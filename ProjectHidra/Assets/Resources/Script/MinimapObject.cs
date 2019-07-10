using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MinimapObject : MonoBehaviour
{
    public enum MINIMAP_STATE
    {
        TILE,
        BUILDING,
        UNIT
    }

    [SerializeField]
    private MINIMAP_STATE state;
    [SerializeField]
    private GameObject iconPrefab = null;

    private GameObject icon = null;

    
    private GameObject minimap = null;
    
    // Start is called before the first frame update
    void Start()
    {
        minimap = GameObject.FindGameObjectWithTag("Minimap");

        icon = GameObject.Instantiate(iconPrefab, minimap.transform);

        Image iconImage = icon.GetComponent<Image>();

        RectTransform rectIcon = icon.GetComponent<RectTransform>();

        switch (state)
        {
            case MINIMAP_STATE.BUILDING:
                iconImage.sprite = Resources.Load<Sprite>("UI/MiniMap/Icon/spr_Building_Icon");
                iconImage.SetNativeSize();
                rectIcon.SetSiblingIndex(19);
                break;
            case MINIMAP_STATE.UNIT:
                iconImage.sprite = Resources.Load<Sprite>("UI/MiniMap/Icon/spr_Unit_Icon");
                iconImage.SetNativeSize();
                rectIcon.SetAsLastSibling();
                break;
            case MINIMAP_STATE.TILE:
                iconImage.sprite = Resources.Load<Sprite>("UI/MiniMap/Icon/spr_grid_Icon");
                iconImage.SetNativeSize();
                rectIcon.SetAsFirstSibling();
                break;
        }
    }

    void Update()
    {
        icon.transform.localPosition = transform.position * 20;
        icon.transform.Translate(new Vector3(0, 0, (int)state));
    }

    private void OnDestroy()
    {
        if(icon)
            Destroy(icon);
    }
}