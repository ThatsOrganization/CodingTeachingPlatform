using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class StorageBlock : UnaryFunctionBlock
{
    [SerializeField]
    GameObject valuesTipPrefab;

    [SerializeField]
    GameObject wavesObject;

    List<int> _values = new List<int>();

    public List<int> Values => _values;

    public override TypeBlock BlockType => TypeBlock.Storage;

    public override void BlockAction(Runner runner)
    {
        //wavesObject.GetComponent<ParticleSystem>().Play();
        _values.Add(OperativeField.Value);
        runner.Move(Direction);
    }

    protected override BlockTipController ShowTip()
    {
        var tip = base.ShowTip();

        if (tip == null) return null;

        if (Values.Count > 0)
        {
            var valuesTip = Instantiate(valuesTipPrefab);
            valuesTip.GetComponent<Text>().text = Values.First().ToString() + 
                string.Concat(Values.Skip(1).Select(v => string.Format(", {0}", v.ToString())));
            tip.AddComponent(valuesTip.transform);
        }

        return tip;
    }

    protected override string GetDataTip() => OperativeField.Name;
}
