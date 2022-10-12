using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 実際のゲーム画面で各プレイヤーをカメラで追いかける処理
/// </summary>
public class CameraScript : MonoBehaviour
{

    GameObject player = null;
    
    /// <summary>
    /// カメラがプレイヤーを追いかける処理
    /// </summary>
    void Update()
    {
        if(player != null)
        {
            Vector3 v = player.GetComponent<Transform>().position;
            GetComponent<Transform>().position = new Vector3(v.x, v.y, -10);
        }
    }

    /// <summary>
    /// 追いかけるプレイヤーを指定する
    /// </summary>
    public void SetPlayer(GameObject player)
    {
        this.player = player;
    }
}
