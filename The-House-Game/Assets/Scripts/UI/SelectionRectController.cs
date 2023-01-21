using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Units.Settings;
using UnityEngine.UI;

public class SelectionRectController : MonoBehaviour
{
    [SerializeField] private GameObject rect;
    [SerializeField] private GameObject uiCanvas;
    public Vector3 firstCorner;

    void Start()
    {
        rect.SetActive(false);
    }

    public void InitializeRect(Vector3 corner)
    {
        rect.SetActive(true);
        rect.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 1);
        rect.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 1);
        firstCorner = corner;
        rect.transform.SetPositionAndRotation(firstCorner, rect.transform.rotation);
    }

    public void SetSecondCorner(Vector3 secondCorner)
    {
        rect.transform.SetPositionAndRotation((secondCorner + firstCorner) / 2, rect.transform.rotation);
        var hor = Mathf.Abs(uiCanvas.transform.InverseTransformVector(secondCorner).x - uiCanvas.transform.InverseTransformVector(firstCorner).x);
        var vert = Mathf.Abs(uiCanvas.transform.InverseTransformVector(secondCorner).y - uiCanvas.transform.InverseTransformVector(firstCorner).y);
        rect.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, hor);
        rect.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, vert);
    }

    public void HideRect()
    {
        rect.SetActive(false);
    }
}