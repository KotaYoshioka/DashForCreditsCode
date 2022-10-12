using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// チュートリアルの階段の処理
/// </summary>
public class TutorialStep : MonoBehaviour
{
    [SerializeField] string positionName;
    [SerializeField] Vector2 myPosition;
    [SerializeField] TutorialStep toLocation;

    /// <summary>
    /// 階段にプレイヤーが近づいたときに、移動先を示す
    /// </summary>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 6)
        {
            TutorialPlayer dpm = collision.gameObject.GetComponent<TutorialPlayer>();
            if (!dpm.GetStepMove())
            {
                dpm.MoveStep();
                dpm.SetNowPositionName(toLocation.GetPositionName(), toLocation.GetPosition());
            }
        }
    }

    /// <summary>
    /// 現在地を取得する
    /// </summary>
    public Vector2 GetPosition()
    {
        return myPosition;
    }
    /// <summary>
    /// 現在地のフロア名を取得する
    /// </summary>
    public string GetPositionName()
    {
        return positionName;
    }
}
