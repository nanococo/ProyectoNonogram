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

    private List<Line> _rows;
    private List<Line> _columns;

    private bool changeFlag;
    

    public Matrix(List<List<int>> pMatrixrepresentation, List<List<int>> pRowClues, List<List<int>> pColumnClues, int pHeight, int pLength)
    {

        changeFlag = false;
        
        matrixRepresentation = pMatrixrepresentation;
        rowClues = pRowClues;
        columnClues = pColumnClues;
        
        _rows = new List<Line>();
        _columns = new List<Line>();

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

    private Line createRow(int index)
    {
        return new Line(rowClues[index], height, index);
    }

    private void createColumns()
    {
        for (int index = 0; index < height; index++)
        {
            _columns.Add(createColumn(index));
        }

    }

    private Line createColumn(int index)
    {
        return new Line(columnClues[index], length, index);
    }

    private void solveMatrix()
    {
        int counter = 0;
        do
        {
            resetThisChangeFlag();
            Debug.Log("Rows" + counter);
            goThroughRows();
            Debug.Log(listToString(_rows));
            Debug.Log("Columns" + counter);
            goThroughColumns();
            Debug.Log(listToString(_rows));
            resetLinesChangeFlags(_rows);
            resetLinesChangeFlags(_columns);
            counter++;
        } while (changeFlag);
        
         
     
    }
    
    private void goThroughRows()
    {
        foreach (var row in _rows)
        {
            row.analyzeLine();
            if (row.wasChanged()) changeFlag = true;
            updateColumns(row);
        }
    }

    private void goThroughColumns()
    {
        foreach (var column in _columns)
        {
            column.analyzeLine();
            if (column.wasChanged()) changeFlag = true;
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

    private void resetLinesChangeFlags(List<Line> pLineList)
    {
        foreach (var line in pLineList)
        {
            line.setChangeFlag(false);
        }
    }

    private void resetThisChangeFlag()
    {
        this.changeFlag = false;
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
