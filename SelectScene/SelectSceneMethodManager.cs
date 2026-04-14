using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
/// <summary>
/// 選択画面シーンでのシーン移行とゲーム終了の関数を管理するクラス
/// </summary>
public class SelectSceneMethodManager : MonoBehaviour
{
    //スタートボタンを押すと、Stage1のシーンに移行する
    public void StartButtonPush()
    {
        StartCoroutine(SceneChange1());
    }
    IEnumerator SceneChange1()
    {
        float interval = 2.5f;
        AudioManager.Instance.StopBGM();
        yield return new WaitForSeconds(interval);
        SceneManager.LoadScene("Stage1");
    }

    //クレジットボタンを押すと、CreditSceneのシーンに移行する
    public void CreditButtonPush()
    {
        StartCoroutine(SceneChange2());
    }
    IEnumerator SceneChange2()
    {
        float interval = 2.5f;
        yield return new WaitForSeconds(interval);
        SceneManager.LoadScene("CreditScene");
    }

    //チュートリアルボタンを押すと、TutorialSceneのシーンに移行する
    public void TutorialButtonPush()
    {
        StartCoroutine(SceneChange3());
    }

    IEnumerator SceneChange3()
    {
        float interval = 2.5f;
        yield return new WaitForSeconds(interval);
        SceneManager.LoadScene("TutorialScene");
    }

    //終了ボタンを押すと、ゲームを終了する
    public void EndButtonPush()
    {
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }
}
