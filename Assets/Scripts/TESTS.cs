using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TESTS
{
    
    private List<int> _testClues = new List<int> {1, 8, 2, 2};
    private Line _testLine;
    private Backtracking backtracking;
    
    public TESTS()
    {
        _testClues = new List<int> {1, 8, 2, 2};
        _testLine = new Line(_testClues, 20, 7);
        backtracking = new Backtracking();
        setTest();
        Debug.Log(backtracking.IsSafeForTesting(_testLine));
        
    }

    public void setTest()
    {
        _testLine.Cells[0].Confirm();
        //_testLine.Cells[1].Confirm();
        // _testLine.Cells[2].Confirm();
        //_testLine.Cells[3].Confirm();
        //_testLine.Cells[4].Confirm();
        //_testLine.Cells[5].Confirm();
        //_testLine.Cells[6].Confirm();
        //_testLine.Cells[7].Confirm();
        //_testLine.Cells[8].Confirm();
        //_testLine.Cells[9].Confirm();
        //_testLine.Cells[10].Confirm();
        //_testLine.Cells[11].Confirm();
        //_testLine.Cells[12].Confirm();
        //_testLine.Cells[13].Confirm();
        //_testLine.Cells[14].Confirm();
        //_testLine.Cells[15].Confirm();
        //_testLine.Cells[16].Confirm();
        //_testLine.Cells[17].Confirm();
        //_testLine.Cells[18].Confirm();
        //_testLine.Cells[19].Confirm();
        
        /*
        _testLine.Cells[0].Discard();
        _testLine.Cells[1].Discard();
        _testLine.Cells[2].Discard();
        _testLine.Cells[3].Discard();
        _testLine.Cells[4].Discard();
        _testLine.Cells[5].Discard();
        _testLine.Cells[6].Discard();
        _testLine.Cells[7].Discard();
        _testLine.Cells[8].Discard();
        _testLine.Cells[9].Discard();
        _testLine.Cells[10].Discard();
        _testLine.Cells[11].Discard();
        _testLine.Cells[12].Discard();
        _testLine.Cells[13].Discard();
        _testLine.Cells[14].Discard();
        _testLine.Cells[15].Discard();
        _testLine.Cells[16].Discard();
        _testLine.Cells[17].Discard();
        _testLine.Cells[18].Discard();
        _testLine.Cells[19].Discard();
        */
    }




}
