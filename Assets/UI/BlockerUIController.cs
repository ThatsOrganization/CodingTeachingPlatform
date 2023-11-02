using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BlockerUIController : MonoBehaviour
{
    Image _blockingImage;

    void Start()
    {
        _blockingImage = gameObject.GetComponent<Image>();
    }

    public void BlockUI()
    {
        _blockingImage.raycastTarget = true;
    }

    public void UnblockUI()
    {
        _blockingImage.raycastTarget = false;
    }
}
