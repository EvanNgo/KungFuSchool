﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Option", menuName = "Option")]
public class Option : ScriptableObject {
    public string title;
    public StatManager.TagManager tag;
    public int maxPoint = 0;
    public int point = 0;
    public string unit;
    public int pointPerUnit = 1;
}
