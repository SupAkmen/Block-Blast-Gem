using System;
using System.Collections;
using NUnit.Framework;
using UnityEngine;

public class ShapeSquare : MonoBehaviour
{
    [SerializeField] public  GameObject underlaySprite;
    [SerializeField] private GameObject topSprite;
    [SerializeField] private GameObject bottomSprite;
    [SerializeField] private GameObject leftSprite;
    [SerializeField] private GameObject rightSprite;
    [SerializeField] private GameObject overlaySprite;
    [SerializeField] private GameObject bonusSprite;
    
    private BoxCollider2D boxCollider2D;
    public bool IsActive;
    
    private Vector3 startLocalscale;
    
    // GridSquare dang bi chiem
    public bool IsOccupied { get; set; }
    
    private Shape shape;
    
    private void Awake()
    {
        boxCollider2D = GetComponent<BoxCollider2D>();
        shape = GetComponentInParent<Shape>();
    }

    private void Start()
    {
        startLocalscale = transform.localScale;
        if (!IsActive)
        {
            SetBlockVisual(false,false,false,false,false,false);
        }
        else if(IsActive)
        {
            SetBlockVisual(true,true,true,true,true,false);
        }
    }
    
    
    public void CopyVisualTo(ShapeSquare target)
    {
        target.SetBlockVisual(
            topSprite.activeSelf,
            bottomSprite.activeSelf,
            leftSprite.activeSelf,
            rightSprite.activeSelf,
            overlaySprite.activeSelf,
            bonusSprite.activeSelf
            );
    }

    public void CopyFrom(ShapeSquare source)
    {
        IsOccupied = true;
        
        SetBlockVisual(
            source.topSprite.activeSelf,
            source.bottomSprite.activeSelf,
            source.leftSprite.activeSelf,
            source.rightSprite.activeSelf,
            source.overlaySprite.activeSelf,
            source.bonusSprite.activeSelf
            );
        
        CopyColorFrom(source);
        
        transform.localScale = startLocalscale;
    }

    public void SetBlockVisual(bool top, bool bottom, bool left, bool right, bool overlay, bool bonus)
    {
        underlaySprite.SetActive(true);
        
        topSprite.SetActive(top);
        bottomSprite.SetActive(bottom);
        leftSprite.SetActive(left);
        rightSprite.SetActive(right);
        overlaySprite.SetActive(overlay);
        bonusSprite.SetActive(bonus);
    }

    public void ApplyPallete(ColorPallete pallete)
    {
        if (pallete == null) return;
        
        SetSpriteColor(underlaySprite,pallete.underlayColor);
        SetSpriteColor(topSprite,pallete.topColor);
        SetSpriteColor(bottomSprite,pallete.bottomColor);
        SetSpriteColor(leftSprite,pallete.leftColor);
        SetSpriteColor(rightSprite,pallete.rightColor);
        SetSpriteColor(overlaySprite,pallete.overlayColor);
        SetSpriteColor(bonusSprite,pallete.bonusColor);
        
    }

    private void SetSpriteColor(GameObject obj, Color color)
    {
        if (obj == null) return;
        
        SpriteRenderer sr = obj.GetComponent<SpriteRenderer>();
        
        if(sr != null) sr.color = color;
    }

    public void CopyColorFrom(ShapeSquare source)
    {
        CopySpriteColor(source.underlaySprite,underlaySprite);
        CopySpriteColor(source.topSprite,topSprite);
        CopySpriteColor(source.bottomSprite,bottomSprite);
        CopySpriteColor(source.leftSprite,leftSprite);
        CopySpriteColor(source.rightSprite,rightSprite);
        CopySpriteColor(source.overlaySprite,overlaySprite);
        CopySpriteColor(source.bonusSprite,bonusSprite);
    }

    private void CopySpriteColor(GameObject from, GameObject to)
    {
        SpriteRenderer fromSr = from.GetComponent<SpriteRenderer>();
        SpriteRenderer toSr = to.GetComponent<SpriteRenderer>();
        
        toSr.color = fromSr.color;
    }

    public void AddToOrderInLayer(int offset)
    {
        AddOrderToSprite(underlaySprite,offset);
        AddOrderToSprite(topSprite,offset);
        AddOrderToSprite(bottomSprite,offset);
        AddOrderToSprite(leftSprite,offset);
        AddOrderToSprite(rightSprite,offset);
        AddOrderToSprite(overlaySprite,offset);
        AddOrderToSprite(bonusSprite,offset);
    }

    private void AddOrderToSprite(GameObject obj, int offset)
    {
        if (obj == null) return;
        
        SpriteRenderer sr = obj.GetComponent<SpriteRenderer>();

        if (sr != null)
        {
            sr.sortingOrder += offset;
        }
    }

    public void ResetToEmpty()
    {
        IsOccupied = false;
        underlaySprite.SetActive(true);
        SetBlockVisual(false,false,false,false,false,false);
        transform.localScale = startLocalscale;
        
        SetSpriteColor(underlaySprite,new Color(0.1927732f,0.3675f,0.7169812f,1f));
        SetSpriteColor(topSprite,Color.white);
        SetSpriteColor(bottomSprite,Color.white);
        SetSpriteColor(leftSprite,Color.white);
        SetSpriteColor(rightSprite,Color.white);
        SetSpriteColor(overlaySprite,Color.white);
        SetSpriteColor(bonusSprite,Color.white);
    }

    public IEnumerator ClearAnimation()
    {
        Vector3 startScale = transform.localScale;

        float duration = 0.15f;
        float time = 0f;

        while (time < duration)
        {
            time += Time.deltaTime;
            
            float t = time / duration;
            
            transform.localScale = Vector3.Lerp(startScale, Vector3.zero, t);
            yield return null;
        }
        
        transform.localScale = Vector3.zero;
    }
    
}
