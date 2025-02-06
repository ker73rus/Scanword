using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using UnityEngine;

public class Importer : MonoBehaviour
{
    XmlDocument xDoc;
    TextAsset xTextAsset;
    string path = "";
    public Level LoadLevel(TextAsset textAsset)
    {
        try
        { 
            XmlDocument xmlDoc = new XmlDocument();
            xTextAsset = textAsset;
            if (!File.Exists(path + textAsset.name + ".xml")) {
                xmlDoc.LoadXml(textAsset.text);
            }
            else
            {
                xmlDoc.Load(path + textAsset.name + ".xml");
            }
            XmlElement? xmlRoot = xmlDoc.DocumentElement;
            xDoc = xmlDoc;
            if (xmlRoot != null)
                foreach (XmlNode xmlNode in xmlRoot.ChildNodes.Item(0).ChildNodes)
                {
                    if (xmlNode.Name == "crossword")
                    {
                        Level reult = new Level();
                        foreach (XmlNode xnode in xmlNode)
                        {
                            if (xnode.Name == "grid")
                            {
                                int width = int.Parse(xnode.Attributes[0].Value);
                                int height = int.Parse(xnode.Attributes[1].Value);
                                reult = new Level(width, height);
                                reult.name = textAsset.name;
                                foreach (XmlNode xCell in xnode.ChildNodes)
                                {
                                    if (xCell.Name != "cell")
                                        continue;
                                    Cell cell = new();
                                    cell.position = (int.Parse(xCell.Attributes[0].Value) - 1, int.Parse(xCell.Attributes[1].Value) - 1);
                                    if (xCell.Attributes[2].Value == "clue")
                                    {
                                        cell.text = xCell.LastChild.InnerText;
                                        if(xCell.Attributes.Count > 4 && xCell.Attributes[4].Name == "status")
                                        {
                                            cell.status = CellStatus.Complited;
                                        }
                                        else cell.status = CellStatus.Quest;
                                        cell.bar = Bar.None;
                                        reult.words.Add(new Word(id: int.Parse(xCell.LastChild.Attributes[0].Value), cell.text, (int.Parse(xCell.Attributes[0].Value) -1, int.Parse(xCell.Attributes[1].Value) - 1)));
                                        reult.words.Find(word => word.id.ToString() == xCell.LastChild.Attributes[0].Value).Text = cell.text;
                                    }
                                    else if (xCell.Attributes[2].Name == "solution" || xCell.Attributes[3].Name == "solution")
                                    {
                                        cell.status = CellStatus.Idle;
                                        cell.text = "";
                                        if (xCell.Attributes[2].Name == "solution")
                                        {
                                            cell.solution = xCell.Attributes[2].Value;
                                            cell.bar = Bar.None;
                                            if (xCell.Attributes.Count > 3 && xCell.Attributes[3].Value == "Complited")
                                            {
                                                cell.status = CellStatus.Complited;
                                                cell.text = cell.solution;
                                            }
                                        }
                                        else { cell.solution = xCell.Attributes[3].Value;
                                            string bar = xCell.Attributes[2].Name;
                                            bar = bar.Substring(0, bar.IndexOf("-"));
                                            switch (bar)
                                            {
                                                case "left":
                                                    cell.bar = Bar.Left; break;
                                                case "right":
                                                    cell.bar = Bar.Right; break;
                                                case "top":
                                                    cell.bar = Bar.Top; break;
                                                case "bottom":
                                                    cell.bar = Bar.Bottom; break;
                                            }
                                            if (xCell.Attributes.Count > 4 && xCell.Attributes[4].Value == "Complited")
                                            {
                                                cell.status = CellStatus.Complited;
                                                cell.text = cell.solution;
                                            }
                                        }
                                        if (xCell.HasChildNodes && xCell.LastChild.Name == "arrow")
                                        {
                                            switch (xCell.LastChild.Attributes[0].Value)
                                            {
                                                case "top":
                                                    switch (xCell.LastChild.Attributes[1].Value)
                                                    {
                                                        case "right":
                                                            cell.arrow = Arrow.TopToRight;
                                                            break;
                                                        case "left":
                                                            cell.arrow = Arrow.TopToLeft;
                                                            break;
                                                        case "bottom":
                                                            cell.arrow = Arrow.TopToBottom;
                                                            break;
                                                    }
                                                    break;
                                                case "bottom":
                                                    switch (xCell.LastChild.Attributes[1].Value)
                                                    {
                                                        case "right":
                                                            cell.arrow = Arrow.BottomToRight;
                                                            break;
                                                        case "left":
                                                            cell.arrow = Arrow.BottomToLeft;
                                                            break;
                                                        case "top":
                                                            cell.arrow = Arrow.BottomToTop;
                                                            break;
                                                    }
                                                    break;
                                                case "left":
                                                    switch (xCell.LastChild.Attributes[1].Value)
                                                    {
                                                        case "right":
                                                            cell.arrow = Arrow.LeftToRight;
                                                            break;
                                                        case "top":
                                                            cell.arrow = Arrow.LeftToTop;
                                                            break;
                                                        case "bottom":
                                                            cell.arrow = Arrow.LeftToBottom;
                                                            break;
                                                    }
                                                    break;
                                                case "right":
                                                    switch (xCell.LastChild.Attributes[1].Value)
                                                    {
                                                        case "top":
                                                            cell.arrow = Arrow.RightToTop;
                                                            break;
                                                        case "left":
                                                            cell.arrow = Arrow.RightToLeft;
                                                            break;
                                                        case "bottom":
                                                            cell.arrow = Arrow.RightToBottom;
                                                            break;
                                                    }
                                                    break;
                                            }
                                        }
                                        else cell.arrow = Arrow.None;
                                    }
                                    reult.cells[cell.position.Item1, cell.position.Item2] = cell;
                                }
                            }
                            else if (xnode.Name == "word")
                            {
                                int id = int.Parse(xnode.Attributes[0].Value);
                                string x = xnode.Attributes[1].Value;
                                string y = xnode.Attributes[2].Value;
                                int startX = x.Contains("-") ? int.Parse(x.Substring(0, x.IndexOf("-"))) : int.Parse(x);
                                int endX = x.Contains("-") ? int.Parse(x.Substring(x.IndexOf("-"), x.Length - x.IndexOf("-")).Remove(0, 1)) : int.Parse(x);
                                int startY = y.Contains("-") ? int.Parse(y.Substring(0, y.IndexOf("-"))) : int.Parse(y);
                                int endY = y.Contains("-") ? int.Parse(y.Substring(y.IndexOf("-"), y.Length - y.IndexOf("-")).Remove(0, 1)) : int.Parse(y);

                                Word word = reult.words.Find(word => word.id == id);
                                word.AddCells(startX -1 ,startY -1 ,endX - 1,endY - 1);

                                string solution = "";
                                foreach(var pos in word.map)
                                {
                                   solution += reult.cells[pos.Item1, pos.Item2].solution;
                                }
                                word.AddSolution(solution);
                                if (xnode.Attributes.Count > 3 && xnode.Attributes[3].Name == "image") {
                                    word.image = true;
                                }

                            }
                        }
                        return reult;
                    }
                        
                    
                }
                
        }
        catch(Exception e) {
            print("EX:" + e);
        }
        return LoadDefault();
        
    }
    public Level LoadDefault()
    {
        return new Level(1, 1);
    }
    public void Go()
    {
        LoadLevel(new TextAsset());   
    }

    public void SaveCell(Cell cell)
    {
        XmlNode grid = xDoc.ChildNodes[1].ChildNodes[0].ChildNodes[1].ChildNodes[0];
        foreach (XmlElement node in grid.ChildNodes) {
            if(node.Name == "cell")
            {
                if ( (int.Parse(node.Attributes[0].Value) - 1,int.Parse(node.Attributes[1].Value) - 1 ) == cell.position)
                {
                    node.SetAttribute("status", "Complited");
                }
            }
        }
        xDoc.Save(path + xTextAsset.name + ".xml");
    }
    private void Start()
    {
        path = Application.temporaryCachePath + "/";
    }
}
