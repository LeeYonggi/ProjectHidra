using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardUI : MonoBehaviour
{
    private Button button = null;

    private bool isEnterCard = false;

    // Start is called before the first frame update
    void Start()
    {
        button = GetComponent<Button>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isEnterCard)
        {
            Debug.Log(Input.mousePosition);
            transform.position = Vector3.Lerp(transform.position, Input.mousePosition, 0.1f);
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
