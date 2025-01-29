using UnityEngine;
using UnityEngine.UI;
public enum CellStatus
{
    Idle = 0,
    Complited = 1,
    Selected = 2,
    Quest = 3,
    SelectWithWord = 4
}
public enum Arrow
{
    None = 0,
    TopToBottom = 1,
    BottomToTop = 2,
    LeftToRight = 3,
    RightToLeft = 4,
    TopToRight = 5,
    BottomToRight = 6,
    TopToLeft = 7,
    BottomToLeft = 8,
    LeftToTop = 9,
    RightToTop = 10,
    LeftToBottom = 11,
    RightToBottom = 12,
}
public class Cell
{
    public Arrow arrow;
    public CellStatus status;
    public (int,int) position;
    public string text;
    public string solution;

}
