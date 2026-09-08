using TMPro;
using UnityEngine;

namespace Block_Blast.Scripts
{
    public class ComboText : MonoBehaviour
    {
        public TextMeshProUGUI text;
        public Animator animator;

        private void SetText(int comboCount)
        {
            text.text = "x" + comboCount.ToString();
            text.alignment = TextAlignmentOptions.Center;
        }

        public void Show(int comboCount)
        {
            SetText(comboCount);
        }
    }
}