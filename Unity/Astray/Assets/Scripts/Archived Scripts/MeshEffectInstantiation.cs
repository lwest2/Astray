using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshEffectInstantiation : MonoBehaviour
{
    public void ApplyMesh(bool isActive, Color color, GameObject meshObject, GameObject effectInstance)
    {
        PSMeshRendererUpdater psUpdater = effectInstance.GetComponent<PSMeshRendererUpdater>();
        psUpdater.IsActive = isActive;
        psUpdater.UpdateMeshEffect(meshObject);
    }
}
