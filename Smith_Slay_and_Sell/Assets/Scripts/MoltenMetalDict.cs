using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct MoltenMetalMapping
{
    public MetalType metalType;
    public Color color;

    [Range(0, 5)] // slider in the Inspector
    public float emissionIntensity;
}

public class MoltenMetalDict : MonoBehaviour
{
    [SerializeField]
    private MoltenMetalMapping[] visualMappings;

    private Dictionary<MetalType, MoltenMetalMapping> _metalDict;

    void Awake()
    {
        InitializeDictionary();
    }

    private void InitializeDictionary()
    {
        _metalDict = new Dictionary<MetalType, MoltenMetalMapping>();
        foreach (var mapping in visualMappings)
        {
            if (!_metalDict.ContainsKey(mapping.metalType))
            {
                _metalDict.Add(mapping.metalType, mapping);
            }
            else
            {
                Debug.LogWarning(
                    $"Duplicate MetalType found in MoltenMetalDict: {mapping.metalType}"
                );
            }
        }
    }

    public bool TryGetVisuals(MetalType type, out Color color, out float intensity)
    {
        if (_metalDict == null)
            InitializeDictionary();

        if (_metalDict.TryGetValue(type, out MoltenMetalMapping mapping))
        {
            color = mapping.color;
            intensity = mapping.emissionIntensity;
            return true;
        }
        // Default values
        color = Color.gray;
        intensity = 0f;
        return false;
    }
}
