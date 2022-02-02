using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PopUp : MonoBehaviour
{
    public bool enableAutoHide;
    public float hideTime;
    public TextMeshProUGUI textMessage;

    private void OnEnable()
    {
        if (enableAutoHide)
            StartCoroutine(AutoHide());
    }

    IEnumerator AutoHide()
    {
        yield return new WaitForSeconds(hideTime);

        textMessage.text = string.Empty;
        this.gameObject.SetActive(false);
    }
}
