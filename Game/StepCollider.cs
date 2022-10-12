using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 階段の処理
/// </summary>
public class StepCollider : MonoBehaviour
{
    [SerializeField] string positionName;
    [SerializeField] Vector2 myPosition;
    [SerializeField] StepCollider toLocation;

    /// <summary>
    /// この階段の位置を取得する
    /// </summary>
    public Vector2 GetPosition()
    {
        return myPosition;
    }
    /// <summary>
    /// この階段のフロア名を取得する
    /// </summary>
    public string GetPositionName()
    {
        return positionName;
    }

    /// <summary>
    /// 階段にプレイヤーが近づいたとき、別の階段に飛ばす処理
    /// </summary>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 6)
        {
            DemoPlayerMove dpm = collision.gameObject.GetComponent<DemoPlayerMove>();
            if (!dpm.GetStepMove())
            {
                dpm.MoveStep();
                dpm.SetNowPositionName(toLocation.GetPositionName(),toLocation.GetPosition());
            }
        }
    }
}
