using System.Collections.Generic;
using Unity;
using UnityEngine;

public class MouldRecipeDict : MonoBehaviour
{
    [Header("Assign Recipes Here")]
    [SerializeField]
    private MouldRecipeMapping[] recipeMappings;

    private Dictionary<(OreType, MouldType), GameObject> recipeDictionary;

    void Awake()
    {
        InitializeRecipes();
    }

    private void InitializeRecipes()
    {
        recipeDictionary = new Dictionary<(OreType, MouldType), GameObject>();

        foreach (MouldRecipeMapping mapping in recipeMappings)
        {
            (OreType ore, MouldType mould) recipeKey = (mapping.oreType, mapping.mouldType);
            var recipeValue = mapping.prefab;

            if (!recipeDictionary.ContainsKey(recipeKey))
            {
                recipeDictionary.Add(recipeKey, recipeValue);
            }
            else
            {
                Debug.LogWarning($"Duplicate recipe for workable type: {recipeKey}");
            }
        }
    }

    public GameObject GetRecipeForMetalMouldType(OreType ore, MouldType mould)
    {
        if (recipeDictionary.TryGetValue((ore, mould), out GameObject recipe))
        {
            return recipe;
        }
        else
        {
            Debug.LogError($"No recipe found for workable type: {(ore, mould)}");
            return null;
        }
    }
}
