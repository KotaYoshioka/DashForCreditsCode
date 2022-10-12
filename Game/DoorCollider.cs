using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ドアの当たり判定
/// </summary>
public class DoorCollider : MonoBehaviour
{
    [SerializeField] string roomName;

    /// <summary>
    /// ドアにプレイヤーが近づいたとき、そのプレイヤーに部屋名を送る。
    /// </summary>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 6)
        {
            collision.gameObject.GetComponent<DemoPlayerMove>().SendNearRoomName(roomName);
        }        
    }

    /// <summary>
    /// プレイヤーがドアから遠のいたとき、部屋名を隠す。
    /// </summary>
    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.gameObject.layer == 6)
        {
            collision.gameObject.GetComponent<DemoPlayerMove>().SendCloseRoom();
        }
    }
}
