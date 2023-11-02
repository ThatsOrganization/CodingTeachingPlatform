using EPOOutline;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public delegate void RemoveBlockHandler(FunctionalBlock block);

public delegate void PositionChangedHandler(FunctionalBlock block);

[RequireComponent(typeof(Outlinable))]
public abstract class FunctionalBlock : MonoBehaviour
{
    [SerializeField]
    string _name;

    [SerializeField]
    [TextArea]
    string _description;

    [SerializeField]
    Sprite icon;

    [SerializeField]
    protected MeshRenderer _model;

    [SerializeField]
    protected SpriteRenderer _actionSprite;

    [SerializeField]
    protected SpriteRenderer _northArrowSprite;

    [SerializeField]
    protected SpriteRenderer _eastArrowSprite;

    [SerializeField]
    protected SpriteRenderer _southArrowSprite;

    [SerializeField]
    protected SpriteRenderer _westArrowSprite;

    protected Dictionary<TypeDirection, SpriteRenderer> _arrowSprites;

    protected Outlinable _outlinable;

    protected bool isDragging = false;
    bool isMoving = false;
    bool isAddedToGrid = false;
    bool isTipShown = false;
    bool isStationary = false;
    Vector3 lastValidGridCellPos;

    protected GameObject blockTip;

    private GameObject _blockDataTip;

    public event RemoveBlockHandler OnRemoveBlock;

    public event PositionChangedHandler OnPositionChanged;

    public string Name => _name;

    public string Description => _description;

    public RunnerField OperativeField { get; protected set; }

    public abstract TypeBlock BlockType { get; }

    public Sprite Icon => icon;

    public TypeDirection Direction { get; protected set; } = TypeDirection.North;

    public bool Interactable => !isStationary || WorkspaceSceneManager.IsEditMode;

    public static FunctionalBlock EmptyBlock = null;

    public WorkspaceController Workspace { get; protected set; }

    public int X { get; protected set; }

    public int Y { get; protected set; }

    public abstract void BlockAction(Runner runner);

    public virtual void Restore(BlockInfo info)
    {
        SetDirection(info.Direction);
        OnOperativeFieldChanged(Workspace.RunnerFieldsPanel.Runner.Fields, info.OperativeFieldIndex);
    }

    protected void OnOperativeFieldChanged(List<RunnerField> fields, int index)
    {
        OperativeField = fields[index];
        ShowDataTipIfNeeded();
    }

    public virtual void InitializeActionContent(Transform content)
    {
        var actionIcon = Instantiate(DataProvider.ActionIconPrefab, new Vector3(), new Quaternion());
        actionIcon.GetComponent<Image>().sprite = Icon;
        actionIcon.transform.SetParent(content, false);
    }

    protected void UninitializeProperties()
    {
        var propertiesPanel = Workspace.PropertiesPanel;
        propertiesPanel.Uninitialize();
    }

    protected virtual void InitializeProperties()
    {
        var propertiesPanel = Workspace.PropertiesPanel;
        UninitializeProperties();
        propertiesPanel.Initialize(this);
        var directionPanel = Instantiate
            (DataProvider.DirectionPanelPrefab, new Vector3(), new Quaternion());
        var directionPanelContent = Workspace.PropertiesPanel.DirectionPanelContent;
        directionPanel.GetComponent<DirectionPanelController>()
            .Initialize(directionPanelContent, this);
    }

    public virtual void Initialize(Transform content, GridCell cell,
        WorkspaceController workspace, bool stationary = false)
    {
        _arrowSprites = new Dictionary<TypeDirection, SpriteRenderer>
        {
            { TypeDirection.North, _northArrowSprite },
            { TypeDirection.East, _eastArrowSprite },
            { TypeDirection.South, _southArrowSprite },
            { TypeDirection.West, _westArrowSprite }
        };

        Workspace = workspace;

        transform.SetParent(content);
        transform.localPosition = cell.transform.localPosition;
        OnTriggerEnter(cell.GetComponent<Collider>());

        OperativeField = Workspace.RunnerFieldsPanel.Runner.Fields[0];

        isStationary = stationary;

        _outlinable = GetComponent<Outlinable>();
        _outlinable.enabled = false;

        foreach (var arrowSprite in _arrowSprites.Values.Where(x => x))
        {
            arrowSprite.enabled = false;
        }
        SetDirection(Direction);

        ShowDataTipIfNeeded();
    }

    public void RemoveBlock()
    {
        OnRemoveBlock?.Invoke(this);

        UninitializeProperties();
        HideDataTip();

        Workspace.GridPlane.GridCells[X][Y].Block = EmptyBlock;
        Workspace.GridPlane.UpdateCycles();

        Destroy(gameObject);
    }

    public void SetDirection(TypeDirection dir)
    {
        if (_arrowSprites.TryGetValue(Direction, out var oldArrowSprite) && oldArrowSprite)
        {
            oldArrowSprite.enabled = false;
            oldArrowSprite.color = DirectionPanelController.white;
        }

        if (_arrowSprites.TryGetValue(dir, out var newArrowSprite) && newArrowSprite)
        {
            newArrowSprite.color = DirectionPanelController.green;
            newArrowSprite.enabled = true;
        }

        Direction = dir;

        Workspace.GridPlane.UpdateCycles();
    }

    public void EnableSelectionOutline()
    {
        _outlinable.OutlineParameters.Color = Color.white;
        _outlinable.enabled = true;
    }

    public void EnableTrueCycleOutline()
    {
        _outlinable.OutlineParameters.Color = Color.green;
        _outlinable.enabled = true;
    }

    public void EnableFalseCycleOutline()
    {
        _outlinable.OutlineParameters.Color = Color.red;
        _outlinable.enabled = true;
    }

    public void DisableOutline()
    {
        _outlinable.enabled = false;
    }

    protected virtual void Update()
    {
        if (isDragging)
        {
            RaycastHit[] hits;
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if ((hits = Physics.RaycastAll(ray)).Any())
            {
                if (hits.Any(h => h.transform.CompareTag("GridPlane")))
                {
                    var hit = hits.First(h => h.transform.CompareTag("GridPlane"));
                    SetPosition(new Vector3(hit.point.x, transform.position.y, hit.point.z));
                }
            }
        }

        MouseHoverHandler();
    }

    void MouseHoverHandler()
    {
        RaycastHit[] hits;
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if ((hits = Physics.RaycastAll(ray)).Any(h => h.transform == transform))
            ShowTip();
        else
            HideTip();
    }

    protected virtual BlockTipController ShowTip()
    {
        if (isTipShown || isDragging) return null;

        isTipShown = true;

        blockTip = Instantiate(DataProvider.BlockTipPrefab);
        blockTip.transform.SetParent(DataProvider.CanvasUI.transform, false);
        var blockTipController = blockTip.GetComponent<BlockTipController>();
        blockTipController.Initialize(this);
        return blockTipController;
    }

    void HideTip()
    {
        if (!isTipShown) return;

        Destroy(blockTip);
        isTipShown = false;
    }

    public void ShowDataTip()
    {
        var dataTip = GetDataTip();

        if (string.IsNullOrWhiteSpace(dataTip))
        {
            HideDataTip();
            return;
        }

        if (!_blockDataTip)
        {
            _blockDataTip = Instantiate(DataProvider.BlockDataTipPrefab);
            _blockDataTip.transform.SetParent(DataProvider.CanvasUI.transform, false);
        }

        const float offset = 80;
        var newPosition = Camera.main.WorldToScreenPoint(transform.position) + new Vector3(0, offset, 0);
        _blockDataTip.transform.position = newPosition;
        _blockDataTip.GetComponentInChildren<Text>().text = dataTip;
    }

    public void HideDataTip()
    {
        if (!_blockDataTip)
        {
            return;
        }

        Destroy(_blockDataTip);
        _blockDataTip = null;
    }

    protected virtual string GetDataTip() => null;

    protected void ShowDataTipIfNeeded()
    {
        if (Workspace.BlockPanel.ShowBlocksData)
        {
            ShowDataTip();
        }
    }

    IEnumerator MoveSmoothly(Vector3 endPos, float dt)
    {
        var startPos = transform.position;
        float t = 0;
        isMoving = true;
        while (t < 1)
        {
            var x = Mathf.Lerp(startPos.x, endPos.x, t);
            var y = startPos.y;
            var z = Mathf.Lerp(startPos.z, endPos.z, t);
            SetPosition(new Vector3(x, y, z));
            t += dt;
            yield return new WaitForSeconds(0.01f);
        }
        SetPosition(endPos);
        isMoving = false;
        StopCoroutine(nameof(MoveSmoothly));
    }

    public void StartDragging()
    {
        if (Interactable)
        {
            if (isMoving)
            {
                StopCoroutine(nameof(MoveSmoothly));
                isMoving = false;
            }
            isDragging = true;
        }
        HideTip();
        InitializeProperties();
    }

    public void StopDragging()
    {
        if (isDragging)
        {
            StartCoroutine(MoveSmoothly(lastValidGridCellPos, 0.2f));
        }
        isDragging = false;
    }

    void SetPosition(Vector3 pos)
    {
        transform.position = pos;
        ShowDataTipIfNeeded();
        OnPositionChanged?.Invoke(this);
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.transform.CompareTag("GridCell"))
        {
            var gridCell = col.gameObject.GetComponent<GridCell>();
            if (gridCell.Block == FunctionalBlock.EmptyBlock)
            {
                lastValidGridCellPos = col.transform.position;
                gridCell.Block = this;
                isAddedToGrid = true;

                X = gridCell.X;
                Y = gridCell.Y;

                Workspace.GridPlane.UpdateCycles();
            }
        }

        if (!isAddedToGrid && col.transform.CompareTag("GridCellAdditional"))
        {
            var gridCellAdd = col.gameObject.GetComponent<GridCell>();
            lastValidGridCellPos = col.transform.position;
            gridCellAdd.Block = this;
        }
    }

    void OnTriggerExit(Collider col)
    {
        if (col.transform.CompareTag("GridCell") || col.transform.CompareTag("GridCellAdditional"))
        {
            var gridCell = col.gameObject.GetComponent<GridCell>();
            if (gridCell.Block == this)
            {
                gridCell.Block = FunctionalBlock.EmptyBlock;
            }
        }
    }
}
