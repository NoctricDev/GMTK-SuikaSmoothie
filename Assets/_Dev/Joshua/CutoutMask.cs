using UnityEngine;
using UnityEngine.UI;

public class CutoutMask : Image
{
    private static readonly int stencilComp = Shader.PropertyToID("_StencilComp");

    public override Material materialForRendering
    {
        get
        {
            Material newMaterial = new(base.materialForRendering);
            newMaterial.SetFloat(stencilComp, (float)UnityEngine.Rendering.CompareFunction.NotEqual);
            return newMaterial;
        }
    }
}
