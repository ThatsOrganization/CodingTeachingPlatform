using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PropertiesPanelController : MonoBehaviour
{
    [SerializeField]
    GameObject directionPanelContentObject;

    [SerializeField]
    GameObject dataPanelContentObject;

    [SerializeField]
    GameObject removeBlockButtonObject;

    Transform _directionPanelContent;
    Transform _dataPanelContent;

    Button _removeBlockButton;

    public WorkspaceController Workspace { get; set; }

    public Transform DirectionPanelContent => _directionPanelContent;

    public Transform DataPanelContent => _dataPanelContent;

    public Button RemoveBlockButton => _removeBlockButton;

    public FunctionalBlock Block { get; protected set; }

    // Start is called before the first frame update
    void Start()
    {
        _directionPanelContent = directionPanelContentObject.transform;
        _dataPanelContent = dataPanelContentObject.transform;
        _removeBlockButton = removeBlockButtonObject.GetComponent<Button>();

        gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Initialize(FunctionalBlock block)
    {
        gameObject.SetActive(true);

        Block = block;

        if (Block.BlockType == TypeBlock.Start || Block.BlockType == TypeBlock.Finish || Block.BlockType == TypeBlock.Storage || !Block.Interactable)
            RemoveBlockButton.gameObject.SetActive(false);
        else
        {
            RemoveBlockButton.gameObject.SetActive(true);
            RemoveBlockButton.onClick.AddListener(Block.RemoveBlock);
        }
    }

    public void Uninitialize()
    {
        for (int i = 0; i < _directionPanelContent.childCount; i++)
        {
            var child = _directionPanelContent.GetChild(i).gameObject;
            if (child != null) Destroy(child);
        }

        for (int i = 0; i < _dataPanelContent.childCount; i++)
        {
            var child = _dataPanelContent.GetChild(i).gameObject;
            if (child != null) Destroy(child);
        }

        Block = FunctionalBlock.EmptyBlock;

        RemoveBlockButton.onClick.RemoveAllListeners();

        gameObject.SetActive(false);
    }
}
