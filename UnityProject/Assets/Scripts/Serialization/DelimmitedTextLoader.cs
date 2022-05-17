using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DelimmitedTextLoader
{
    private readonly TextAsset textAsset;

    private char delimmiterChar;

    public DelimmitedTextLoader(string fileName, char delimmiter)
    {
        textAsset = Resources.Load<TextAsset>(fileName);
        delimmiterChar = delimmiter;
    }

    public string GetRawData()
    {
        return textAsset.text;
    }

    public string[][] GetData()
    {
        string[] rowsRaw = textAsset.text.Split(new char[] { '\n' });
        List<string[]> parsedDataLst = new List<string[]>();

        for (int i = 0; i < rowsRaw.Length; i++)
        {
            if (rowsRaw[i] != "")
            {
                string[] parsedRow = rowsRaw[i].Split(new char[] { delimmiterChar });
                for (int j = 0; j < parsedRow.Length; j++)
                {
                    parsedRow[j] = parsedRow[j].Replace("\\n", "\n");
                    parsedRow[j] = parsedRow[j].Replace("\"", "");
                }
                parsedDataLst.Add(parsedRow);
            }
        }

        return parsedDataLst.ToArray();
    }

    public string[][] GetDataNoHeader()
    {
        string[][] allData = GetData();

        List<string[]> allRowsLst = new List<string[]>(allData);
        allRowsLst.RemoveAt(0);
        return allRowsLst.ToArray();
    }
}
