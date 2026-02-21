using DG.Tweening;
using UnityEngine;
/// <summary>
/// BGMType に応じて曲を再生するBGM管理クラス
/// </summary>
public class BGMManager : MonoBehaviour
{
    [SerializeField] AudioSource bgmSource; //BGM再生用のAudioSource

    [Header("BGM Clips")]
    [SerializeField] AudioClip startSceneBGM; //スタート時に流すBGM
    [SerializeField] AudioClip selectSceneBGM; //選択画面のBGM
    [SerializeField] AudioClip clearBGM; //クリア時に流すBGM
    [SerializeField] AudioClip gameOverBGM; //ゲームオーバー時に流すBGM
    [SerializeField] AudioClip stage1BGM; //ステージ1で流すBGM
    [SerializeField] AudioClip boss1BGM; //ボス戦で流すBGM

    [SerializeField] float fadeOutTime = 1.0f; //曲のフェードアウトにかかる時間

    Tween fadeTween;//フェードアウト処理のTween（途中キャンセル用）

    public enum BGMType //再生するBGMの種類
    {
        Start,
        Select,
        Clear,
        Stage1,
        GameOver,
        Boss1
    }

    // 指定されたBGMTypeに対応した曲を再生する
    public void Play(BGMType type)
    {
        AudioClip clip = GetClip(type);//BGMTypeから対応するAudioClipを取得
        if (clip == null) return;

        // すでに同じ曲が流れている場合は再生しない
        if (bgmSource.clip == clip) return;

        bgmSource.clip = clip;
        bgmSource.loop = true;
        bgmSource.Play();
    }

    // BGMTypeに応じて対応するAudioClipを返す
    private AudioClip GetClip(BGMType type)
    {
        switch (type)
        {
            case BGMType.Start: return startSceneBGM;
            case BGMType.Select: return selectSceneBGM;
            case BGMType.Clear: return clearBGM;
            case BGMType.Stage1: return stage1BGM;
            case BGMType.Boss1: return boss1BGM;
            default: return null;
        }
    }

    // 再生中のBGMをフェードアウトして停止する
    public void StopWithFade()
    {
        // 再生されていなければ何もしない
        if (!bgmSource.isPlaying) return;

        // 途中でフェードしてたら止める
        fadeTween?.Kill();

        fadeTween = bgmSource
            .DOFade(0f, fadeOutTime)
            .OnComplete(() =>
            {
                bgmSource.Stop();
                bgmSource.volume = 1f; // 次回再生用に戻す
            });
    }
}
