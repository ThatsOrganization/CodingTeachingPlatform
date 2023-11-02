using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BlockInitializer : MonoBehaviour
{

    [SerializeField]
    GameObject blockIconObject;

    [SerializeField]
    GameObject blocksCountInputObject;

    Button _blockButton;
    Image _blockIcon;
    InputField _blocksCountInput;

    public event InitializeBlockHandler OnInitializeBlock;

    int _blocksCount;

    GameObject _blockPrefab;

    public GameObject BlockPrefab => _blockPrefab;

    public int BlocksCount => _blocksCount;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    void Awake()
    {
        _blockButton = transform.GetComponent<Button>();
        _blockButton.onClick.AddListener(AddBlock);

        _blockIcon = blockIconObject.GetComponent<Image>();

        _blocksCountInput = blocksCountInputObject.GetComponent<InputField>();
        _blocksCountInput.onEndEdit.AddListener(OnBlocksCountChanged);
        _blocksCountInput.interactable = WorkspaceSceneManager.IsEditMode;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Initialize(GameObject blockPrefab, int blocksCount, 
        Transform content, InitializeBlockHandler initializeAction)
    {
        _blockPrefab = blockPrefab;
        var icon = _blockPrefab.GetComponent<FunctionalBlock>().Icon;
        _blockIcon.sprite = icon;

        SetBlocksCount(blocksCount);

        transform.SetParent(content);
        transform.localPosition = new Vector3();
        transform.localScale = new Vector3(1, 1, 1);

        OnInitializeBlock += initializeAction;
    }

    public void Block()
    {
        _blockButton.interactable = false;

        var image = GetComponent<Image>();
        image.color = GetBlockedColor(image.color);

        _blockIcon.color = GetBlockedColor(_blockIcon.color);

        _blocksCountInput.textComponent.color = GetBlockedColor(_blocksCountInput.textComponent.color);
    }

    public void Unblock()
    {
        _blockButton.interactable = true;

        var image = GetComponent<Image>();
        image.color = GetUnblockedColor(image.color);

        _blockIcon.color = GetUnblockedColor(_blockIcon.color);

        _blocksCountInput.textComponent.color = GetUnblockedColor(_blocksCountInput.textComponent.color);
    }

    void AddBlock()
    {
        if (!WorkspaceSceneManager.IsEditMode && _blocksCount <= 0) return;

        OnInitializeBlock?.Invoke(_blockPrefab, RemoveBlock);

        if (!WorkspaceSceneManager.IsEditMode) DecreaseBlocksCount();
    }

    void RemoveBlock(FunctionalBlock block)
    {
        if (!WorkspaceSceneManager.IsEditMode) IncreaseBlocksCount();
    }

    void IncreaseBlocksCount()
    {
        SetBlocksCount(_blocksCount + 1);
    }

    void DecreaseBlocksCount()
    {
        SetBlocksCount(_blocksCount - 1);
    }

    void SetBlocksCount(int count)
    {
        _blocksCount = count;
        _blocksCountInput.text = _blocksCount.ToString();
    }

    void OnBlocksCountChanged(string val)
    {
        int count = 0;
        int.TryParse(val, out count);
        SetBlocksCount(count);
    }

    private Color GetBlockedColor(Color color) => new Color(color.r, color.g, color.b, 100f / 255);

    private Color GetUnblockedColor(Color color) => new Color(color.r, color.g, color.b, 1);
}
