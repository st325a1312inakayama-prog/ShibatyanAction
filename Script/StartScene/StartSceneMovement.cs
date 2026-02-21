using UnityEngine;
using UnityEngine.Playables;
/// <summary>
/// スタートシーンでの入力処理とUI操作を管理するクラス
/// StartSceneUIManagerのアニメーション制御と、ゲームスタート時の処理を担当する
/// </summary>
public class StartSceneMovement : MonoBehaviour
{
    PlayableDirector playableDirector; //Timelineの再生用コントローラーの取得
    StartSceneUIManager startSceneUIManager; //UIの動きのアニメーション管理クラスの取得
    [SerializeField] GameObject manager; //ゲーム内のマネージャーオブジェクト

    bool finishedAnimation = false; //Timeline終了を知らせるもの

    void Start()
    {
        //コンポーネントの取得
        playableDirector = GetComponent<PlayableDirector>();
        startSceneUIManager = manager.GetComponent<StartSceneUIManager>();

        //再生時に呼び出す関数
        playableDirector.stopped += OnTimelineStopped;
    }
    
    public void OnTimelineStopped(PlayableDirector director)
    {
        //タイムラインが終了したらUIの演出とBGMを再生させる
        if (finishedAnimation) return;
        finishedAnimation = true;
        startSceneUIManager.StartFlashing();
        AudioManager.Instance.PlayBGM(BGMManager.BGMType.Start);
    }

    public void OnClick()
    {
        //クリックが検出されたらUIの演出が入る
        if (!finishedAnimation) return;

        finishedAnimation = false;
        AudioManager.Instance.StopBGM();
        startSceneUIManager.FootStampAnimation();
        startSceneUIManager.StopFlashing();
        startSceneUIManager.BackGroundAnimation();
        startSceneUIManager.ImageMovementAnimation();
    }
}
