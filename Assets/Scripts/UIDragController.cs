using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIDragController : MonoBehaviour
{
    [SerializeField]
    private Canvas canvas;

    public void dragHandler(BaseEventData data){
        PointerEventData pointerData = (PointerEventData)data;

        Vector3 position;
        RectTransformUtility.ScreenPointToWorldPointInRectangle(
            (RectTransform)canvas.transform,
            pointerData.position,
            canvas.worldCamera,
            out position
        );

        transform.position = canvas.transform.TransformPoint(position.x,position.y,0);
    }
}
