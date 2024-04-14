using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialSwitcher : MonoBehaviour
{
    [SerializeField] private MeshRenderer meshRenderer;
    [SerializeField] private List<Material> materials;

    private void Start()
    {
        if (materials.Count > 0 && meshRenderer != null)
            meshRenderer.SetMaterials(materials.GetRange(0, 1));
    }
        
    void SwitchMaterial(int index)
    {
        if (index > 0 && index < materials.Count && meshRenderer != null)
        {
            meshRenderer.SetMaterials(materials.GetRange(index, 1));
        }
    }
}
