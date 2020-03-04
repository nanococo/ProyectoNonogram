using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class nonogramSolver : MonoBehaviour {
    private List<List<int>> X = new List<List<int>>();
    private List<List<int>> Y = new List<List<int>>();

    // Start is called before the first frame update
    void Start() {
        string[] lines = System.IO.File.ReadAllLines(@"Assets\Scripts\input.txt");
        
        foreach (string line in lines) {
            Debug.Log("\t" + line);


        }

    }

    // Update is called once per frame
    void Update() {

    }
}
