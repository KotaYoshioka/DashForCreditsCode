using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// チュートリアル用のドアの判定。
/// </summary>
public class TutorialDoor : MonoBehaviour
{
    [SerializeField] string roomName;

    /// <summary>
    /// 近くにプレイヤーが来たら、部屋の名前を送る。
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 6)
        {
            collision.gameObject.GetComponent<TutorialPlayer>().SendNearRoomName(roomName);
        }
    }

    /// <summary>
    /// プレイヤーが離れたら、部屋の名前を閉じる。
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 6)
        {
            collision.gameObject.GetComponent<TutorialPlayer>().SendCloseRoom();
        }
    }
}
