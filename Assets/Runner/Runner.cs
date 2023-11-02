using Assets;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Runner : MonoBehaviour
{

    int _x, _y;

    List<RunnerField> _fields = new List<RunnerField>();

    StartBlock _startBlock;

    public WorkspaceController Workspace { get; protected set; }

    public int X
    {
        get => _x;
        protected set
        {
            if (value == _x)
            {
                return;
            }
            if (!Workspace.GridPlane.IsInXRange(value))
            {
                OnFail();
                return;
            }
            _x = value;
            OnCoordinatesChanged();
        }
    }
    public int Y
    {
        get => _y;
        protected set
        {
            if (value == _y)
            {
                return;
            }
            if (!Workspace.GridPlane.IsInYRange(value))
            {
                OnFail();
                return;
            }
            _y = value;
            OnCoordinatesChanged();
        }
    }

    public List<RunnerField> Fields => _fields;

    void OnCoordinatesChanged()
    {
        var gridCells = Workspace.GridPlane.GridCells;
        float x = gridCells[X][Y].transform.position.x;
        float y = transform.position.y;
        float z = gridCells[X][Y].transform.position.z;
        var pos = new Vector3(x, y, z);
        StartCoroutine(MoveSmoothly(pos, 0.05f));
    }

    void OnNextBlockReached()
    {
        var gridCells = Workspace.GridPlane.GridCells;
        if (gridCells[X][Y].Block == FunctionalBlock.EmptyBlock)
        {
            OnFail();
            return;
        }
        gridCells[X][Y].Block.BlockAction(this);
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnFail()
    {
        Restart();
    }

    public void Restart()
    {
        if (!WorkspaceSceneManager.IsEditMode)
            Workspace.BlockerUI.UnblockUI();

        SetPosition(_startBlock.transform.position);
        _x = _startBlock.X;
        _y = _startBlock.Y;
    }

    public void Finish()
    {
        if (!WorkspaceSceneManager.IsEditMode)
            Workspace.BlockerUI.UnblockUI();

        Debug.Log("win!");
    }

    public void Initialize(Transform content, StartBlock startBlock, WorkspaceController workspace)
    {
        transform.SetParent(content);

        Workspace = workspace;

        _startBlock = startBlock;
        if (_startBlock != null)
        {
            _startBlock.OnPositionChanged += OnStartBlockPositionChanged;
            OnStartBlockPositionChanged(startBlock);
        }

    }

    void OnStartBlockPositionChanged(FunctionalBlock startBlock)
    {
        if (startBlock == null || startBlock.BlockType != TypeBlock.Start) return;

        SetPosition(_startBlock.transform.position);
        _x = _startBlock.X;
        _y = _startBlock.Y;
    }

    public void Launch()
    {
        if (!WorkspaceSceneManager.IsEditMode)
        {
            Workspace.PropertiesPanel.Uninitialize();
            Workspace.BlockerUI.BlockUI();
        }
        OnNextBlockReached();
    }

    public void Move(TypeDirection dir)
    {
        X += dir.GetXOffset();
        Y += dir.GetYOffset();
    }

    IEnumerator MoveSmoothly(Vector3 endPos, float dt)
    {
        var startPos = transform.position;
        float t = 0;
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
        yield return new WaitForSeconds(0.5f);
        
        OnNextBlockReached();

        StopCoroutine(nameof(MoveSmoothly));
    }

    void SetPosition(Vector3 pos)
    {
        transform.position = pos;
    }

}
