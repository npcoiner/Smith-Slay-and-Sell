using System.Collections.Generic;
using Unity;
using UnityEngine;

public class FurnaceRecipeDict : MonoBehaviour
{
    [Header("Assign Recipes Here")]
    [SerializeField]
    private OreRecipeMapping[] recipeMappings;

    private Dictionary<OreType, GameObject> recipeDictionary;

    void Awake()
    {
        InitializeRecipes();
    }

    private void InitializeRecipes()
    {
        recipeDictionary = new Dictionary<OreType, GameObject>();

        foreach (OreRecipeMapping mapping in recipeMappings)
        {
            var recipeKey = mapping.oreType;
            var recipeValue = mapping.prefab;

            if (!recipeDictionary.ContainsKey(recipeKey))
            {
                recipeDictionary.Add(recipeKey, recipeValue);
            }
            else
            {
                Debug.LogWarning($"Duplicate recipe for ore type: {recipeKey}");
            }
        }
    }

    public GameObject GetRecipeForOreType(OreType oreType)
    {
        if (recipeDictionary.TryGetValue(oreType, out GameObject recipe))
        {
            return recipe;
        }
        else
        {
            Debug.LogError($"No recipe found for ore type: {oreType}");
            return null;
        }
    }
}
