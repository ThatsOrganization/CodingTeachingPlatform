using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DataProvider : MonoBehaviour
{
    [SerializeField]
    Canvas canvasUI;

    [SerializeField]
    GameObject directionPanelPrefab;

    [SerializeField]
    GameObject unaryBlockDataPanelPrefab;

    [SerializeField]
    GameObject binaryBlockDataPanelPrefab;

    [SerializeField]
    GameObject blockTipPrefab;

    [SerializeField]
    GameObject blockDataTipPrefab;

    [SerializeField]
    GameObject fieldPanelPrefab;

    [SerializeField]
    GameObject blockButtonPrefab;

    [SerializeField]
    GameObject actionIconPrefab;

    [SerializeField]
    GameObject compareActionIconButtonPrefab;

    [SerializeField]
    GameObject saveLevelPanelPrefab;

    [SerializeField]
    private CodePanelController codePanelPrefab;

    [SerializeField]
    private Text codeLinePrefab;

    [SerializeField]
    GameObject inputPanelPrefab;

    //blocks

    [SerializeField]
    GameObject startBlockPrefab;

    [SerializeField]
    GameObject finishBlockPrefab;

    [SerializeField]
    GameObject storageBlockPrefab;

    [SerializeField]
    GameObject additionBlockPrefab;

    [SerializeField]
    GameObject subtractionBlockPrefab;

    [SerializeField]
    GameObject multiplicationBlockPrefab;

    [SerializeField]
    GameObject conditionBlockPrefab;

    [SerializeField]
    GameObject transitionBlockPrefab;

    static Canvas _canvasUI;
    public static Canvas CanvasUI => _canvasUI;

    static GameObject _blockTipPrefab;
    public static GameObject BlockTipPrefab => _blockTipPrefab;

    private static GameObject _blockDataTipPrefab;
    public static GameObject BlockDataTipPrefab => _blockDataTipPrefab;

    static GameObject _fieldPanelPrefab;
    public static GameObject FieldPanelPrefab => _fieldPanelPrefab;

    static GameObject _blockButtonPrefab;
    public static GameObject BlockButtonPrefab => _blockButtonPrefab;

    static GameObject _actionIconPrefab;
    public static GameObject ActionIconPrefab => _actionIconPrefab;

    static GameObject _compareActionIconButtonPrefab;
    public static GameObject CompareActionIconButtonPrefab => _compareActionIconButtonPrefab;

    static GameObject _directionPanelPrefab;
    public static GameObject DirectionPanelPrefab => _directionPanelPrefab;

    static GameObject _unaryBlockDataPanelPrefab;
    public static GameObject UnaryBlockDataPanelPrefab => _unaryBlockDataPanelPrefab;

    static GameObject _binaryBlockDataPanelPrefab;
    public static GameObject BinaryBlockDataPanelPrefab => _binaryBlockDataPanelPrefab;

    static GameObject _saveLevelPanelPrefab;
    public static GameObject SaveLevelPanelPrefab => _saveLevelPanelPrefab;

    private static CodePanelController _codePanelPrefab;
    public static CodePanelController CodePanelPrefab => _codePanelPrefab;

    private static Text _codeLinePrefab;
    public static Text CodeLinePrefab => _codeLinePrefab;

    static GameObject _inputPanelPrefab;
    public static GameObject InputPanelPrefab => _inputPanelPrefab;

    static Dictionary<TypeBlock, GameObject> _blocks;

    public static Dictionary<TypeBlock, GameObject> Blocks => _blocks;

    void Awake()
    {
        _canvasUI = canvasUI;

        _directionPanelPrefab = directionPanelPrefab;

        _unaryBlockDataPanelPrefab = unaryBlockDataPanelPrefab;

        _binaryBlockDataPanelPrefab = binaryBlockDataPanelPrefab;

        _blockTipPrefab = blockTipPrefab;

        _blockDataTipPrefab = blockDataTipPrefab;

        _fieldPanelPrefab = fieldPanelPrefab;

        _blockButtonPrefab = blockButtonPrefab;

        _actionIconPrefab = actionIconPrefab;

        _compareActionIconButtonPrefab = compareActionIconButtonPrefab;

        _saveLevelPanelPrefab = saveLevelPanelPrefab;

        _codePanelPrefab = codePanelPrefab;

        _codeLinePrefab = codeLinePrefab;

        _inputPanelPrefab = inputPanelPrefab;

        _blocks = new Dictionary<TypeBlock, GameObject>()
        {
            { TypeBlock.Start, startBlockPrefab },
            { TypeBlock.Finish, finishBlockPrefab },
            { TypeBlock.Storage, storageBlockPrefab },
            { TypeBlock.Addition, additionBlockPrefab },
            { TypeBlock.Subtraction, subtractionBlockPrefab },
            { TypeBlock.Multiplication, multiplicationBlockPrefab },
            { TypeBlock.Condition, conditionBlockPrefab },
            { TypeBlock.Transition, transitionBlockPrefab }
        };
    }
}
