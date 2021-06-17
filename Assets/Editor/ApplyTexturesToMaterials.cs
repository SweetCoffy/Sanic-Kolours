using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Linq;

public class ApplyTexturesToMaterials : Editor
{
    [MenuItem("Tools/ApplyTexturesToMaterials")]
    static void ApplyTextures ()
    {
        try
        {
            AssetDatabase.StartAssetEditing();
            var materials = Selection.GetFiltered(typeof(Material), SelectionMode.Assets).Cast<Material>();
            foreach(var mat in materials)
            {
                if (AssetDatabase.LoadAssetAtPath("Assets/SRB2 MAP01/Textures/" + mat.name + ".png",typeof(Texture)) != null)
                {
                    Texture tex = (Texture)AssetDatabase.LoadAssetAtPath("Assets/SRB2 MAP01/Textures/" + mat.name + ".png",typeof(Texture));
                    mat.SetTexture("_MainTex", tex);
                    continue;
                }
            }
        } catch (System.Exception e) {
            Debug.LogError(e);
        }
        finally
        {
            AssetDatabase.StopAssetEditing();
            AssetDatabase.SaveAssets();
        }
    }
    [MenuItem("Tools/Search for no textures")]
    static void SearchForNoTextures() {
        var materials = Selection.GetFiltered(typeof(Material), SelectionMode.Assets).Cast<Material>();
        foreach (var mat in materials) {
            var tex = mat.GetTexture("_MainTex");
            if (tex == null) {
                Debug.Log($"Material {AssetDatabase.GetAssetPath(mat)} doesn't have a texture");
            }
        }
    }
}