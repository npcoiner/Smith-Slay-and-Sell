using System.Collections.Generic;
using Unity;
using UnityEngine;

public class AnvilRecipeDict : MonoBehaviour
{
    [Header("Assign Recipes Here")]
    [SerializeField]
    private AnvilRecipeMapping[] recipeMappings;

    private Dictionary<(MetalType, WorkableType), GameObject> recipeDictionary;

    void Awake()
    {
        InitializeRecipes();
    }

    private void InitializeRecipes()
    {
        recipeDictionary = new Dictionary<(MetalType, WorkableType), GameObject>();

        foreach (AnvilRecipeMapping mapping in recipeMappings)
        {
            (MetalType metal, WorkableType workable) recipeKey = (
                mapping.metalType,
                mapping.workableType
            );
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

    public GameObject GetRecipeForWorkableType(MetalType metal, WorkableType workable)
    {
        if (recipeDictionary.TryGetValue((metal, workable), out GameObject recipe))
        {
            return recipe;
        }
        else
        {
            Debug.LogError($"No recipe found for workable type: {(workable, metal)}");
            return null;
        }
    }
}
