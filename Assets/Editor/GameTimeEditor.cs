using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(GameTime))]
public class GameTimeEditor : Editor
{
	public override void OnInspectorGUI()
	{
		DrawDefaultInspector();

		GameTime myScript = (GameTime)target;

		EditorGUILayout.BeginHorizontal ();

		if(GUILayout.Button("Jump -1 min",GUILayout.Width(100) ))
		{
			myScript.JumpForward (-60);
		}

		if(GUILayout.Button("Jump +1 min",GUILayout.Width(100) ))
		{
			myScript.JumpForward (60);

		}
	

		EditorGUILayout.EndHorizontal ();
		EditorGUILayout.BeginHorizontal ();


		if(GUILayout.Button("Jump -1 hour",GUILayout.Width(100) ))
		{
			myScript.JumpForward (-3600);
		}

		if(GUILayout.Button("Jump +1 hour",GUILayout.Width(100) ))
		{
			myScript.JumpForward (3600);
		}
			

		EditorGUILayout.EndHorizontal ();
		EditorGUILayout.BeginHorizontal ();

		if(GUILayout.Button("Jump -1 day",GUILayout.Width(100) ))
		{
			myScript.JumpForward (-86400);
		}

		if(GUILayout.Button("Jump +1 day",GUILayout.Width(100) ))
		{
			myScript.JumpForward (86400);
		}
			
		EditorGUILayout.EndHorizontal ();
		EditorGUILayout.BeginHorizontal ();

		if(GUILayout.Button("Jump -1 week",GUILayout.Width(100) ))
		{
			myScript.JumpForward (-86400*7);
		}

		if(GUILayout.Button("Jump +1 week",GUILayout.Width(100) ))
		{
			myScript.JumpForward (86400*7);
		}

		EditorGUILayout.EndHorizontal ();
		EditorGUILayout.BeginHorizontal ();

		if(GUILayout.Button("Jump -30 d",GUILayout.Width(100) ))
		{
			myScript.JumpForward (-86400*30);
		}

		if(GUILayout.Button("Jump +30 d",GUILayout.Width(100) ))
		{
			myScript.JumpForward (86400*30);
		}

		EditorGUILayout.EndHorizontal ();
		EditorGUILayout.BeginHorizontal ();

		if(GUILayout.Button("Jump to realtime",GUILayout.Width(100) ))
		{
			myScript.JumpToRealtime();	
		}
			
		if(GUILayout.Button("Seek +1h >>",GUILayout.Width(200) ))
		{
			myScript.SpeedTo((myScript.time + 3600.0) , 4, 20);	
		}

		EditorGUILayout.EndHorizontal ();

	}
}