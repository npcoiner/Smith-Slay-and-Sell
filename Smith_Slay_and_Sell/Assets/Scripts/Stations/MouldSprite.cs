using System.Collections.Generic;
using Unity;
using UnityEngine;

[System.Serializable]
public struct MouldSpriteMapping
{
    public MouldType mould;
    public Sprite sprite;
}

public class MouldSprite : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;

    [SerializeField]
    private MouldSpriteMapping[] spriteMappings;

    private Dictionary<MouldType, Sprite> mouldSprites;

    void Awake()
    {
        Initialization();
    }

    private void Initialization()
    {
        mouldSprites = new Dictionary<MouldType, Sprite>();
        foreach (MouldSpriteMapping mapping in spriteMappings)
        {
            MouldType key = mapping.mould;
            Sprite value = mapping.sprite;
            mouldSprites.Add(key, value);
        }
    }

    public Sprite GetSpriteForMouldType(MouldType mould)
    {
        if (mouldSprites.TryGetValue(mould, out Sprite sprite))
        {
            return sprite;
        }

        Debug.LogWarning($"No sprite found for mould type: {mould}");
        return null;
    }
}
