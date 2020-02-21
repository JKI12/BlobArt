using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(CentrePiece))]
public class SphereEditor : Editor
{
    CentrePiece sphere;
    Editor shapeEditor;
    Editor colourEditor;

    private void OnEnable()
    {
        sphere = (CentrePiece) target;
    }

    public override void OnInspectorGUI()
    {
        using (var check = new EditorGUI.ChangeCheckScope())
        {       
            base.OnInspectorGUI();

            if (check.changed)
            {
                sphere.GenerateSphere();
            }
        }

        if (GUILayout.Button("Generate Sphere"))
        {
            sphere.GenerateSphere();
        }

        DrawSettingsEditor(sphere.shapeSettings, sphere.OnShapeSettingsUpdated, ref sphere.shapeSettingsShown, ref shapeEditor);
        DrawSettingsEditor(sphere.colourSettings, sphere.OnColourSettingsUpdated, ref sphere.colourSettingsShown, ref colourEditor);
    }

    void DrawSettingsEditor(Object settings, System.Action onSettingsUpdated, ref bool foldOut, ref Editor editor)
    {
        if (settings != null)
        {
            foldOut = EditorGUILayout.InspectorTitlebar(foldOut, settings);

        using (var check = new EditorGUI.ChangeCheckScope())
        {            
            if (foldOut)
            {
                CreateCachedEditor(settings, null, ref editor);
                editor.OnInspectorGUI();
                
                if (check.changed)
                {
                    if (onSettingsUpdated != null)
                    {
                        onSettingsUpdated();
                    }
                }
            }
        }
        }
    }
}
