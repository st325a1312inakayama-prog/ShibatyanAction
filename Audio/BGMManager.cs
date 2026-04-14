using DG.Tweening;
using UnityEngine;
/// <summary>
/// BGMType に応じて曲を再生するBGM管理クラス
/// </summary>
public class BGMManager : MonoBehaviour
{
    [SerializeField] AudioSource audioSource; //BGM再生用のAudioSource

    [Header("BGM Clips")]
    [SerializeField] AudioClip[] bgmList;

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
        if (audioSource.clip == clip) return;

        audioSource.clip = clip;
        audioSource.loop = true;
        audioSource.Play();
    }

    // BGMTypeに応じて対応するAudioClipを返す
    private AudioClip GetClip(BGMType type)
    {
        switch (type)
        {
            case BGMType.Start: return bgmList[0];
            case BGMType.Select: return bgmList[1];
            case BGMType.Clear: return bgmList[2];
            case BGMType.Stage1: return bgmList[3];
            case BGMType.Boss1: return bgmList[4];
            default: return null;
        }
    }

    // 再生中のBGMをフェードアウトして停止する
    public void StopWithFade()
    {
        // 再生されていなければ何もしない
        if (!audioSource.isPlaying) return;

        // 途中でフェードしてたら止める
        fadeTween?.Kill();

        fadeTween = audioSource
            .DOFade(0f, fadeOutTime)
            .OnComplete(() =>
            {
                audioSource.Stop();
                audioSource.volume = 1f; // 次回再生用に戻す
            });
    }
}
