using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleAnimator : MonoBehaviour
{
    [SerializeField]
    Sprite defaultSprite;
    [SerializeField]
    Anim[] anims;


    SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    public void PlayAnim(int index)
    {
        if (index < 0 || index >= anims.Length)
            return;
        StartCoroutine(PlayAnimCot(index));
    }

    private IEnumerator PlayAnimCot(int index)
    {
        for (int i = 0; i < anims[index].frames.Length; i++) 
        {
            spriteRenderer.sprite = anims[index].frames[i].sprite;
            yield return new WaitForSeconds(anims[index].frames[i].time);
        }
        spriteRenderer.sprite = defaultSprite;
    }

    public void TurnLeft()
    {
        spriteRenderer.flipX = true;
    }
    public void TurnRight()
    {
        spriteRenderer.flipX = false;
    }
}


[System.Serializable]
class Anim
{
    [SerializeField]
    public Frame[] frames;
}

[System.Serializable]
struct Frame
{
    public Sprite sprite;
    public float time;
}