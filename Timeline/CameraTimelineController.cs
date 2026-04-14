using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.InputSystem;

/// <summary>
/// Timeline再生中はプレイヤー＆カメラ操作を完全に無効化する
/// </summary>
public class CameraTimelineController : MonoBehaviour
{
    [SerializeField] private PlayableDirector timelineDirector; //対象のタイムライン
    [SerializeField] private GameObject player; //プレイヤー
    [SerializeField] private CinemachineCamera activeCamera; //プレイヤーを追従するカメラ

    private PlayerController playerController; //プレイヤーの行動を制御するクラス
    private PlayerAnimation playerAnimation; //プレイヤーのアニメーション再生の関数を管理しているクラス
    private PlayerInput playerInput; //PlayerInput
    private CinemachineInputAxisController inputController; //キー、スティック入力しカメラを動かす

    void Start()
    {
        //コンポーネントの取得
        playerController = player.GetComponent<PlayerController>();
        playerAnimation = player.GetComponent<PlayerAnimation>();
        playerInput = player.GetComponent<PlayerInput>();
        inputController = activeCamera.GetComponent<CinemachineInputAxisController>();

        //Timeline開始時
        LockControl();

        //Timeline再生
        timelineDirector.Play();
        timelineDirector.stopped += OnTimelineFinished;
    }

    void LockControl()
    {
        //入力を完全停止
        if (playerInput != null)
            playerInput.enabled = false;

        //プレイヤーロジックも念のため停止
        if (playerController != null)
        {
            playerController.InputBlocked = true;
            playerController.StopAllCoroutines();
            playerController.ResetInputState();
            playerController.enabled = false;
        }

        //アニメーション制御を止める
        if (playerAnimation != null)
            playerAnimation.enabled = false;

        //カメラ入力停止
        if (inputController != null)
            inputController.enabled = false;
    }

    void UnlockControl()
    {
        //入力再開
        if (playerInput != null)
            playerInput.enabled = true;

        if (playerController != null)
        {
            playerController.enabled = true;
            playerController.InputBlocked = false;
        }

        if (playerAnimation != null)
            playerAnimation.enabled = true;

        if (inputController != null)
            inputController.enabled = true;
    }

    void OnTimelineFinished(PlayableDirector director)
    {
        UnlockControl();
    }
}
