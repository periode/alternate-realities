using UnityEngine;
using UnityEditor;
using System.Collections;
using System.IO;
using System.Collections.Generic;
using System.Text;

namespace TerEdge
{
    public static class teUI
    {	
		public static void LabelBold(string labelText){
			GUILayout.Label (labelText,EditorStyles.miniBoldLabel);
		}
		
		public static int DropDown(string labelText,int selectedIndex, string[] strOptions){
			GUILayout.BeginHorizontal();
			GUILayout.Label(labelText,GUILayout.Width(80));
			selectedIndex = EditorGUILayout.Popup(selectedIndex,strOptions);
			GUILayout.EndHorizontal();
			return selectedIndex;
		}
		
		public static string FolderPicker(string labelText,string importFolder,string tipText){
			GUILayout.BeginHorizontal();
			GUILayout.Label(labelText,GUILayout.Width(80));
			importFolder = GUILayout.TextField(importFolder,GUILayout.MinWidth(80));
			if(GUILayout.Button("...",GUILayout.Width(30))){
				string folderPath = EditorUtility.OpenFolderPanel("Select folder to import from","","")	;
				if(folderPath.Length>1){
					importFolder = folderPath;	
				}
			}
			GUILayout.EndHorizontal();
			return importFolder;
		}
		
		public static string TextBox(string labelText,string strContent){
			GUILayout.BeginHorizontal();
			GUILayout.Label(labelText,GUILayout.Width(80));
			strContent = GUILayout.TextField(strContent);
			GUILayout.EndHorizontal();	
			return strContent;
		}		
		
		public static bool CheckBox(string labelText,bool boxChecked,string checkOptionName){
			GUILayout.BeginHorizontal();
			GUILayout.Label(labelText,GUILayout.Width(80));
			boxChecked = GUILayout.Toggle(boxChecked,checkOptionName);
			GUILayout.EndHorizontal();	
			return boxChecked;
		}
		
		public static bool MiniButton(string labelText){
			return(GUILayout.Button(labelText,EditorStyles.miniButton));	
		}
		
		public static float floatSlider(string label, float floatVar, float floatMin, float floatMax)
		{
			GUILayout.BeginHorizontal();
			GUILayout.Label (label, GUILayout.Width(80));
			floatVar = EditorGUILayout.Slider(floatVar,floatMin,floatMax);
			GUILayout.EndHorizontal();	
			return floatVar;
		}
			
		public static string colorPicker(string labelText, string colorString){
			GUILayout.BeginHorizontal();				
				GUILayout.Label (labelText, GUILayout.Width(80));
				colorString = EditorGUILayout.ColorField(teFunc.colorParse(colorString),GUILayout.MinWidth(40)).ToString();
			GUILayout.EndHorizontal();
			return colorString;
		}
		
		public static int intSlider(string labelText, int intVar, int intMin, int intMax){
			GUILayout.BeginHorizontal();
	        GUILayout.Label(labelText, GUILayout.Width(80));
	        intVar = EditorGUILayout.IntSlider(intVar,intMin,intMax);
			GUILayout.EndHorizontal();
			return intVar;
		}
		
	}
}