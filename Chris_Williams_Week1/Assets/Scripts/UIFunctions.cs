using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIFunctions : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public bool cursorIsHovering { get; private set; }

    public void OnPointerEnter(PointerEventData eventData)
    {
        cursorIsHovering = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        cursorIsHovering = false;
    }

    public void _ShuffleColor() { Services.GameManager.RefreshButton(); }

    public void _ShowInstructions() { Services.GameManager.ShowInstructions(); }

    public void _ShowGame()
    {
        Debug.Assert(Services.Scenes.CurrentScene is InstructionSceneScript);
        
        Services.GameManager.PopScene();
    }

}
