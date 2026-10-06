using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "New Tutorial List", menuName = "Microkart/Tutorial List")]
public class TutorialList : ScriptableObject
{
    public Texture2D headerImage; // 👈 New field for image

    public string headerName;

    public List<Tutorial> tutorials = new List<Tutorial>();
}