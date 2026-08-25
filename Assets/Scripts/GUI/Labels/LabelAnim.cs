using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LabelAnim : MonoBehaviour
{
     public Image icon;

     [SerializeField] private GameObject fxPrefab;
     [SerializeField] private TextMeshProUGUI coinsTextPrefab;
     [SerializeField] private ResourceObject asscociatedResource;
     
     private Tweener tweener;
     private Transform _fxParent;
}
