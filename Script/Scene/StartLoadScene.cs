using UnityEngine;
using UnityEngine.SceneManagement;
/// <summary>
/// スタート画面へシーン移行するためのクラス
/// </summary>
public class StartLoadScene : MonoBehaviour
{
    public void Load()
    {
        SceneManager.LoadScene("Start");
    }
}
