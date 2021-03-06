﻿using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Windows;
using UnityEditor;
using UnityEngine.UI;


public class MainMenu : MonoBehaviour
{

    public static bool showAnimation;

    public static string puzzlePath = Path.GetFullPath(@"Assets/Scripts/Puzzles/5x5_45.txt");

    public GameObject ShowAnimationGameObject;
    
    
    public void SolveNonogram()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void backToMainMenu()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
        showAnimation = false;
    }
    
    public void ToggleValueChanged(bool change)
    {
        showAnimation = ShowAnimationGameObject.GetComponent<Toggle>().isOn;
        Debug.Log(showAnimation);
    }

    public void selectPuzzleFile() {
        //showAnimation = false;
        //ShowAnimationGameObject.GetComponent<Toggle>().isOn = false;
        string path = EditorUtility.OpenFilePanel(
            "Select a puzzle to solve",
            Path.GetFullPath(@"Assets/Scripts/Puzzles"), 
            "txt");
        puzzlePath = path;
    }
    
    
    
}
