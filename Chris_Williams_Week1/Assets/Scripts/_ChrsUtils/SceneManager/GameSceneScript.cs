using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameSceneScript : Scene<TransitionData>
{
    public static bool hasWon { get; private set; }

    public const int LEFT_CLICK = 0;
    public const int RIGHT_CLICK = 1;

    private const string TEXT = "Text";
    private const string CIRCLE = "Circle_Full";
    private const string INSTRUCTIONS = "Instructions";

    public TextController textController;
    private CircleController circleController;

    private UIFunctions instructionButton;

    TaskManager tm = new TaskManager();

    private void Start()
    {
        hasWon = false;

        textController = GameObject.Find(TEXT).GetComponent<TextController>();
        circleController = GameObject.Find(CIRCLE).GetComponent<CircleController>();
        instructionButton = GameObject.Find(INSTRUCTIONS).GetComponent<UIFunctions>();
        if (Services.GameManager.previousGoalColor == Color.white || Services.GameManager.shuffled) circleController.Init(Vector3.zero);
        else circleController.Init(Vector3.one);

        textController.Init(Services.GameManager.previousGoalColor, Services.GameManager.shuffled);
        tm.Do
        (
            new LERPColor(textController.text, Services.GameManager.previousGoalColor, textController.targetColor, 0.5f)
        );
    }

    internal override void OnEnter(TransitionData data)
    {
        

        
    }

    internal override void OnExit()
    {
        hasWon = false;
    }

    private void ConfirmWin()
    {
        hasWon = circleController.CheckWin() && textController.CheckWin();
        tm.Do
        (
                        new WaitTask(0.5f))
                .Then(  new ActionTask(WinFeedback)
        );
    }

    private void WinFeedback()
    {
        if (hasWon)
        {
            Services.GameManager.ResetShuffle();
            Services.AudioManager.PlayClip(Clips.WIN);
        }
        else Services.AudioManager.PlayClip(Clips.LOSE);

        circleController.ConfirmState(hasWon);
        textController.ConfirmState(hasWon);
    }

	// Update is called once per frame
	void Update ()
    {
        tm.Update();
        if (Input.GetMouseButtonDown(LEFT_CLICK) && (!instructionButton.cursorIsHovering || hasWon))
        {
            if (hasWon) Services.GameManager.RefreshGame(textController.targetColor);
   
            tm.Do
            (
                       new ActionTask(ConfirmWin)
               
            );
        }
	}
}
