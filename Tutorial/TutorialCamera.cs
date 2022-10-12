using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// チュートリアル用のカメラ。プレイヤーを追う。
/// </summary>
public class TutorialCamera : MonoBehaviour
{
    [SerializeField] GameObject player;
    
    void Update()
    {
        if (player != null)
        {
            Vector3 v = player.GetComponent<Transform>().position;
            GetComponent<Transform>().position = new Vector3(v.x, v.y, -10);
        }
    }
}
