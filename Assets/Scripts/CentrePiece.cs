using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CentrePiece : MonoBehaviour
{
    [Range(2, 256)]
    public int resolution = 10;
    public bool autoUpdate = true;

    public ShapeSettings shapeSettings;
    public ColourSettings colourSettings;

    ShapeGenerator shapeGenerator;

    [SerializeField, HideInInspector]
    MeshFilter[] meshFilters;
    PieceFace[] faces;

    [HideInInspector]
    public bool shapeSettingsShown;

    [HideInInspector]
    public bool colourSettingsShown;

    void Init()
    {
        shapeGenerator = new ShapeGenerator(shapeSettings);

        if (meshFilters == null || meshFilters.Length == 0)
        {
            meshFilters = new MeshFilter[6];
        }

        faces = new PieceFace[6];

        Vector3[] directions = {
            Vector3.up,
            Vector3.down,
            Vector3.left,
            Vector3.right,
            Vector3.forward,
            Vector3.back
        };

        for (int i = 0; i < 6; i++)
        {
            if (meshFilters[i] == null)
            {                
                var meshObj = new GameObject("mesh");
                meshObj.transform.parent = transform;

                meshObj.AddComponent<MeshRenderer>().sharedMaterial = new Material(Shader.Find("Standard"));
                meshFilters[i] = meshObj.AddComponent<MeshFilter>();
                meshFilters[i].sharedMesh = new Mesh();
            }

            faces[i] = new PieceFace(shapeGenerator, meshFilters[i].sharedMesh, resolution, directions[i]);
        }
    }

    public void GenerateSphere()
    {
        Init();
        GenerateMesh();
        GenerateColours();
    }

    public void OnShapeSettingsUpdated()
    {
        if (autoUpdate)
        {
            Init();
            GenerateMesh();
        }
    }

    public void OnColourSettingsUpdated()
    {
        if (autoUpdate)
        {
            Init();
            GenerateColours();
        }
    }

    void GenerateMesh()
    {
        foreach (var face in faces)
        {
            face.ConstructMesh();
        }
    }

    void GenerateColours()
    {
        foreach (var m in meshFilters)
        {
            var mat = m.GetComponent<MeshRenderer>().sharedMaterial;
            mat.color = colourSettings.colour;
        }
    }
}
