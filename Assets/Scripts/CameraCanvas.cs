using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using static System.Net.Mime.MediaTypeNames;

public class CameraCanvas : MonoBehaviour
{
    [SerializeField]
    Button[] buttons;
    [SerializeField]
    TextMeshProUGUI quest;
    [SerializeField]
    List<GameObject> answer;
    string rightLetters = "";
    [SerializeField]
    Main main;
    List<Cell> cellList;
    Word curword;
    [SerializeField]
    GameObject answerLetter;
    [SerializeField]
    GameObject keyboard;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }
    public void NextWord()
    {
        main.ToNext(curword.id);
    }
    public void PreviousWord() 
    {
        main.ToPrev(curword.id);
    }


    public void FillInputPanel(Word word, List<Cell> cells)
    {
        curword = word;
        cellList = cells;
        rightLetters = "";
        quest.text = word.image ? "Что это?" : word.Text.Replace("-\\n", "").Replace("\\n", " ");
        answer.ForEach(letter => Destroy(letter));
        answer.Clear();
        if(word.alphabet.Length != 16)
        {
            print("pupupu");
        }

        for( int i = 0; i < cells.Count; i++){
            answer.Add(Instantiate(answerLetter, keyboard.transform));
            RectTransform rect = keyboard.GetComponent<RectTransform>();
            RectTransform letter = answer[i].GetComponent<RectTransform>();
            print(letter.sizeDelta);
            letter.sizeDelta = Vector2.one * ((rect.rect.width - 5 * cells.Count)/cells.Count) ;
            letter.localPosition = new Vector2(- rect.rect.width/2 +letter.rect.width/2 + (letter.rect.width + 5) * i, 0);
            answer[i].GetComponentInChildren<TextMeshProUGUI>().text = cells[i].status == CellStatus.Complited ? cells[i].text : "_";
            rightLetters += cells[i].solution;
        }
        for(int i = 0; i < 16; i++)
        {
            if (!buttons[i].interactable) buttons[i].interactable = true;
            buttons[i].GetComponentInChildren<TextMeshProUGUI>().text = word.alphabet[i].ToString();
            string output = word.alphabet[i].ToString().Clone() as string;
            Button but = buttons[i];
            but.onClick.RemoveAllListeners();
            but.onClick.AddListener( () => { ButtonOff(but); OnClick(output); });

        }
        cellList
            .FindAll(cell => cell.status == CellStatus.Complited)
            .ForEach(cell =>
               ButtonOff(
                   buttons.ToList().Find(a => a.interactable && a.GetComponentInChildren<TextMeshProUGUI>().text == cell.text)
               )
            );
    }


    void ButtonOff(Button but)
    {
        //but.interactable = false;
    }

    public void OnClick(string text)
    {
        GameObject letter = answer.Find(_ => _.GetComponentInChildren<TextMeshProUGUI>().text == "_");
        if (letter != null) {
            letter.GetComponentInChildren<TextMeshProUGUI>().text = text;
            letter.GetComponent<Button>().onClick.AddListener(() => RemoveLetter(letter));
            if(answer.Find(_ => _.GetComponentInChildren<TextMeshProUGUI>().text == "_") == null)
            {
                CheckAnswer();
            }
        }
        
    }
    public void RemoveLetter(GameObject letter)
    {
        foreach (var item in buttons)
        {
            if(item.GetComponentInChildren<TextMeshProUGUI>().text == letter.GetComponentInChildren<TextMeshProUGUI>().text)
            {
                item.interactable = true;
                letter.GetComponentInChildren<TextMeshProUGUI>().text = "_";
                break;
            }
        }
    }

    public void CheckAnswer()
    {
        string curAnswer = "";
        answer.ForEach(ans => curAnswer += ans.GetComponentInChildren<TextMeshProUGUI>().text);
        if (curAnswer == rightLetters)
        {
            main.WordComlited(curword.Text);
        }
    }


    public void DeleteLetters()
    {
        foreach (var but in buttons) {
            if (!rightLetters.Contains(but.GetComponentInChildren<TextMeshProUGUI>().text))
            {
                but.interactable = false;
            }
        }
    }
    public void OpenLetter()
    {
        foreach (var but in buttons)
        {
            if (rightLetters[answer.IndexOf(answer.Find(a => a.GetComponentInChildren<TextMeshProUGUI>().text == "_"))].ToString() == but.GetComponentInChildren<TextMeshProUGUI>().text)
            {
                but.onClick.Invoke();
                return;
            }
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
