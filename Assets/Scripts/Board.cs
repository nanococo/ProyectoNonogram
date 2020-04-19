using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour {

    private int _width;
    private int _height;

    // Start is called before the first frame update
    void Start() {
        //Instantiate(cell, new Vector2(0,0), Quaternion.identity);
    }

    public static Board MakeFooObject(int height, int width) {
        GameObject go = new GameObject("Board");
        Board ret = go.AddComponent<Board>();
        ret._height = height;
        ret._width = width;
        return ret;
    }

    public void SetWidth(int width) {
        this._width = width;
    }

    public void SetHeight(int height) {
        this._height = height;
    }

    public void Draw(GameObject cell) {
        Debug.Log(this._width);
        int xDisplacement = 0;
        for (int i = 0; i < this._width; i++) {
            Instantiate(cell, new Vector2(xDisplacement, 0), Quaternion.identity);
            xDisplacement += 4;
        }
    }

    // Update is called once per frame
    void Update() {

    }
}
