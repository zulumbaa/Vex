using UnityEngine;
using UnityEngine.UI;

public class PanicIconController : MonoBehaviour
{
    [SerializeField] private Image panicIconImage;
    [SerializeField] private Sprite[] panicSprites; 

    [Range(0, 100)]
    public float panicLevel;

    private void Update()
    {
        UpdatePanicIcon(panicLevel);
    }

    public void UpdatePanicIcon(float level)
    {
        int index = Mathf.Clamp(Mathf.FloorToInt(level / (100f / panicSprites.Length)), 0, panicSprites.Length - 1);
        panicIconImage.sprite = panicSprites[index];
    }
}
