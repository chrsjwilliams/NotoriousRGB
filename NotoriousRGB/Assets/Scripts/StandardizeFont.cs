using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StandardizeFont : MonoBehaviour
{
    [SerializeField]private List<Text> instructionFonts;
	// Use this for initialization
	void Start ()
    {
        instructionFonts = new List<Text>();

        instructionFonts.Add(GameObject.Find("SCROLL TEXT").GetComponent<Text>());
        instructionFonts.Add(GameObject.Find("RIGHT CLICK TEXT").GetComponent<Text>());
        instructionFonts.Add(GameObject.Find("LEFT CLICK TEXT").GetComponent<Text>());

        foreach(Text text in instructionFonts)
        {
            text.fontSize = instructionFonts[0].fontSize;
        }

    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
