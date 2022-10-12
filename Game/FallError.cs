using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ミスなどの際に出現するエラーが一定時間後に消える処理。
/// </summary>
public class FallError : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(nameof(Delete));
    }

    IEnumerator Delete()
    {
        yield return new WaitForSeconds(1);
        Destroy(gameObject);
    }
}
