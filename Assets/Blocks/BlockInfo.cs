using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[Serializable]
public class BlockInfo
{
    TypeBlock _blockType;

    int _x, _y;

    int _operativeFieldIndex, _accessoryFieldIndex;

    TypeDirection _direction;
    TypeDirection _falseDirection;

    Dictionary<TypeInfo, object> _additionalInfo;

    public TypeBlock BlockType => _blockType;

    public int X => _x;
    public int Y => _y;

    public int OperativeFieldIndex => _operativeFieldIndex;
    public int AccessoryFieldIndex => _accessoryFieldIndex;

    public TypeDirection Direction => _direction;
    public TypeDirection FalseDirection => _falseDirection;

    public Dictionary<TypeInfo, object> AdditionalInfo => _additionalInfo;

    public BlockInfo(TypeBlock blockType, TypeDirection direction, TypeDirection falseDirection = TypeDirection.West, 
        int x = -1, int y = -1, int operativeFieldIndex = 0, int accessoryFieldIndex = 1, 
        Dictionary<TypeInfo, object> additionalInfo = null)
    {
        _blockType = blockType;

        _x = x;
        _y = y;

        _direction = direction;
        _falseDirection = falseDirection;

        _operativeFieldIndex = operativeFieldIndex;
        _accessoryFieldIndex = accessoryFieldIndex;

        _additionalInfo = additionalInfo;
        if (_additionalInfo == null) _additionalInfo = new Dictionary<TypeInfo, object>();
    }

}
