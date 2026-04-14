using UnityEngine;
/// <summary>
/// ステージ1のBGM
/// </summary>
public class Stage1Audio : MonoBehaviour
{
    void Start()
    {
        //BGMをはじめから流す
        AudioManager.Instance.PlayBGM(BGMManager.BGMType.Stage1);
    }
}
