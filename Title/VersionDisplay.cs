using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

/// <summary>
/// versionを表示する処理。
/// </summary>
public class VersionDisplay : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI versiontext;
    
    void Start()
    {
        versiontext.text = "version" + Application.version;    
    }
}
