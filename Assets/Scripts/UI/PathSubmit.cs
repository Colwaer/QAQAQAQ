using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using Map;
public class PathSubmit : MonoBehaviour
{
    public InputField input;
    public MapCreate creator;
    public Text text;
    public void ChangePath()
    {
        string content = input.text;
        //Debug.Log(content);
        if (content.Length >= 1 && content.Length < 60)
            creator.ChangePath(Path.Combine(Application.persistentDataPath, content));

    }

}
