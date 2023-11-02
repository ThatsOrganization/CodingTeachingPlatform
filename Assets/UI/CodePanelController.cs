using Assets.CodeTranslation;
using Assets.CodeTranslation.Nodes;
using UnityEngine;
using UnityEngine.UI;

public class CodePanelController : MonoBehaviour
{
    [SerializeField]
    private ScrollRect _codeView;

    #region language selection

    [SerializeField]
    private GameObject _codeLanguagesView;

    [SerializeField]
    private Button _selectButton;

    [SerializeField]
    private Button _csharpButton;

    [SerializeField]
    private Button _pythonButton;

    #endregion language selection

    [SerializeField]
    private Button _printButton;

    [SerializeField]
    private Button _saveButton;

    [SerializeField]
    private Button _copyButton;

    [SerializeField]
    private Button _closeButton;

    private CodeTranslator _codeTranslator;

    private Program _program;

    private CodeLanguage _language = CodeLanguage.CSharp;

    private string _code;

    public void Initialize(CodeTranslator codeTranslator, Program program)
    {
        _codeTranslator = codeTranslator;
        _program = program;
    }

    private void Start()
    {
        _selectButton.onClick.AddListener(OnSelect);

        _csharpButton.onClick.AddListener(() => SelectLanguage(CodeLanguage.CSharp, "C#"));
        _pythonButton.onClick.AddListener(() => SelectLanguage(CodeLanguage.Python, "Python"));

        _printButton.onClick.AddListener(PrintCode);
        _saveButton.onClick.AddListener(OnSave);
        _copyButton.onClick.AddListener(OnCopy);
        _closeButton.onClick.AddListener(OnClose);
    }

    private void OnSelect()
    {
        _codeLanguagesView.SetActive(true);
    }

    private void SelectLanguage(CodeLanguage language, string title)
    {
        _language = language;
        _selectButton.GetComponentInChildren<Text>().text = title;
        _codeLanguagesView.SetActive(false);
    }

    private void PrintCode()
    {
        //for (int i = 0; i < _codeView.content.childCount; i++)
        //{
        //    Destroy(_codeView.content.GetChild(i));
        //}
        _code = _codeTranslator.GenerateCode(_program, _language);
        foreach (var line in _code.Split('\n'))
        {
            var codeLine = Instantiate(DataProvider.CodeLinePrefab, new Vector3(), new Quaternion());
            codeLine.transform.SetParent(_codeView.content);
            codeLine.transform.localPosition = new Vector3();
            codeLine.transform.localScale = new Vector3(1, 1, 1);
            codeLine.text = line;
        }
    }

    private void OnSave()
    {

    }

    private void OnCopy()
    {
        var te = new TextEditor();
        te.text = _code;
        te.SelectAll();
        te.Copy();
    }

    private void OnClose()
    {
        Destroy(gameObject);
    }
}
