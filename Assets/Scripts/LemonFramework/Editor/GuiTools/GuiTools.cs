using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class GuiTools
{
	/// <summary>
	///  开关列表
	/// </summary>
	public static bool BeginFoldOut(string text, bool foldOut, bool endSpace = true)
	{

		text = "<b><size=11>" + text + "</size></b>";
		if (foldOut)
		{
			text = "\u25BC " + text;
		}
		else
		{
			text = "\u25BA " + text;
		}

		if (!GUILayout.Toggle(true, text, "dragtab"))
		{
			foldOut = !foldOut;
		}

		if (!foldOut && endSpace) GUILayout.Space(5f);

		return foldOut;
	}

	/// <summary>
	/// 开始位置
	/// </summary>
	public static void BeginPosition(float padding = 0)
    {
		GUILayout.BeginHorizontal();
		GUILayout.Space(padding);
		GUILayout.BeginVertical();
		GUILayout.Space(2f);
	}

	/// <summary>
	/// 结束位置
	/// </summary>
	public static void EndPosition()
    {
		GUILayout.Space(3f);
		GUILayout.EndVertical();
		GUILayout.Space(3f);
		GUILayout.EndHorizontal();
	}


	/// <summary>
	///  开始区域
	/// </summary>
	public static Rect BeginGroup(float padding = 0)
	{
		GUILayout.BeginHorizontal();
		GUILayout.Space(padding);
		var rect = EditorGUILayout.BeginHorizontal("TextArea", GUILayout.MinHeight(10f));
		GUILayout.BeginVertical();
		GUILayout.Space(2f);
		return rect;
	}

	/// <summary>
	/// 结束区域
	/// </summary>
    public static void EndGroup(bool endSpace = true)
    {
        GUILayout.Space(3f);
        GUILayout.EndVertical();
        EditorGUILayout.EndHorizontal();
        GUILayout.Space(3f);
        GUILayout.EndHorizontal();

        if (endSpace)
        {
            GUILayout.Space(10f);
        }
    }

	public static GUIStyle AreaStyle()
    {
		GUIStyle style = new GUIStyle(GUI.skin.textArea);
		return style;
	}

	/// <summary>
	/// 绘制区域
	/// </summary>
	public static Rect BeginArea(Rect rect,GUIStyle style=null)
    {
		if (style == null) style = AreaStyle();
		GUILayout.BeginArea(rect, style);
		return rect;
	}

	/// <summary>
	/// 绘制区域
	/// </summary>
	public static void EndArea()
    {
		GUILayout.EndArea();
	}

	/// <summary>
	///  绘制线段
	/// </summary>
	public static void DrawSeparatorLine(float padding = 0)
	{
		EditorGUILayout.Space();
		DrawLine(Color.gray, padding);
		EditorGUILayout.Space();
	}

	/// <summary>
	/// 绘制线段
	/// </summary>
	private static void DrawLine(Color color, float padding = 0)
	{
		GUILayout.Space(10);
		Rect lastRect = GUILayoutUtility.GetLastRect();
		GUI.color = color;
		GUI.DrawTexture(new Rect(padding, lastRect.yMax - lastRect.height / 2f, Screen.width, 1f), EditorGUIUtility.whiteTexture);
		GUI.color = Color.white;
	}


	/// <summary>
	/// 创建按钮
	/// </summary>
	static public bool Button(string label, Color color, float width, bool leftAligment = false, float height = 0)
	{

		GUI.backgroundColor = color;
		GUIStyle buttonStyle = new GUIStyle("Button");

		if (leftAligment)
			buttonStyle.alignment = TextAnchor.MiddleLeft;

		if (height == 0)
		{
			if (GUILayout.Button(label, buttonStyle, GUILayout.Width(width)))
			{
				GUI.backgroundColor = Color.white;
				return true;
			}
		}
		else
		{
			if (GUILayout.Button(label, buttonStyle, GUILayout.Width(width), GUILayout.Height(height)))
			{
				GUI.backgroundColor = Color.white;
				return true;
			}
		}
		GUI.backgroundColor = Color.white;

		return false;
	}
}
