using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
#endif
using EasyButtons;

public class CreatePreview : MonoBehaviour
{
#if UNITY_EDITOR
    public string assetBundleName;
    public int size = 128;
    public UnityEngine.Experimental.Rendering.DefaultFormat defaultFormat = UnityEngine.Experimental.Rendering.DefaultFormat.LDR;
    public UnityEngine.Experimental.Rendering.TextureCreationFlags textureCreationFlags = UnityEngine.Experimental.Rendering.TextureCreationFlags.None;

    void OnValidate()
    {
        if (!gameObject.activeInHierarchy) return;
        if (assetBundleName == null || assetBundleName == "") assetBundleName = this.gameObject.scene.name + "Preview";
    }

    [Button("Create level preview")]
    void CreateCubemap()
    {
        Cubemap cubemap = new Cubemap(size, defaultFormat, textureCreationFlags);
        Camera cam = this.gameObject.AddComponent<Camera>();
        cam.RenderToCubemap(cubemap);
        string cubemapPath = this.gameObject.scene.path.Replace(".unity", ".cubemap");
        AssetDatabase.CreateAsset(cubemap, cubemapPath);
        // AssetImporter.GetAtPath(cubemapPath).SetAssetBundleNameAndVariant(assetBundleName, "");
        UnityEngine.Rendering.Universal.UniversalAdditionalCameraData cameraData = cam.GetComponent<UnityEngine.Rendering.Universal.UniversalAdditionalCameraData>();
        if (cameraData) DestroyImmediate(cameraData);
        DestroyImmediate(cam);
        EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
        Debug.Log("Cubemap created in " + cubemapPath);
    }
#endif
}