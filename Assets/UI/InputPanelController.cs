using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public delegate void InputValuesChangedHandler(List<int> values);

public class InputPanelController : MonoBehaviour
{
    [SerializeField]
    GameObject textObject;

    Text inputText;

    string valuesString = "";

    InputValuesChangedHandler onValuesChanged;

    Dictionary<KeyCode, char> inputKeys = new Dictionary<KeyCode, char>()
    {
        { KeyCode.Alpha0, '0' },
        { KeyCode.Alpha1, '1' },
        { KeyCode.Alpha2, '2' },
        { KeyCode.Alpha3, '3' },
        { KeyCode.Alpha4, '4' },
        { KeyCode.Alpha5, '5' },
        { KeyCode.Alpha6, '6' },
        { KeyCode.Alpha7, '7' },
        { KeyCode.Alpha8, '8' },
        { KeyCode.Alpha9, '9' }
    };

    public void Initialize(Transform content, List<int> values, 
        InputValuesChangedHandler valuesChangedHandler)
    {
        transform.SetParent(content, false);

        if (values.Count > 0)
        {
            valuesString = values[0].ToString();
            for (int i = 1; i < values.Count; i++)
                valuesString += string.Format(" {0}", values[i]);
            inputText.text = valuesString.Replace(" ", ", ");
        }

        onValuesChanged = valuesChangedHandler;
    }
    
    void Awake()
    {
        inputText = textObject.GetComponent<Text>();
    }
    
    void Update()
    {
        if (Input.anyKeyDown)
            KeysHandler();
    }

    void KeysHandler()
    {
        var len = inputText.text.Length;

        foreach (var key in inputKeys.Keys)
            if (Input.GetKeyDown(key))
            {
                var digit = inputKeys[key];
                inputText.text += digit;
                valuesString += digit;
                OnInputChanged();
            }

        if (Input.GetKeyDown(KeyCode.Space) && len > 0 && inputText.text[len - 1] != ' ')
        {
            inputText.text += ", ";
            valuesString += ' ';
        }

        //if (Input.GetKeyDown(KeyCode.Backspace))
        //{
        //    inputText.text.Remove(len - 1);
        //    OnInputChanged();
        //}
    }

    void OnInputChanged()
    {
        var values = valuesString.Split(' ')
            .Where(s => s != "").Select(s => System.Convert.ToInt32(s)).ToList();

        onValuesChanged?.Invoke(values);
    }
}
