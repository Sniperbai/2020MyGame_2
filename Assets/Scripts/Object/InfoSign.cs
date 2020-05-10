
using UnityEngine;

public class InfoSign : MonoBehaviour
{
    public Sprite normalSprite, lighSprite;

    SpriteRenderer render;

    public string tipContent;

    public AudioClip speekClip;   //要播放的音效

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
            TipMessagePanel.Instance.ShowTip(tipContent, TipStyle.Style1);

            //播放说话的音乐
            AudioManager.Instance.PlaySound(speekClip);
            
        }
        
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            render.sprite = normalSprite;
            
            TipMessagePanel.Instance.HideTip(TipStyle.Style1);
        }

    }
}


