using System.Collections;
using System.Collections.Generic;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

public class Matrix 
{
    
    private int height;
    private int length;
    private List<List<int>> matrixRepresentation;
    private List<List<int>> rowClues;
    private List<List<int>> columnClues;

    private List<Row> _rows;
    private List<Column> _columns;

    private bool skip = false;
    public GameObject graphicCell;

    public Matrix()
    {
        matrixRepresentation = new List<List<int>>();
        
        rowClues = new List<List<int>>();
        columnClues = new List<List<int>>();
        
        _rows = new List<Row>();
        _columns = new List<Column>();
        
        createCluesMatrix();
        createLogicalMatrixRepresentation();
        createRows();
        createColumns();
        
        test();

        

    }

    public void test()
    {
        Debug.Log("-----------------------------------------------");
        Tests testClass = new Tests();
        Debug.Log("-----------------------------------------------");
        Debug.Log("MATRIX");
        Debug.Log(printMatrix(matrixRepresentation));
        Debug.Log("MATRIX");
        Debug.Log("-----------------------------------------------");
        solveMatrix();
        Debug.Log("ROWS");
        Debug.Log(listToString(_rows));
        Debug.Log("ROWS");
        Debug.Log("-----------------------------------------------");
        Debug.Log("COLUMNS");
        Debug.Log(listToString(_columns));
        Debug.Log("COLUMNS");
        
    }
    
    
    

    private void createLogicalMatrixRepresentation() {

        for (int i = 0; i < height; i++) {
            List<int> row = new List<int>();
            for (int j = 0; j < length; j++) {
                row.Add(9);
            }

            matrixRepresentation.Add(row);
        }

    }

    private void createCluesMatrix() {
        string[] lines = System.IO.File.ReadAllLines(@"Assets\Scripts\input.txt");
        bool rows = true;
        foreach (string line in lines) {
            if (!skip) {
                skip = true;
                string[] words = line.Split(',');
                length = int.Parse(words[0].Trim());
                height = int.Parse(words[1].Trim());
            }
            else {
                if (!line.Contains("FILAS")) {
                    if (line.Contains("COLUMNAS")) {
                        rows = false;
                    }
                    else {  
                        string[] words = line.Split(',');

                        List<int> listToAdd = new List<int>();
                        
                        foreach (string number in words)
                        {
                            listToAdd.Add(int.Parse(number.Trim()));
                        }
                        
                        if (rows) {
                            rowClues.Add(listToAdd);
                        }
                        else {
                            columnClues.Add(listToAdd);
                        }
                    }
                }
            }
        }
    }
    
    private void createRows()
    {
        for (int index = 0; index < length; index++)
        {
            _rows.Add(createRow(index));
        }
        
    }

    private Row createRow(int index)
    {
        return new Row(rowClues[index], height, index);
    }

    private void createColumns()
    {
        for (int index = 0; index < length; index++)
        {
            _columns.Add(createColumn(index));
        }

    }

    private Column createColumn(int index)
    {
        return new Column(columnClues[index], length, index);
    }

    private void solveMatrix()
    {
        Debug.Log("Rows");
        goThroughRows();
        Debug.Log("Columns");
        goThroughColumns();
        
    }
    
    

    private void goThroughRows()
    {
        foreach (var row in _rows)
        {
            row.analyzeLine();
            updateColumns(row);
        }
    }

    private void goThroughColumns()
    {
        foreach (var column in _columns)
        {
            column.analyzeLine();
            updateRows(column);
        }
    }
    

    private void updateColumns(Line row)
    {
        foreach (var column in _columns)
        {
            column.refresh(row);
            if (column.isComplete()) column.discardLeftSpaces();
        }
    }
    private void updateRows(Line column)
    {
        foreach (var row in _rows)
        {
            row.refresh(column);
            if (row.isComplete()) row.discardLeftSpaces();
        }
    }
    

    
    public string printMatrix<T>(List<List<T>> matrix) {
        string test = "";
        foreach (var list in matrix)
        {
            test += listToString(list) + ",\n";
        }
        
        return test;
    }
    
    public string listToString<T>(List<T> list)
    {
        string result = "";
        foreach (var element in list)
        {
            result += element + "," + "\n";
        }

        result += "";
        return result;
    }
    
    
}
