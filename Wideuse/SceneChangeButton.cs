using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// 設定されたscenenameのシーンに飛ぶだけの汎用的なボタン。
/// </summary>
public class SceneChangeButton : MonoBehaviour
{
    [SerializeField] string scenename;

    public void OnClick()
    {
        SceneManager.LoadScene(scenename);
    }
}
