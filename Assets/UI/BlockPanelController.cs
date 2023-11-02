using Assets.CodeTranslation;
using Assets.CodeTranslation.Nodes;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public delegate void InitializeBlockHandler(GameObject blockPrefab, 
    RemoveBlockHandler removeAction);

public class BlockPanelController : MonoBehaviour
{
    [SerializeField]
    GameObject blocksIconsContentObject;

    [SerializeField]
    GameObject launchRunnerButtonObject;

    [SerializeField]
    GameObject showHideDataButtonObject;

    [SerializeField]
    private CodeTranslator _codeTranslator;

    Transform blocksIconsContent;

    private bool _showBlocksData = false;

    public BlockInitializer[] BlockInitializers => blocksIconsContent.GetComponentsInChildren<BlockInitializer>();

    public WorkspaceController Workspace { get; set; }

    public bool ShowBlocksData => _showBlocksData;

    void Start()
    {
        blocksIconsContent = blocksIconsContentObject.GetComponent<ScrollRect>().content;

        //var addBlockButton = gameObject.transform.GetChild(0).GetComponent<Button>();
        //addBlockButton.onClick.AddListener(AddBlock);

        var runnerButton = launchRunnerButtonObject.GetComponent<Button>();
        runnerButton.onClick.AddListener(LaunchRunner);

        var showHideDataButton = showHideDataButtonObject.GetComponent<Button>();
        showHideDataButton.onClick.AddListener(ToggleShowBlocksData);

        var saveButton = transform.Find("ButtonSave").GetComponent<Button>();
        saveButton.onClick.AddListener(OnSave);
        saveButton.gameObject.SetActive(WorkspaceSceneManager.IsEditMode);

        var codeButton = transform.Find("ButtonCode").GetComponent<Button>();
        codeButton.onClick.AddListener(OnShowCode);
        codeButton.gameObject.SetActive(!WorkspaceSceneManager.IsEditMode);

        var exitButton = transform.Find("ButtonExit").GetComponent<Button>();
        exitButton.onClick.AddListener(OnExit);
    }

    void OnSave()
    {
        var saveLevelPanel = Instantiate(DataProvider.SaveLevelPanelPrefab).GetComponent<SaveLevelPanelController>();
        saveLevelPanel.transform.SetParent(DataProvider.CanvasUI.transform, false);
        saveLevelPanel.Initialize(Workspace);
    }

    private void OnShowCode()
    {
        if (_codeTranslator.TryBuildSyntaxTree(
            Workspace.GridPlane,
            Workspace.GridPlane.Cycles,
            Workspace.RunnerFieldsPanel.Runner,
            out var program))
        {
            var codePanel = Instantiate(DataProvider.CodePanelPrefab);
            codePanel.transform.SetParent(DataProvider.CanvasUI.transform, false);
            codePanel.Initialize(_codeTranslator, program);
        }
        else
        {
            Debug.Log("CANT TRANSLATE");
        }
    }

    void OnExit()
    {
        WorkspaceSceneManager.LoadMainMenu();
    }

    public BlockInitializer AddBlockButton(GameObject blockPrefab, int blocksCount)
    {
        var blockButton = Instantiate(DataProvider.BlockButtonPrefab, new Vector3(), new Quaternion());
        var blockInitializer = blockButton.GetComponent<BlockInitializer>();
        blockInitializer.Initialize(blockPrefab, blocksCount, 
            blocksIconsContent, AddBlock);
        return blockInitializer;
    }

    void AddBlock(GameObject blockPrefab, RemoveBlockHandler removeAction)
    {
        var gridPlane = Workspace.GridPlane;
        //if (gridPlane.GridCellAdditional.Block != FunctionalBlock.EmptyBlock) return; //если какой-то блок был добавлен
        ////до этого, но не перетянут на сетку
        if (blockPrefab.GetComponent<FunctionalBlock>().BlockType == TypeBlock.CycleStart)
        {

        }
        else
        {
            var block = Instantiate(blockPrefab, new Vector3(), new Quaternion());
            var funcBlock = block.GetComponent<FunctionalBlock>();
            if (gridPlane.FreeCellsCount >= 1)
            {
                var freeCell = gridPlane.GetFreeCell();
                funcBlock.Initialize(gridPlane.transform, freeCell,
                    Workspace);
                funcBlock.OnRemoveBlock += removeAction;
            }
        }
    }

    void LaunchRunner()
    {
        Workspace.RunnerFieldsPanel.Runner.Launch();
    }

    private void ToggleShowBlocksData()
    {
        var blocks = Workspace.GridPlane.GridCells
            .SelectMany(row => row.Select(cell => cell.Block).Where(block => block != FunctionalBlock.EmptyBlock))
            .ToList();
        var image = showHideDataButtonObject.GetComponent<Image>();
        if (_showBlocksData)
        {
            foreach (var block in blocks)
            {
                block.HideDataTip();
            }
            _showBlocksData = false;
            image.color = new Color(image.color.r, image.color.g, image.color.b, 100f / 255);
        }
        else
        {
            foreach (var block in blocks)
            {
                block.ShowDataTip();
            }
            _showBlocksData = true;
            image.color = new Color(image.color.r, image.color.g, image.color.b, 1);
        }
    }

}
