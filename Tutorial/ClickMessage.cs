using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// チュートリアルのメッセージウィンドウの処理。
/// クリックされたら、次のメッセージに移行する。
/// </summary>
public class ClickMessage : MonoBehaviour
{
    [SerializeField] TutorialScript ts;

    private void Update()
    {
        if (ts.next && Input.GetMouseButtonDown(0))
        {
            ts.Phase();
        }
    }
}
