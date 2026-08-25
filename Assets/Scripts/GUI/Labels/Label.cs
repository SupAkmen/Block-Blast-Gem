using System;
using TMPro;
using UnityEngine;

public class Label : MonoBehaviour
{
    public TextMeshProUGUI label;

    private void Awake()
    {
        if (label == null)
        {
            label = GetComponent<TextMeshProUGUI>();
        }
    }
}
