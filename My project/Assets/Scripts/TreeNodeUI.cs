using TMPro;
using UnityEngine;

public class TreeNodeUI : MonoBehaviour
{
    public TMP_Text valueText;

    public void SetValue(int value)
    {
        valueText.text = value.ToString();
    }
}
