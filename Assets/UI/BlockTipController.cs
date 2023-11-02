using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BlockTipController : MonoBehaviour
{
    [SerializeField]
    GameObject titleObject;

    [SerializeField]
    GameObject descriptionObject;

    public void Initialize(FunctionalBlock block)
    {
        titleObject.GetComponent<Text>().text = block.Name;
        descriptionObject.GetComponent<Text>().text = block.Description;

        float offset = 200;
        //for (int i = 0; i < transform.childCount; i++)
        //{
        //    var rectTransform = transform.GetChild(i).GetComponent<RectTransform>();
        //    if (rectTransform == null) continue;
        //    offset += rectTransform.rect.height;
        //}
        transform.position = Camera.main.WorldToScreenPoint(block.transform.position) + new Vector3(0, offset, 0);
    }

    public void AddComponent(Transform component)
    {
        component.SetParent(transform, false);
    }
}
