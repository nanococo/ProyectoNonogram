using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Line))]
public class Tests // : MonoBehaviour
{
  public static List<int> testClues = new List<int> {1,1,1,2};
  public Row testRow = new Row(testClues, 10, 2);

  public Tests()
  {

  }
  
}
