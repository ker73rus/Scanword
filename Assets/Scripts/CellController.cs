using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CellController : MonoBehaviour
{
    public Cell cell;
    Button Button;
    public
    Image image;
    public
    Image backimage;
    public Main main;
    public TextMeshProUGUI text;
    [SerializeField]
    GameObject[] bars;
    void Start()
    { 
        text = GetComponentInChildren<TextMeshProUGUI>();
        Button = GetComponent<Button>();
        if (cell != null)
        {
            if (cell.arrow != Arrow.None)
            {
                switch (cell.arrow)
                {
                    case Arrow.TopToBottom:
                        image.sprite = Resources.Load("Pictures/TtB", typeof(Sprite)) as Sprite;
                        break;
                    case Arrow.TopToLeft:
                        image.sprite = (Sprite)Resources.Load("Pictures/TtL", typeof(Sprite));
                        break;
                    case Arrow.TopToRight:
                        image.sprite = (Sprite)Resources.Load("Pictures/TtR", typeof(Sprite));
                        break;
                    case Arrow.LeftToBottom:
                        image.sprite = (Sprite)Resources.Load("Pictures/LtB", typeof(Sprite));
                        break;
                    case Arrow.LeftToRight:
                        image.sprite = (Sprite)Resources.Load("Pictures/LtR", typeof(Sprite));
                        break;
                    case Arrow.LeftToTop:
                        image.sprite = (Sprite)Resources.Load("Pictures/LtT", typeof(Sprite));
                        break;
                    case Arrow.BottomToTop:
                        image.sprite = (Sprite)Resources.Load("Pictures/BtT", typeof(Sprite));
                        break;
                    case Arrow.BottomToLeft:
                        image.sprite = (Sprite)Resources.Load("Pictures/BtL", typeof(Sprite));
                        break;
                    case Arrow.BottomToRight:
                        image.sprite = (Sprite)Resources.Load("Pictures/BtR", typeof(Sprite));
                        break;
                    case Arrow.RightToTop:
                        image.sprite = (Sprite)Resources.Load("Pictures/RtT", typeof(Sprite));
                        break;
                    case Arrow.RightToLeft:
                        image.sprite = (Sprite)Resources.Load("Pictures/RtL", typeof(Sprite));
                        break;
                    case Arrow.RightToBottom:
                        image.sprite = (Sprite)Resources.Load("Pictures/RtB", typeof(Sprite));
                        break;
                }
            }
            if(cell.bar != Bar.None)
            {
                bars[(int)cell.bar].SetActive(true);
            }
        }
        Outlined();
    }
    void Update()
    {

    }
    public void OnClick()
    {
        if(cell.status == CellStatus.Quest)
        {
            main.SelectQuest(cell.text);
          
        }
        else if(cell.status == CellStatus.Idle)
        {
            main.SelectIdle(cell.position);
            cell.status = CellStatus.Selected;
            Outlined();
        }
        else if(cell.status == CellStatus.Selected)
        {
            main.SelectSelected(cell.position);
            cell.status = CellStatus.Selected;
            Outlined();
        }
        else if( cell.status == CellStatus.SelectWithWord)
        {
            main.SelectSelected(cell.position);
            cell.status = CellStatus.Selected;
            Outlined();
        }
            
    }
    public void Outlined()
    {
        Color color = GetComponent<Image>().color;
        if (cell.status == CellStatus.Quest)
        {
            if (color == Color.white)
            {
                ColorUtility.TryParseHtmlString("#69FFB6", out color);
            }
            else color = Color.white;
        }
        else if (cell.status == CellStatus.SelectWithWord)
        {
            ColorUtility.TryParseHtmlString("#FFE097", out color);
        }
        else if (cell.status == CellStatus.Selected)
        {
            ColorUtility.TryParseHtmlString("#FFBD21", out color);
        }
        else if(cell.status == CellStatus.Complited)
        {
            color = Color.grey;
        }
        else
            color = Color.white;

        GetComponent<Image>().color = color;
    }
}
