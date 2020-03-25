
using UnityEngine;

public class InfoSign : MonoBehaviour
{
    public Sprite normalSprite, lighSprite;

    SpriteRenderer render;

    public string tipContent;

    public void Start()
    {
        render = transform.GetComponent<SpriteRenderer>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            render.sprite = lighSprite;
            //显示一个提示
            TipMessagePanel._instance.ShowTip(tipContent, TipStyle.Style1);
        }
        
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            render.sprite = normalSprite;
            
            TipMessagePanel._instance.HideTip(TipStyle.Style1);
        }

    }
}


