using UnityEngine;
using UnityEngine.SceneManagement;
/// <summary>
/// クリアオブジェクトにアタッチする、ゲームクリアをした際にシーン移行するためのクラス
/// </summary>
public class ClearLoadScene : MonoBehaviour
{
    //Playerタグに触れた際にClearシーンへ移行する
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            SceneManager.LoadScene("Clear");
        }
    }
}
