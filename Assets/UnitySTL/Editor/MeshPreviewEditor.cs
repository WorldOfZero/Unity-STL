using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using STLBuilder;
using UnityEditor;
using UnityEngine;
using Mesh = UnityEngine.Mesh;

[ExecuteInEditMode]
public class MeshPreviewEditor : EditorWindow
{
    private PreviewRenderUtility previewRenderer;

    public void Initialize()
    {
        previewRenderer = new PreviewRenderUtility();
        previewRenderer.cameraFieldOfView = 60;
        previewRenderer.camera.clearFlags = CameraClearFlags.Skybox;
        previewRenderer.camera.transform.position = new Vector3(0, 0, -5);
        previewRenderer.camera.farClipPlane = 1000;
        
        previewRenderer.lights[0].transform.rotation = FindDirectionalLights()[0].transform.rotation;
        previewRenderer.lights[0].intensity = 1;
        for (int i = 1; i < previewRenderer.lights.Length; ++i)
        {
            previewRenderer.lights[i].intensity = 0;
        }
    }

    private Light[] FindDirectionalLights()
    {
        return GameObject.FindObjectsOfType<Light>().Where(light => light.type == LightType.Directional).ToArray();
    }

    [MenuItem("Tools/Mesh Preview Editor")]
    static void InitializeWindow()
    {
        var window = GetWindow<MeshPreviewEditor>("Mesh Preview Editor", true);
        window.titleContent.tooltip = "Mesh Preview Editor";
        window.autoRepaintOnSceneChange = true;
        window.Show();
    }

    private void Update()
    {
        Repaint();
    }

    public void OnGUI()
    {
        if (Selection.activeGameObject == null)
        {
            EditorGUILayout.LabelField("No game object selected.");
            return; // Do nothing if no game object selected
        }
        
        var meshFilters = Selection.activeGameObject.GetComponentsInChildren<MeshFilter>();
        var skinnedMeshRenderers = Selection.activeGameObject.GetComponentsInChildren<SkinnedMeshRenderer>();
        if (meshFilters == null)
        {
            EditorGUILayout.LabelField("Game Object does not contain the required components.");
            return; // The necessary components aren't present. Skip.
        }

        if (previewRenderer == null)
        {
            Initialize();
        }

        previewRenderer.camera.transform.RotateAround(Vector3.zero, new Vector3(0.0f, 1.0f, 0.0f).normalized, Time.deltaTime);

        var boundaries = new Rect(0, 0, this.position.width, this.position.height);
        previewRenderer.BeginPreview(boundaries, GUIStyle.none);
        foreach (var filter in meshFilters)
        {
            var meshRenderer = filter.GetComponent<MeshRenderer>();
            if (meshRenderer)
            {
                DrawSelectedMesh(filter.sharedMesh, meshRenderer.sharedMaterial, filter.gameObject.transform);
            }
        }
        foreach (var skin in skinnedMeshRenderers)
        {
            Mesh mesh = new Mesh();
            skin.BakeMesh(mesh);
            DrawSelectedMesh(mesh, skin.sharedMaterial, skin.gameObject.transform);
        }
        previewRenderer.camera.Render();
        var render = previewRenderer.EndPreview();
        GUI.DrawTexture(new Rect(0, 0, boundaries.width, boundaries.height), render);

        previewRenderer.camera.transform.position = previewRenderer.camera.transform.position.normalized *
            EditorGUILayout.Slider(previewRenderer.camera.transform.position.magnitude, 1, 100);
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Selected: " + Selection.activeGameObject.name);
        if (GUILayout.Button("Export STL"))
        {
            var selectedMeshes = meshFilters.Select(filter => new MeshContainer()
            {
                Mesh = filter.sharedMesh,
                Translation = filter.transform.position - Selection.activeGameObject.transform.position
            });
            var skinnedMeshes = skinnedMeshRenderers.Select(filter =>
            {
                Mesh mesh = new Mesh();
                filter.BakeMesh(mesh);
                return new MeshContainer()
                {
                    Mesh = mesh,
                    Translation = filter.transform.position - Selection.activeGameObject.transform.position
                };
            });

            var meshExport = STLBuilder.Mesh.Build("UnityExport", selectedMeshes.Concat(skinnedMeshes).ToArray());
            var stlFile = StlBuilder.Build(meshExport);
            File.WriteAllText(Path.Combine(Directory.GetCurrentDirectory(), "exported-mesh.stl"), stlFile); //TODO: Change this
        }
        EditorGUILayout.EndHorizontal();
    }

    private void DrawSelectedMesh(Mesh mesh, Material material, Transform transform)
    {
        previewRenderer.DrawMesh(mesh, Matrix4x4.TRS(transform.position, transform.rotation, transform.localScale), material, 0);
    }
}
