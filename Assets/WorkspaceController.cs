public class WorkspaceController
{
    private GridPlaneController _gridPlane;
    private BlockPanelController _blockPanel;
    private PropertiesPanelController _propertiesPanel;
    private RunnerFieldsPanelController _runnerFieldsPanel;

    public LevelInfo CurrentLevel { get; }

    public BlockerUIController BlockerUI { get; }

    public GridPlaneController GridPlane
    {
        get => _gridPlane;
        protected set
        {
            _gridPlane = value;
            if (_gridPlane != null) _gridPlane.Workspace = this;
        }
    }

    public BlockPanelController BlockPanel
    {
        get => _blockPanel;
        protected set
        {
            _blockPanel = value;
            if (_blockPanel != null) _blockPanel.Workspace = this;
        }
    }

    public PropertiesPanelController PropertiesPanel
    {
        get => _propertiesPanel;
        protected set
        {
            _propertiesPanel = value;
            if (_propertiesPanel != null) _propertiesPanel.Workspace = this;
        }
    }

    public RunnerFieldsPanelController RunnerFieldsPanel
    {
        get => _runnerFieldsPanel;
        protected set
        {
            _runnerFieldsPanel = value;
            if (_runnerFieldsPanel != null) _runnerFieldsPanel.Workspace = this;
        }
    }

    public WorkspaceController
        (LevelInfo levelInfo,
        BlockerUIController blockerUI,
        GridPlaneController gridPlane, 
        BlockPanelController blockPanel, 
        PropertiesPanelController propertiesPanel,
        RunnerFieldsPanelController runnerFieldsPanel)
    {
        CurrentLevel = levelInfo;

        BlockerUI = blockerUI;

        GridPlane = gridPlane;
        BlockPanel = blockPanel;
        PropertiesPanel = propertiesPanel;
        RunnerFieldsPanel = runnerFieldsPanel;
    }
}
