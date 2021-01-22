using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PathSubmit : MonoBehaviour
{
    public InputField input;
    public MapCreate creator;
    public void ChangePath()
    {
        string content = input.text;
        if(content.Length >= 1&&content.Length < 60)
            creator.ChangePath(content);
    }

}
