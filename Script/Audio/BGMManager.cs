using DG.Tweening;
using UnityEngine;
/// <summary>
/// BGMType に応じて曲を再生するBGM管理クラス
/// </summary>
public class BGMManager : MonoBehaviour
{
    [SerializeField] AudioSource audioSource; 

    [Header("BGM Clips")] //=> bgm系はリストとかにして、Inspector上で増やせるようにした方がいいと思います 
    [SerializeField] AudioClip startSceneBGM; 
    [SerializeField] AudioClip selectSceneBGM; 
    [SerializeField] AudioClip clearBGM; 
    [SerializeField] AudioClip gameOverBGM; 

    //拡張性がありません。今は1個でも、将来増える可能性があるなら配列とかにしてください
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
