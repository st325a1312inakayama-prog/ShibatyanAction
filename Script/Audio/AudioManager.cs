using UnityEngine;
using static BGMManager;
using static SEManager;
/// <summary>
/// ゲーム全体のBGMとSEを管理するオーディオの窓口クラス(このクラス経由で他のスクリプトで音を鳴らせるようにする)
/// </summary>
public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;//どこからでも参照できるAudioManagerの実体

    [SerializeField] BGMManager bgmManager;//BGM再生管理クラス
    [SerializeField] SEManager seManager; //SE再生管理クラス

    void Awake()
    {
        //AudioManagerはシーンに1つだけ存在させる
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        // シーン切り替え時も破棄されないようにする
        DontDestroyOnLoad(gameObject);
    }

    // 指定されたBGMを再生
    public void PlayBGM(BGMType type)
    {
        bgmManager.Play(type);
    }

    // 再生中のBGMをフェードアウトで停止
    public void StopBGM()
    {
        bgmManager.StopWithFade();
    }

    // 指定されたSEを再生
    public void PlaySE(SEType type)
    {
        seManager.Play(type);
    }
}

