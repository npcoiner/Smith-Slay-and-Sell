using System.Collections.Generic;
using Unity;
using UnityEngine;

public class MouldRecipeDict : MonoBehaviour
{
    [Header("Assign Recipes Here")]
    [SerializeField]
    private MouldRecipeMapping[] recipeMappings;

    private Dictionary<(MetalType, MouldType), GameObject> recipeDictionary;

    void Awake()
    {
        InitializeRecipes();
    }

    private void InitializeRecipes()
    {
        recipeDictionary = new Dictionary<(MetalType, MouldType), GameObject>();

        foreach (MouldRecipeMapping mapping in recipeMappings)
        {
            (MetalType metal, MouldType mould) recipeKey = (mapping.metalType, mapping.mouldType);
            var recipeValue = mapping.prefab;

            if (!recipeDictionary.ContainsKey(recipeKey))
            {
                recipeDictionary.Add(recipeKey, recipeValue);
            }
            else
            {
                Debug.LogWarning($"Duplicate recipe for workable + metal type: {recipeKey}");
            }
        }
    }

    public GameObject GetRecipeForMetalMouldType(MetalType metal, MouldType mould)
    {
        if (recipeDictionary.TryGetValue((metal, mould), out GameObject recipe))
        {
            return recipe;
        }
        else
        {
            Debug.LogError($"No recipe found for workable type: {(metal, mould)}");
            return null;
        }
    }
}
