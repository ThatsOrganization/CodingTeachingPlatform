using UnityEngine;

public class GridCell : MonoBehaviour
{
    int _x, _y;
    
    public FunctionalBlock Block { get; set; }

    public int X => _x;
    public int Y => _y;

    public GridCell()
    {
        Block = null;
    }

    public void Initialize(int x, int y)
    {
        _x = x;
        _y = y;
    }
}
