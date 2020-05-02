using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextController : MonoBehaviour
{

    [SerializeField] private bool finishedIntro;

    public const int COLOR_BASE = 256;
    public const int NUM_COLOR_CHANNELS = 3;

    public const int LEFT_CLICK = 0;
    public const int RIGHT_CLICK = 1;

    public Text text { get; private set; }
    private const string SCROLL_WHEEL = "Mouse ScrollWheel";

    private Image indicator;
    [SerializeField] private Color[] indicatorColor;

    [SerializeField]private int growthRate = 60;
    private float growthInc;

    public Color targetColor { get; private set; }
    private float[] prevColorArray;
    private int colorIndex = 0;
    private int prevIndex = 0;

    public float[] backgroundColorArray;
    public Camera camera_ { get; private set; }

    [SerializeField] private float deltaColor;
    TaskManager tm = new TaskManager();

    public void Init(Color previousTargetColor, bool shuffled)
    {
        indicatorColor = new Color[] {  new Color(253f / COLOR_BASE, 191f/ COLOR_BASE, 194f/ COLOR_BASE),
                                        new Color(194f / COLOR_BASE, 253f / COLOR_BASE, 191f / COLOR_BASE),
                                        new Color(191f / COLOR_BASE, 194f / COLOR_BASE, 253f / COLOR_BASE)
                                        };
        indicator = GameObject.Find("Swatch").GetComponent<Image>();
        indicator.color = indicatorColor[colorIndex];

        finishedIntro = false;
        camera_ = Services.GameManager.MainCamera;
        deltaColor = 0.2f;

        if (!shuffled)
        {
            backgroundColorArray = GameManager.color_to_float_array(previousTargetColor);
        }
        else
        {
            backgroundColorArray = GameManager.color_to_float_array(Color.white);
        }
        prevColorArray = backgroundColorArray;
        text = GetComponent<Text>();
        text.color = previousTargetColor;
        tm.Do
        (
                    new WaitTask(1.5f))
            .Then(  new ActionTask(FinishedIntro)
        );

        

        
        targetColor = GameManager.float_array_to_Color(Services.GameManager.targetColorArray);
    }

    private void FinishedIntro()
    {
        finishedIntro = true;
    }

    public bool CheckWin()
    {
        for (int i = 0; i < Services.GameManager.targetColorArray.Length; i++)
        {  
            float lowerLimit = Services.GameManager.targetColorArray[i] - deltaColor;
            float upperLimit = Services.GameManager.targetColorArray[i] + deltaColor;

            if(!(lowerLimit < backgroundColorArray[i] && backgroundColorArray[i] < upperLimit))
            {
                return false;
            }
        }

        return true;
    }
	
    public void ConfirmState(bool hasWon)
    {
        if(hasWon)
        {
            indicator.color = Color.Lerp(indicatorColor[colorIndex], Color.white, 1);
        }
        else
        {

        }
    }

    private void LERPSwatch()
    {
        tm.Do
        (
            new LERPColor(indicator, indicatorColor[prevIndex], indicatorColor[colorIndex], 0.6f)
        );
    }

    // Update is called once per frame
    void Update ()
    {
        tm.Update();
        if(!finishedIntro)
        {
            for (int i = 0; i < backgroundColorArray.Length; i++)
            {
                backgroundColorArray[i] = Mathf.Lerp(prevColorArray[i], 1.3f, Time.deltaTime);
            }
        }

        growthInc = Input.GetAxis(SCROLL_WHEEL) * growthRate;
        
        if (Input.GetMouseButtonDown(RIGHT_CLICK))
        {
            
            prevIndex = colorIndex;
            
            colorIndex = (colorIndex + 1) % NUM_COLOR_CHANNELS;
            LERPSwatch();
            switch (colorIndex)
            {
                case 0:
                    Services.AudioManager.PlayClip(Clips.RED);
                    break;
                case 1:
                    Services.AudioManager.PlayClip(Clips.GREEN);
                    break;
                case 2:
                    Services.AudioManager.PlayClip(Clips.BLUE);
                    break;
                default:
                    break;
            }
        }

        backgroundColorArray[colorIndex] += growthInc / COLOR_BASE;

        if (backgroundColorArray[colorIndex] < 0) backgroundColorArray[colorIndex] = 0;
        if (backgroundColorArray[colorIndex] > 1) backgroundColorArray[colorIndex] = 1;

        camera_.backgroundColor = GameManager.float_array_to_Color(backgroundColorArray);
    }

    
}
