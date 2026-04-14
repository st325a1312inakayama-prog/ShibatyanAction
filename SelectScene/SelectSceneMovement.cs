using System.Collections.Generic;
using Pixeye.Unity;
using UnityEngine;
using UnityEngine.InputSystem;
/// <summary>
/// セレクトシーンでの入力処理とUI操作を管理するクラス
/// SelectSceneUIManagerのアニメーション制御と、各ボタンの選択処理を担当する
/// </summary>
public class SelectSceneMovement : MonoBehaviour
{
    SelectSceneUIManager selectSceneUIManager; //UIの動きのアニメーション管理クラスの取得
    SelectSceneMethodManager selectSceneMethodManager; //シーン移行関数管理クラスの取得

    private State currentState = State.notSelected; //現在の選択状態を表す
    private bool stateChange = false; //初期選択状態を一度だけ設定するためのフラグ
    private bool isClicked = false; //決定後に入力を受け付けないようにするフラグ
    bool canDown = false; //Down入力の一回押し判定用フラグ
    bool canUp = false; //Up入力の一回押し判定用フラグ
    #region カーソル
    [Foldout("骨型のカーソルUI()")]
    [Header("選択カーソル"),SerializeField] RectTransform cursorUI;
    [Foldout("骨型のカーソルUI")]
    [Header("カーソルの移動ポジション"),SerializeField] List<Vector2> pos = new List<Vector2>();
    #endregion

    //メニューの選択状態
    public enum State
    {
        notSelected,
        gameStart,
        credit,
        tutorial,
        gameEnd
    }
    void Start()
    {
        //コンポーネントの取得
        selectSceneUIManager = GetComponent<SelectSceneUIManager>();
        selectSceneMethodManager = GetComponent<SelectSceneMethodManager>();

        //セレクト画面の初期UIアニメーションを再生
        selectSceneUIManager.SelectContainerAnimation();

        //BGMの再生
        AudioManager.Instance.PlayBGM(BGMManager.BGMType.Select);
    }

    void Update()
    {
        //UIのアニメーションの再生が終了したら選択可能になる
        if(selectSceneUIManager.CanSelect && !stateChange)
        {
            stateChange = true;
            currentState = State.gameStart;
        }
    }

    //選択状態によるカーソルの場所
    void UpdateCursorPosition()
    {
        switch (currentState)
        {
            case State.gameStart:
                cursorUI.anchoredPosition = pos[0];
                Debug.Log("gameStart");
                break;
            case State.credit:
                cursorUI.anchoredPosition = pos[1];
                Debug.Log("credit");
                break;
            case State.tutorial:
                cursorUI.anchoredPosition = pos[2];
                Debug.Log("tutorial");
                break;
            case State.gameEnd:
                cursorUI.anchoredPosition = pos[3];
                Debug.Log("gameEnd");
                break;
        }
    }

    //クリックした際のアニメーションとシーン移行
    public void OnClick(InputAction.CallbackContext context)
    {
        RectTransform rect = selectSceneUIManager.Footprints.rectTransform;
        if (context.started && !isClicked)
        {
            switch (currentState)
            {
                //notSelectedの時はなにも反応しない
                case State.notSelected:
                    break;

                //notSelected以外はUIのアニメーションを再生させ、シーン移行の関数を実行させる
                case State.gameStart:
                    rect.anchoredPosition = pos[0];
                    selectSceneMethodManager.StartButtonPush();
                    selectSceneUIManager.FootStampAnimation();
                    selectSceneUIManager.FadeOutAnimation();
                    isClicked = true;
                    break;
                case State.credit:
                    rect.anchoredPosition = pos[1];
                    selectSceneMethodManager.CreditButtonPush();
                    selectSceneUIManager.FootStampAnimation();
                    selectSceneUIManager.FadeOutAnimation();
                    isClicked = true;
                    break;
                case State.tutorial:
                    rect.anchoredPosition = pos[2];
                    selectSceneMethodManager.TutorialButtonPush();
                    selectSceneUIManager.FootStampAnimation();
                    selectSceneUIManager.FadeOutAnimation();
                    isClicked = true;
                    break;
                case State.gameEnd:
                    rect.anchoredPosition = pos[3];
                    selectSceneMethodManager.EndButtonPush();
                    selectSceneUIManager.FootStampAnimation();
                    selectSceneUIManager.FadeOutAnimation();
                    isClicked = true;
                    break;
            }
        }
    }

    //Backに対応したキーやボタンを押したら、ゲーム終了の位置にカーソルを合わせる
    public void Back(InputAction.CallbackContext context)
    {
        if(context.started &&!isClicked)
        {
            currentState = State.gameEnd;
            UpdateCursorPosition();
            //効果音を流す
            AudioManager.Instance.PlaySE(SEManager.SEType.Cancel);
        }
    }

    //Downに対応したキーやボタンを押すと、それぞれの位置にカーソルを合わせて選択状態を変化させる
    public void SelectingDown(InputAction.CallbackContext context)
    {
        if (context.started && !isClicked)
        {
            canDown = true;
            if (currentState == State.gameStart && canDown)
            {
                currentState = State.credit;
                canDown = false;
                AudioManager.Instance.PlaySE(SEManager.SEType.Cursor);
            }
            else if (currentState == State.credit && canDown)
            {
                currentState = State.tutorial;
                canDown = false;
                AudioManager.Instance.PlaySE(SEManager.SEType.Cursor);
            }
            else if (currentState == State.tutorial && canDown)
            {
                currentState = State.gameEnd;
                canDown = false;
                AudioManager.Instance.PlaySE(SEManager.SEType.Cursor);
            }
            UpdateCursorPosition();
        }
        if(context.canceled || isClicked)
        {
            canDown = false;
        }
    }
    //Upに対応したキーやボタンを押すと、それぞれの位置にカーソルを合わせて選択状態を変化させる
    public void SelectingUp(InputAction.CallbackContext context)
    {
        if (context.started && !isClicked)
        {
            canUp = true;

            if (currentState == State.credit)
            {
                currentState = State.gameStart;
                canUp = false;
                AudioManager.Instance.PlaySE(SEManager.SEType.Cursor);
            }
            else if (currentState == State.tutorial)
            {
                currentState = State.credit;
                canUp = false;
                AudioManager.Instance.PlaySE(SEManager.SEType.Cursor);
            }
            else if (currentState == State.gameEnd)
            {
                currentState = State.tutorial;
                canUp = false;
                AudioManager.Instance.PlaySE(SEManager.SEType.Cursor);
            }
            UpdateCursorPosition();
        }
        if(context.canceled || isClicked)
        { 
            canUp = false;
        }
    }
}
