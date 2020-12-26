using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class CreditsScroll : MonoBehaviour
{
    [SerializeField] private Transform scrollTransform = null;
    [SerializeField] private FloatRange scrollRangeY = default;
    [SerializeField] private float scrollSpeed = 1f;

    float currentScroll;
    private bool isScrolling = false;

    private void OnValidate()
    {
        if (scrollSpeed < 0f)
            scrollSpeed = 0f;
        scrollRangeY.ClampIncreasing();
    }

    public void StartScrolling()
    {
        currentScroll = scrollRangeY.min;
        isScrolling = true;
    }
    public void StopScrolling()
    {
        isScrolling = false;
    }

    private void Update()
    {
        if (isScrolling)
        {
            currentScroll += scrollSpeed * Time.deltaTime;
            if (currentScroll > scrollRangeY.max)
                currentScroll -= scrollRangeY.Delta;
            scrollTransform.localPosition = new Vector3
            {
                x = scrollTransform.localPosition.x,
                y = currentScroll
            };
        }
    }
}
