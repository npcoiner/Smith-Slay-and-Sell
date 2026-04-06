using System.Collections.Generic;
using Unity;
using UnityEngine;

public class AnvilRecipeDict : MonoBehaviour
{
    [Header("Assign Recipes Here")]
    [SerializeField]
    private AnvilRecipeMapping[] recipeMappings;

    private Dictionary<WorkableType, GameObject> recipeDictionary;

    void Awake()
    {
        InitializeRecipes();
    }

    private void InitializeRecipes()
    {
        recipeDictionary = new Dictionary<WorkableType, GameObject>();

        foreach (AnvilRecipeMapping mapping in recipeMappings)
        {
            var recipeKey = mapping.workableType;
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

    public GameObject GetRecipeForWorkableType(WorkableType workableType)
    {
        if (recipeDictionary.TryGetValue(workableType, out GameObject recipe))
        {
            return recipe;
        }
        else
        {
            Debug.LogError($"No recipe found for workable type: {workableType}");
            return null;
        }
    }
}
