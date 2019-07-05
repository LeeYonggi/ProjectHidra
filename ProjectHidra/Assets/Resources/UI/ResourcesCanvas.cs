using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResourcesCanvas : MonoBehaviour
{
    [SerializeField]
    private Text mineralUI = null;

    [SerializeField]
    private Text maxStructureUI = null;

    [SerializeField]
    private Text nowStructureUI = null;

    [SerializeField]
    private Text messageUI = null;

    private Color messageColor;

    private ResourcesStatus resourcesStatus = null;
    // Start is called before the first frame update
    void Start()
    {
        messageColor = messageUI.color;

        resourcesStatus = GetComponent<ResourcesStatus>();
    }

    // Update is called once per frame
    void Update()
    {
        mineralUI.text = resourcesStatus.Mineral.ToString();
        maxStructureUI.text = resourcesStatus.MaxStructure.ToString();
        nowStructureUI.text = resourcesStatus.NowStructure.ToString();
    }

    void ChangeMessageText(string str)
    {
        messageUI.text = str;
        StopCoroutine("MessageCoroutine");
        StartCoroutine("MessageCoroutine");
    }

    IEnumerator MessageCoroutine()
    {
        yield return new WaitForSeconds(2.0f);
        messageUI.text = string.Empty;
    }
}
