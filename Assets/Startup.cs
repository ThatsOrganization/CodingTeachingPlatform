using UnityEngine;

public class Startup : MonoBehaviour
{
    [SerializeField]
    private GameObject plane;

    [SerializeField]
    private GameObject blocksPanel;

    [SerializeField]
    private GameObject propertiesPanel;

    [SerializeField]
    private GameObject runnerFieldsPanel;

    [SerializeField]
    private GameObject blockerUI;

    private void Start()
    {
        var cubeSize = 0.5f;
        var gridCellAsset = Resources.Load("GridCell") as GameObject;
        int n = 7;
        int m = 7;
        var gridCellSize = 0.7f;
        var start = (-n / 2) * gridCellSize;
        var gridCells = new GridCell[n][];
        for (int i = 0; i < n; i++)
        {
            gridCells[i] = new GridCell[m];
            for (int j = 0; j < m; j++)
            {
                var gridCell = Instantiate(gridCellAsset, new Vector3(), new Quaternion());
                gridCell.transform.parent = plane.transform;
                gridCell.transform.localScale =
                    new Vector3(gridCellSize - cubeSize, 0.5f, gridCellSize - cubeSize);
                gridCell.transform.localPosition = new Vector3(start + i * gridCellSize, -0.25f, start + j * gridCellSize);
                gridCells[i][j] = gridCell.GetComponent<GridCell>();
                gridCells[i][j].Initialize(i, j);
            }
        }

        var gridCellAdd = Instantiate(gridCellAsset, new Vector3(), new Quaternion());
        gridCellAdd.transform.parent = plane.transform;
        gridCellAdd.transform.localScale =
            new Vector3(gridCellSize - cubeSize, 0.5f, gridCellSize - cubeSize);
        gridCellAdd.transform.localPosition = new Vector3(start, -0.25f, start - gridCellSize);
        gridCellAdd.transform.tag = "GridCellAdditional";

        var gridPlaneObj = plane.GetComponent<GridPlaneController>();
        gridPlaneObj.GridCells = gridCells;
        gridPlaneObj.GridCellAdditional = gridCellAdd.GetComponent<GridCell>();

        var blockPanelObj = blocksPanel.GetComponent<BlockPanelController>();
        //var blocksPrefabs = Resources.LoadAll("Blocks")
        //    .Where(b => b is GameObject blockPrefab && blockPrefab.GetComponent<FunctionalBlock>() != null)
        //    .Cast<GameObject>();
        //foreach (var blockPrefab in blocksPrefabs)
        //    blockPanelObj.AddBlockButton(blockPrefab, 3);
        foreach (var block in WorkspaceSceneManager.CurrentLevelInfo.AvailableBlocks)
        {
            var blockInitializer = blockPanelObj.AddBlockButton(DataProvider.Blocks[block.Key], block.Value);
            if (!WorkspaceSceneManager.IsEditMode && block.Value == 0)
            {
                blockInitializer.Block();
            }
        }

        var propertiesPanelObj = propertiesPanel.GetComponent<PropertiesPanelController>();

        var runnerFieldsPanelObj = runnerFieldsPanel.GetComponent<RunnerFieldsPanelController>();
        var runnerPrefab = Resources.Load("Runner/RunnerUfo") as GameObject;
        var runner = Instantiate(runnerPrefab, new Vector3(), new Quaternion());
        var fieldsValues = WorkspaceSceneManager.CurrentLevelInfo.FieldsValues;
        runnerFieldsPanelObj.Initialize(runner.GetComponent<Runner>(), fieldsValues);

        var workspace = new WorkspaceController
            (WorkspaceSceneManager.CurrentLevelInfo,
            blockerUI.GetComponent<BlockerUIController>(),
            gridPlaneObj, 
            blockPanelObj, 
            propertiesPanelObj, 
            runnerFieldsPanelObj);
        
        foreach (var blockInfo in WorkspaceSceneManager.CurrentLevelInfo.Blocks)
        {
            if (blockInfo == null) continue;

            var blockPrefab = DataProvider.Blocks[blockInfo.BlockType];
            var block = Instantiate(blockPrefab, new Vector3(), new Quaternion());
            var funcBlock = block.GetComponent<FunctionalBlock>();
            GridCell cell = gridPlaneObj.GetFreeCell();
            if (blockInfo.X >= 0 && blockInfo.Y >= 0)
                cell = gridPlaneObj.GridCells[blockInfo.X][blockInfo.Y];
            funcBlock.Initialize(gridPlaneObj.transform, cell, workspace, stationary : true);
            funcBlock.Restore(blockInfo);
        }

        gridPlaneObj.UpdateCycles();

        var startBlock = gridPlaneObj.GetBlockByType(TypeBlock.Start) as StartBlock;
        runner.GetComponent<Runner>().Initialize(plane.transform, startBlock, workspace);
    }
}