using UnityEngine;
using UnityEngine.UI;
using System.Diagnostics;
using System;
public class FileExplorerComposition : MonoBehaviour
{
    public Button openPicturesButton;

    void Start()
    {
        openPicturesButton.onClick.AddListener(OpenPicturesFolder);
    }

    void OpenPicturesFolder()
    {
        // Open the Pictures folder
        Process.Start("explorer.exe", Environment.GetFolderPath(Environment.SpecialFolder.MyPictures));
    }
}
