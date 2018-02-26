using UnityEngine;
using UnityEditor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace TerEdge
{
    public static class teShaders
    {	
		public static void reset(bool firstPass, bool addPass){
			if(firstPass==true){AssetDatabase.DeleteAsset("terrainFirst.shader");}
			if(addPass==true){AssetDatabase.DeleteAsset("terrainAdd.shader");}
		}
		
		public static void setShaders(int shaderId){
			/*
			 * First                     Add
			 * -------------------------------------------------
			 * 0 = diffuse               4 = diffuse
			 * 1 = bump+spec             5 = bump+spec     
			 * 2 = allegorithmic clone   6 = (NOT YET IMPLEMENTED)
			 * 3 = triplanar bump+spec   7 = triplanar bump+spec
			*/
			string shaderFileName="terrainFirst.shader";
			if(shaderId>3){shaderFileName="terrainAdd.shader";}
			File.Delete(Application.dataPath+"/"+shaderFileName);
			if(shaderId!=0&&shaderId!=4){
				FileInfo srcfile = new FileInfo (Application.dataPath+"/shaderdata.txt");
				StreamReader srcstream = srcfile.OpenText();
				string textin = " ";
	            string textout = "";
				bool copydata = false;
				while(textin!=null){
					textin=srcstream.ReadLine();
					if(textin=="</SHADER"+shaderId.ToString()+">"){copydata=false;}
					if(copydata==true){
						textout=textout+textin+"\n";	
					}
					if(textin=="<SHADER"+shaderId.ToString()+">"){copydata=true;}	
				}
				File.WriteAllText(Application.dataPath+"/"+shaderFileName, textout);
				srcstream.Close();
			}
			AssetDatabase.Refresh();
		}
		
		
		public static void setTerrainShaderParams(GameObject basegameobject, Texture2D[] bumpmap, Texture2D[] specmap, float[] Spec, float[] MixUV, float MixScale){
			for(int i=0;i<8;i++){
				Shader.SetGlobalFloat("_MixScale", MixScale);
				if(i<bumpmap.Length){if(bumpmap[i]){Shader.SetGlobalTexture("_BumpMap"+i, bumpmap[i]);} else {Shader.SetGlobalTexture("_BumpMap"+i, generateBlankNormal());}} else {Shader.SetGlobalTexture("_BumpMap"+i, generateBlankNormal());}
				if(i<specmap.Length){if(specmap[i]){Shader.SetGlobalTexture("_SpecMap"+i, specmap[i]);} else {Shader.SetGlobalTexture("_SpecMap"+i, generateBlankSpec());}}else{Shader.SetGlobalTexture("_SpecMap"+i, generateBlankSpec());}	
				if(i<basegameobject.GetComponent<Terrain>().terrainData.splatPrototypes.Length){
					Shader.SetGlobalFloat("_Tile"+i, basegameobject.GetComponent<Terrain>().terrainData.splatPrototypes[i].tileSize.x);
					Shader.SetGlobalFloat("_Spec"+i, Spec[i]);
					Shader.SetGlobalFloat("_mixUVTile"+i, MixUV[i]);
					Shader.SetGlobalFloat("_TerrainTexScale"+i, 1f/ basegameobject.GetComponent<Terrain>().terrainData.splatPrototypes[i].tileSize.x );
				} else {
					Shader.SetGlobalFloat("_Tile"+i, 15f);
					Shader.SetGlobalFloat("_Spec"+i, 0f);
					Shader.SetGlobalFloat("_mixUVTile"+i, 1f);
					Shader.SetGlobalFloat("_TerrainTexScale"+i, 15f );
				}
			}
			Shader.SetGlobalFloat("_TerrainX", basegameobject.GetComponent<Terrain>().terrainData.size.x);
			Shader.SetGlobalFloat("_TerrainZ", basegameobject.GetComponent<Terrain>().terrainData.size.z);	
			SceneView.lastActiveSceneView.Repaint();
		}		
		
		
		public static Texture2D generateBlankNormal () {
			Texture2D texture = new Texture2D ( 16, 16, TextureFormat.ARGB32, false );
			Color32[] cols = texture.GetPixels32 ( 0 );
			int colsLength = cols.Length;
			for( int i=0 ; i < colsLength; i++ ) {
				cols[i] = new Color ( 0f, 0.5f, 0f, 0.5f );
			}
			texture.SetPixels32 ( cols, 0 );
			texture.Apply();
			texture.Compress ( false );
			return texture;
		}

		public static Texture2D generateBlankSpec () {
			Texture2D texture = new Texture2D ( 16, 16, TextureFormat.RGBA32, false );
			Color32[] cols = texture.GetPixels32 ( 0 );
			int colsLength = cols.Length;
			for( int i=0 ; i < colsLength; i++ ) {
				cols[i] = new Color ( 0.1f, 0.1f, 0f, 0f );
			}
			texture.SetPixels32 ( cols, 0 );
			texture.Apply();
			texture.Compress ( false );
			return texture;
		}
		
		
	}
}
