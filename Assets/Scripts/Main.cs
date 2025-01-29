using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Main : MonoBehaviour
{
    [SerializeField]
    Importer Importer;
    [SerializeField]
    GameObject Cell;
    RectTransform rectTransform;
    Level curLevel;
    [SerializeField]
    CameraCanvas cameraCanvas;
    [SerializeField]
    CellController[,] cells;
    Word previusWord;
    [SerializeField]
    GameObject panel;
    List<TextAsset> textAssets = new();
    [SerializeField]
    GameObject menu;
    [SerializeField]
    GameObject scroll;
    [SerializeField]
    GameObject lvlButton;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        StartCoroutine(ShowKeyboard());
        var tt = Resources.LoadAll("Scanwords", typeof(TextAsset));
        foreach(var t in tt)
        {
            textAssets.Add(t as TextAsset);
        }
        if(textAssets.Count != 0)
        {
            textAssets.Reverse();
            FillLvlList();
        }
    }
    void FillLvlList()
    {
        for(int i = 0; i < textAssets.Count;i++)
        {
            GameObject curButton = Instantiate(lvlButton, new Vector3(0, i), Quaternion.identity, scroll.transform);
            TextAsset copy = textAssets[i];
            curButton.GetComponent<Button>().onClick.AddListener( () => LoadLevel(copy));
            curButton.GetComponentInChildren<TextMeshProUGUI>().text = copy.name;
        }

    }


    IEnumerator ShowKeyboard()
    {
        yield return new WaitUntil(() => previusWord != null);
        panel.SetActive(true);

    }
    public void SelectQuest(string text)
    {
        if(previusWord != null)
        {
            cells[previusWord.position.Item1, previusWord.position.Item2].Outlined();
            for (int i = previusWord.startX; i <= previusWord.endX; i++)
            {
                for (int j = previusWord.startY; j <= previusWord.endY; j++)
                {
                    if (cells[i, j].cell.status == CellStatus.SelectWithWord || cells[i, j].cell.status == CellStatus.Selected)
                    {
                        cells[i, j].cell.status = CellStatus.Idle;
                        cells[i, j].Outlined();
                    }
                }
            }
        }
        Word word = curLevel.words.Find(word => word.Text == text);
        cells[word.position.Item1, word.position.Item2].Outlined();
        List<Cell> cellList = new();
        for(int i = word.startX; i <= word.endX; i++)
        {
            for(int j = word.startY; j <= word.endY; j++)
            {
                if (cells[i,j].cell.status == CellStatus.Idle)
                {
                    cells[i, j].cell.status = CellStatus.SelectWithWord;
                    cells[i, j].Outlined();
 
                }
                cellList.Add(cells[i, j].cell);
            }
        }
        previusWord = word;
        cameraCanvas.FillInputPanel(word ,cellList);
    }

    public void FillCell(int id, string text)
    {
        Word word = curLevel.words.Find(word => word.id == id);
    }

    public void WordComlited(string text)
    {
        Word word = curLevel.words.Find(word=>word.Text == text);
        cells[word.position.Item1, word.position.Item2].cell.status = CellStatus.Complited;
        cells[word.position.Item1, word.position.Item2].Outlined();
        CellChanged(cells[word.position.Item1,word.position.Item2].cell);
        for (int i = word.startX; i <= word.endX; i++)
        {
            for (int j = word.startY; j <= word.endY; j++)
            {
                if (cells[i, j].cell.status == CellStatus.Selected || cells[i, j].cell.status == CellStatus.SelectWithWord)
                {
                    cells[i, j].cell.status = CellStatus.Complited;
                    cells[i, j].Outlined();
                }
                if (cells[i,j].cell.status == CellStatus.Complited)
                {
                    cells[i, j].Outlined();
                }
            }
        }
    }

    public void CellChanged(Cell cell)
    {
        Importer.SaveCell(cell);
    }

    public void SelectIdle((int,int) position)
    {
        List<Word> finded = curLevel.words.FindAll(word => word.map.Contains(position));
        if(finded.Count > 2)
        {
            print("недоразумение");
        }
        if (finded.First() == previusWord) SelectQuest(finded.Last().Text);
        else SelectQuest(finded.First().Text);
    }
    public void SelectSelected((int,int) position)
    {
        List<Word> finded = curLevel.words.FindAll(word => word.map.Contains(position));
        if(finded.Count > 2)
        {
            print("недоразумение");
        }
        else
        {

        }
        if (finded.Last() == previusWord) SelectQuest(finded.First().Text);
        else SelectQuest(finded.Last().Text);
    }
    // Update is called once per frame
    void Update()
    {
        
    }

    void LevelComplited() { print("complited"); }

    public void ToNext(int id)
    {
        bool flag = false;
        foreach (var word in curLevel.words)
        {
            if (curLevel.cells[word.position.Item1, word.position.Item2].status == CellStatus.Quest) { flag = true; break; };

        }
        if (!flag)
        {
            LevelComplited();
        }
        if (curLevel.words.Count < id + 1)
        {
            int i = 1;
            Word word = curLevel.words.Find(word => word.id == i);
            while (curLevel.cells[word.position.Item1, word.position.Item2].status != CellStatus.Quest) { i++; word = curLevel.words.Find(word => word.id == i); }
            SelectQuest(word.Text);
        }
        else { 
            int i = id+1;
            Word word = curLevel.words.Find(word => word.id == i);
            while (curLevel.cells[word.position.Item1, word.position.Item2].status != CellStatus.Quest) { if (curLevel.words.Count > i) i++; else i = 0; word = curLevel.words.Find(word => word.id == i); }
            SelectQuest(word.Text);

        }
    }
    public void ToPrev(int id) 
    { 
        if(id - 1 < 0)
        {
            int i = curLevel.words.Count;
            Word word = curLevel.words.Find(word => word.id == i);
            while (curLevel.cells[word.position.Item1, word.position.Item2].status != CellStatus.Quest)
            {
                i--;
                word = curLevel.words.Find(word => word.id == i);
            }
            SelectQuest(word.Text);
        }
        else
        {
            int i = id-1;
            Word word = curLevel.words.Find(word => word.id == i);
            while (curLevel.cells[word.position.Item1, word.position.Item2].status != CellStatus.Quest)
            {
                if (i > 1)
                    i--;
                else
                    i = curLevel.words.Count;
                word = curLevel.words.Find(word => word.id == i);
            }
        }
    }



    public void LoadLevel(TextAsset lvl)
    {
        menu.SetActive(false);
        curLevel = Importer.LoadLevel(lvl);
        cells = new CellController[curLevel.width,curLevel.height];
        int t = 105;
        for (int i = 0; i < curLevel.width; i++) {
            for (int j = 0; j < curLevel.height; j++)
            {
                GameObject curCell = Instantiate(Cell, Vector2.zero, Quaternion.identity, gameObject.transform);
                curCell.GetComponent<CellController>().cell = curLevel.cells[i,j];
                curCell.GetComponent<RectTransform>().localPosition = new Vector2(-Mathf.RoundToInt(rectTransform.rect.width / 2 - rectTransform.rect.width / 10) + t*i, Mathf.RoundToInt(rectTransform.rect.height / 2 - rectTransform.rect.height / 10) - t*j);
                curCell.GetComponentInChildren<TextMeshProUGUI>().text = curLevel.cells[i,j].text;
                curCell.GetComponent<CellController>().main = this;
                cells[i, j] = curCell.GetComponent<CellController>();
            }
        }
    
    
    
    }
}
