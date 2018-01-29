using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(BezierSpline))]
public class BezierSplineInspector : Editor
{

    private const int stepsPerCurve = 10;
    private const float directionScale = 0.5f;
    private const float handleSize = 0.04f;
    private const float pickSize = 0.06f;

    private static Color[] modeColors = {
		Color.white,
		Color.yellow,
		Color.cyan
	};

    private BezierSpline spline;
    private Transform handleTransform;
    private Quaternion handleRotation;
    private static int selectedIndex = -1;

    public override void OnInspectorGUI()
    {
        //Debug.Log("OnInspectorGUI: " + GetInstanceID());

        spline = target as BezierSpline;

        if (selectedIndex >= 0 && selectedIndex < spline.ControlPointCount)
        {
            DrawSelectedPointInspector();
        }

        EditorGUI.BeginChangeCheck();
        bool createUniformSpline = EditorGUILayout.Toggle("Create Uniform Spline", spline.createUniformSpline);
        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(spline, "Create Uniform Spline");
            EditorUtility.SetDirty(spline);
            spline.createUniformSpline = createUniformSpline;
        }

        if (spline.createUniformSpline)
        {
            spline.numMetersPerUniformPoint = EditorGUILayout.Slider(new GUIContent(" Meters per Uniform Curve Point"), spline.numMetersPerUniformPoint, 0.1f, 10f);
            spline.uniformSplineSampleDistance = EditorGUILayout.Slider(new GUIContent(" Uniform spline sample distance"), spline.uniformSplineSampleDistance, 0.01f, 1f);
        }

        EditorGUI.BeginChangeCheck();
        bool loop = EditorGUILayout.Toggle("Loop", spline.Loop);
        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(spline, "Toggle Loop");
            EditorUtility.SetDirty(spline);
            spline.Loop = loop;
        }


        if (GUILayout.Button("Add Curve"))
        {
            Undo.RecordObject(spline, "Add Curve");
            spline.AddCurve();
            EditorUtility.SetDirty(spline);
        }

        EditorGUI.BeginChangeCheck();
        bool displayDebug = EditorGUILayout.Toggle("Debug", spline.displayWhenNotSelected);
        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(spline, "Toggle Debug");
            EditorUtility.SetDirty(spline);
            spline.displayWhenNotSelected = displayDebug;
        }

        if (spline.displayWhenNotSelected)
        {
            spline.splineSmoothness = EditorGUILayout.Slider(new GUIContent(" Spline smoothness"), spline.splineSmoothness, 0.01f, 0.99f);
            spline.splineColor = EditorGUILayout.ColorField(new GUIContent(" Spline color"), spline.splineColor);
            spline.pointSphereRadius = EditorGUILayout.Slider(new GUIContent(" Control sphere radius"), spline.pointSphereRadius, 0f, 1f);
            spline.pointSphereColor = EditorGUILayout.ColorField(new GUIContent(" Control sphere color"), spline.pointSphereColor);
        }
    }

    private void DrawSelectedPointInspector()
    {
        GUILayout.Label("Selected Point");
        EditorGUI.BeginChangeCheck();
        Vector3 point = EditorGUILayout.Vector3Field("Position", spline.GetControlPoint(selectedIndex));
        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(spline, "Move Point");
            EditorUtility.SetDirty(spline);
            spline.SetControlPoint(selectedIndex, point);
        }
        EditorGUI.BeginChangeCheck();
        BezierControlPointMode mode = (BezierControlPointMode)EditorGUILayout.EnumPopup("Mode", spline.GetControlPointMode(selectedIndex));
        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(spline, "Change Point Mode");
            spline.SetControlPointMode(selectedIndex, mode);
            EditorUtility.SetDirty(spline);
        }
    }

    private void OnSceneGUI()
    {
        //Debug.Log("OnSceneGUI: " + GetInstanceID());
        spline = target as BezierSpline;
        handleTransform = spline.transform;
        handleRotation = Tools.pivotRotation == PivotRotation.Local ?
            handleTransform.rotation : Quaternion.identity;

        Vector3 p0 = ShowPoint(0);
        for (int i = 1; i < spline.ControlPointCount; i += 3)
        {
            Vector3 p1 = ShowPoint(i);
            Vector3 p2 = ShowPoint(i + 1);
            Vector3 p3 = ShowPoint(i + 2);

            Handles.color = Color.gray;
            Handles.DrawLine(p0, p1);
            Handles.DrawLine(p2, p3);

            Handles.DrawBezier(p0, p3, p1, p2, Color.white, null, 2f);
            p0 = p3;
        }
        ShowDirections();
    }

    private void ShowDirections()
    {
        Handles.color = Color.green;
        Vector3 point = spline.GetPoint(0f);
        Handles.DrawLine(point, point + spline.GetDirection(0f) * directionScale);
        int steps = stepsPerCurve * spline.CurveCount;
        for (int i = 1; i <= steps; i++)
        {
            point = spline.GetPoint(i / (float)steps);
            Handles.DrawLine(point, point + spline.GetDirection(i / (float)steps) * directionScale);
        }
    }

    private Vector3 ShowPoint(int index)
    {
        //Debug.Log("Show point: " + index);
        Vector3 point = handleTransform.TransformPoint(spline.GetControlPoint(index));
        float size = HandleUtility.GetHandleSize(point);
        if (index == 0)
        {
            size *= 2f;
        }
        Handles.color = modeColors[(int)spline.GetControlPointMode(index)];

        if (Handles.Button(point, handleRotation, size * handleSize, size * pickSize, Handles.DotCap))
        {
            selectedIndex = index;
            Repaint();
        }
        if (selectedIndex == index)
        {
            //Debug.Log("Index: " + index);
            EditorGUI.BeginChangeCheck();
            point = Handles.DoPositionHandle(point, handleRotation);
            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(spline, "Move Point");
                EditorUtility.SetDirty(spline);
                spline.SetControlPoint(index, handleTransform.InverseTransformPoint(point));
            }
        }
        return point;
    }
}