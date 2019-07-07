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

    [SerializeField]
    private Text timerUI = null;

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
        mineralUI.text      = resourcesStatus.Mineral.ToString();
        maxStructureUI.text = resourcesStatus.MaxStructure.ToString();
        nowStructureUI.text = resourcesStatus.NowStructure.ToString();
        timerUI.text        = GameManager.Instance.Timer.ToString();
    }

    public void ChangeMessageText(string str, float time)
    {
        StartCoroutine(MessageCoroutine(str, time));
    }

    public void ChangeMessageText(string str)
    {
        messageUI.text = str;
    }

    public void ChangeMessageTextSize()
    {

    }

    IEnumerator MessageCoroutine(string str, float time)
    {
        messageUI.text = str;
        yield return new WaitForSeconds(2.0f);
        messageUI.text = string.Empty;
    }
}
