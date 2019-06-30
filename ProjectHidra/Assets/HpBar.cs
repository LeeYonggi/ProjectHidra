using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HpBar : MonoBehaviour
{
    [SerializeField]
    private GameObject hp;
    [SerializeField]
    private GameObject mask;
    [SerializeField]
    private GameObject parent;

    private ObjectStatus parentStatus;
    private SpriteRenderer parentSprite;

    // Start is called before the first frame update
    void Start()
    {
        parentStatus = parent.GetComponent<ObjectStatus>();

        parentSprite = parent.GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        MoveToParentPosition();
        MoveHpPosition();
    }

    public void AdjustHp(float maxHp, float nowHp)
    {
        Vector3 maskPosition = mask.transform.localPosition;

        maskPosition.x = ((nowHp / maxHp) * 100.0f) - 100.0f;
        mask.transform.localPosition = maskPosition;
    }

    public void MoveToParentPosition()
    {
        float height = parentSprite.bounds.size.y;

        transform.position = Camera.main.WorldToScreenPoint(parent.transform.position + new Vector3(0, height * 0.7f));

        AdjustHp(parentStatus.basicHp, parentStatus.Hp);
    }

    private void MoveHpPosition()
    {
        Vector3 hpPosition = hp.transform.localPosition;

        hpPosition.x = -mask.transform.localPosition.x;
        hp.transform.localPosition = hpPosition;
    }
}
