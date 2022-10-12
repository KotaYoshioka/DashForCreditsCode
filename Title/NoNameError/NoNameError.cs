using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// エラー文が一定時間で消える処理
/// </summary>
public class NoNameError : MonoBehaviour
{
    void Start()
    {
        gameObject.transform.parent =  GameObject.Find("Canvas").transform;
        gameObject.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 0);
        StartCoroutine(nameof(SelfDelete));
    }

    IEnumerator SelfDelete()
    {
        yield return new WaitForSeconds(1);
        GameObject.Destroy(gameObject);
    }
}
