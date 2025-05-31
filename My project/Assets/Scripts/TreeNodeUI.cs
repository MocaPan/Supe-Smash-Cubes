using TMPro;
using UnityEngine;

public class TreeNodeUI : MonoBehaviour
{
    public TMP_Text numberText;

    public void SetValue(int value)
    {
        numberText.text = value.ToString();
    }
}

