﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BombsDisplay : MonoBehaviour
{
    public PlayerController playerController;
    public Text bombsNumberText;

    private void Update()
    {
        bombsNumberText.text = "" + playerController.bombsNumber;
    }

}