using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleController : MonoBehaviour
{
    public const int LEFT_CLICK = 0;
    public const int RIGHT_CLICK = 1;

    public const float LEFT_LIMIT = -7.6f;
    public const float RIGHT_LIMIT = 7.6f;

    private const float TARGET_POS = 0.0f;
    private Vector3 TARGET_VEC;

    [SerializeField] private float deltaXPos;

    private const string SCROLL_WHEEL = "Mouse ScrollWheel";
    private float moveSpeed = 50.0f;
    private float dx;
    private float x;
    public float xPos {
        get { return x; }
        private set
        {       
            if (value < LEFT_LIMIT)     value = LEFT_LIMIT;
            if (value > RIGHT_LIMIT)    value = RIGHT_LIMIT;
            x = value;
        }
    }
    private const float yPos = -3;

    private SpriteRenderer sprite;
    private Color wrongColorVec;

    private Vector3 defaultScale;
    private GameObject ring;

    TaskManager tm = new TaskManager();

    public void Init(Vector3 scale)
    {
        ring = GameObject.Find("Circle_Empty");
        
        ring.transform.localScale = scale;
        deltaXPos = 0.2f;
        sprite = GetComponent<SpriteRenderer>();
        wrongColorVec = new Color(0.88f, 0.41f, 0.30f);
        defaultScale = new Vector3(0.65f, 0.65f, 0.65f);
        transform.localScale = scale;
        
        tm.Do
        (
                        new Scale(ring, scale, Vector3.one, 0.5f))
                .Then(  new Scale(gameObject, scale, defaultScale, 0.5f)
        );
        TARGET_VEC = new Vector3(TARGET_POS, -3, 0);
    }
	
    public bool CheckWin()
    {
        float lowerLimit = TARGET_POS - deltaXPos;
        float upperLimit = TARGET_POS + deltaXPos;

        if (lowerLimit < transform.position.x && transform.position.x < upperLimit)
        {
            tm.Do
            (
                        new Scale(gameObject, defaultScale, Vector3.one, 0.5f)
             );
            return true;
        }
        else
        {
            tm.Do
            (
                        new LERPColor(sprite, Color.white, wrongColorVec, 0.8f))
                .Then(  new LERPColor(sprite, wrongColorVec, Color.white, 0.5f)
             );
            return false;
        }
    }

    public void ConfirmState(bool hasWon)
    {

        if(hasWon)
        {
            tm.Do
            (
                   new LERP(gameObject, transform.position, TARGET_VEC, 0.2f)
             );
        }
        else
        {
            tm.Do
            (          
                   new Scale(gameObject, Vector3.one, defaultScale, 0.5f)
             );
             
        }
    }

	// Update is called once per frame
	void Update ()
    {
        tm.Update();

        if (!GameSceneScript.hasWon)
        {
            dx = Input.GetAxis(SCROLL_WHEEL) * moveSpeed;
            xPos += dx * Time.deltaTime;
            transform.position = new Vector3(xPos, yPos);
        }
    }
}
