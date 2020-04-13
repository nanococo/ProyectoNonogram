﻿using System.Collections;
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
    

    public Matrix(List<List<int>> pMatrixrepresentation, List<List<int>> pRowClues, List<List<int>> pColumnClues, int pHeight, int pLength)
    {
        matrixRepresentation = pMatrixrepresentation;
        rowClues = pRowClues;
        columnClues = pColumnClues;
        
        _rows = new List<Row>();
        _columns = new List<Column>();

        height = pHeight;
        length = pLength;

        createRows();
        createColumns();
        
        test();

        
    }

    public void test()
    {
        solveMatrix();
        
        Debug.Log("ROWS");
        Debug.Log(listToString(_rows));
        Debug.Log("ROWS");
        Debug.Log("-----------------------------------------------");
        Debug.Log("COLUMNS");
        Debug.Log(listToString(_columns));
        Debug.Log("COLUMNS");
        
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
        for (int index = 0; index < height; index++)
        {
            _columns.Add(createColumn(index));
        }

    }

    private Column createColumn(int index)
    {
        Debug.Log(index);
        return new Column(columnClues[index], length, index);
    }

    private void solveMatrix()
    {
        Debug.Log("Rows1");
        goThroughRows();
        Debug.Log(listToString(_rows));
        Debug.Log("Columns1");
        goThroughColumns();
        Debug.Log(listToString(_rows));
        Debug.Log("Rows2");
        goThroughRows();
        Debug.Log(listToString(_rows));
        Debug.Log("Columns2");
        goThroughColumns();
        Debug.Log(listToString(_rows));
        Debug.Log("Rows3");
        goThroughRows();
        Debug.Log(listToString(_rows));
        Debug.Log("Columns3");
        goThroughColumns();
        Debug.Log(listToString(_rows));
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
        }
    }
    private void updateRows(Line column)
    {
        foreach (var row in _rows)
        {
            row.refresh(column);
        }
    }
    

    
    public string printMatrix<T>(List<List<T>> matrix) {
        string test = "";
        foreach (var list in matrix)
        {
            test += listToString(list) + "\n";
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
