using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class MainMenuScroll : MonoBehaviour
{
    [SerializeField] private Sprite[] backgrounds = null;

    [SerializeField] private SpriteRenderer leftBackground = null;
    [SerializeField] private SpriteRenderer rightBackground = null;

    [SerializeField] private float scrollSpeed = 1f;
    [SerializeField] private float scrollWidth = 32f;

    private float cyclePosition;

    private void OnValidate()
    {
        if (scrollWidth < 0.005f)
            scrollWidth = 0.005f;
        if (scrollSpeed < 0f)
            scrollSpeed = 0f;
    }


    public void RandomizeImage()
    {
        Sprite newSprite = backgrounds.RandomElement();
        leftBackground.sprite = newSprite;
        rightBackground.sprite = newSprite;
    }

    private void Awake()
    {
        cyclePosition = 0f;
        RandomizeImage();
    }

    private void Update()
    {
        cyclePosition += Time.deltaTime * scrollSpeed;
        cyclePosition %= 2 * scrollWidth;

        if (cyclePosition < scrollWidth)
        {
            leftBackground.transform.localPosition
                = Vector2.right * (cyclePosition - scrollWidth);
            rightBackground.transform.localPosition
                = Vector2.right * cyclePosition;
        }
        else
        {
            leftBackground.transform.localPosition
                = Vector2.right * (cyclePosition - scrollWidth);
            rightBackground.transform.localPosition
                = Vector2.right * (cyclePosition - 2 * scrollWidth);
        }
    }
}
