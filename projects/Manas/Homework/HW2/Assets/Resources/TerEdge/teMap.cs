using UnityEngine;
using UnityEditor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace TerEdge
{
    public static class teMap
    {	
		/*public enum SaveFormat { Triangles, Quads }
		public enum SaveResolution { Full=0, Half, Quarter, Eighth, Sixteenth }
		public static SaveFormat saveFormat = SaveFormat.Triangles;
	    public static SaveResolution saveResolution = SaveResolution.Half;*/
		
		public static void ImportHeights(GameObject go, string filepath,int mapType, int fileFormat){
		// fileFormat: 0=Raw(win) 1=Raw(mac) 2=PNG 3=Obj
			if(!go){Debug.Log("No terrain selected to import to.");return;}
			if(!go.GetComponent<Terrain>()){Debug.Log("No terrain selected to import to.");return;}
			TerrainData terdata = go.GetComponent<Terrain>().terrainData;
			if(mapType==0){ // Heightmap ------------------------------------------------------------
				if(fileFormat==0){
					if(filepath.Length<1){
						filepath = EditorUtility.OpenFilePanel("Import RAW (Win)", "", "raw");
					}
					if(filepath.Length>1){
						byte[] bytes = File.ReadAllBytes(filepath);
						Undo.RegisterUndo(terdata,"Import Heightmap");
						terdata.SetHeights(0,0,TerEdge.teFunc.bytesToDimFloat(bytes,false));
					}
				} else if (fileFormat==1){
					if(filepath.Length<1){
						filepath = EditorUtility.OpenFilePanel("Import RAW (Mac)", "", "raw");
					}
					if(filepath.Length>1){
						byte[] bytes = File.ReadAllBytes(filepath);
						Undo.RegisterUndo(terdata,"Import Heightmap");
						terdata.SetHeights(0,0,TerEdge.teFunc.bytesToDimFloat(bytes,true));
					}
				}
			}
		}
		
		public static Texture2D loadSplatmap(){
			Texture2D tmpTex = new Texture2D(16,16);
			string filepath = EditorUtility.OpenFilePanel("Import Splatmap", "", "");
			File.Copy(filepath,Application.dataPath + "/teImported.png",true);
			AssetDatabase.Refresh();
			TextureImporter textureImporter = AssetImporter.GetAtPath("Assets/teImported.png") as TextureImporter;
		    textureImporter.isReadable = true;
			textureImporter.mipmapEnabled = false;
			textureImporter.textureFormat = TextureImporterFormat.RGB24;
			AssetDatabase.ImportAsset("Assets/teImported.png");
			
			//myimp isReadable = true;

			tmpTex = (Texture2D) AssetDatabase.LoadAssetAtPath("Assets/teImported.png",typeof(Texture2D));
			return tmpTex;
		}	

		public static void AssignImportedChannels(Texture2D controlTex,int[] layerIndexes){
			// Assign the RGBA channels of controlTex to the alphamap layers indicated by layerIndexes.
			// NOTE: an index value of zero indicates the channel is unassigned. All other index 
			//       values should be reduced by one to get the actual splat prototype ID.       
			TerrainData td = Selection.activeGameObject.GetComponent<Terrain>().terrainData;
			float xmult = (float)controlTex.width*(1.0f/(float)td.alphamapResolution);
			float zmult = (float)controlTex.height*(1.0f/(float)td.alphamapResolution);
			float[,,] alpha = td.GetAlphamaps(0,0,td.alphamapResolution,td.alphamapResolution);
			float newval = 0f;
			for(int i=0;i<layerIndexes.Length;i++){
				int thisLayer = layerIndexes[i];
				if(thisLayer!=0){
					thisLayer = thisLayer - 1;
					for(int z=0;z<td.alphamapResolution;z++){	
						for(int x=0;x<td.alphamapResolution;x++){
							Color controlColor = controlTex.GetPixel((int)((float)x*xmult),(int)((float)z*zmult));
							if(i==1){newval = (float)controlColor.r;}
							if(i==2){newval = (float)controlColor.g;}
							if(i==3){newval = (float)controlColor.b;}
							if(i==0){newval = (float)controlColor.a;}
							//if(i==0){newval = 1.0f - ((float)controlColor.r+(float)controlColor.g+(float)controlColor.b);}
							float otherLayerTotal = 0f;
							for(int lay=0;lay<td.splatPrototypes.Length;lay++){
								if(lay!=thisLayer){
									otherLayerTotal+=alpha[x,z,lay];	
								}
							}
							
							for(int lay=0;lay<td.splatPrototypes.Length;lay++){
								if(thisLayer==lay){
									alpha[x,z,lay]=newval;
								} else {
									alpha[x,z,lay]=(1f-newval) * (alpha[x,z,lay] / otherLayerTotal);
								}
							}
						}
					}
				}
			}
			td.SetAlphamaps(0,0,alpha);
		}
		
		public static void ExportHeights(GameObject go, string filename, bool confirmname, int fileFormat){
			// formatid: 0=Raw(win) 1=Raw(mac) 2=PNG 3=OBJ
			string filepath = "";
			TerrainData terdata = go.GetComponent<Terrain>().terrainData;
			if(fileFormat==0){ // RAW win
				if(confirmname){ filepath = EditorUtility.SaveFilePanel("Export RAW (Win)", "", filename, "raw");}else{filepath = filename+".raw";}
				if(filepath.Length>1){
					File.WriteAllBytes(filepath, TerEdge.teFunc.dimFloatToBytes(terdata.GetHeights(0,0,terdata.heightmapResolution,terdata.heightmapResolution),false));
				}
			} else if (fileFormat==1){ // RAW mac
				if(confirmname){ filepath = EditorUtility.SaveFilePanel("Export RAW (Mac)", "", filename, "raw");}else{filepath = filename+".raw";}
				if(filepath.Length>1){
					File.WriteAllBytes(filepath, TerEdge.teFunc.dimFloatToBytes(terdata.GetHeights(0,0,terdata.heightmapResolution,terdata.heightmapResolution),false));
				}	
			} else if (fileFormat==2){ // PNG
				if(confirmname){ filepath = EditorUtility.SaveFilePanel("Export PNG", "", terdata.name+"_heightmap", "png");}else{filepath = filename+".png";}
				if(filepath.Length>1){Texture2D imgOut = ToTexture(go,0);SavePNG(filepath,imgOut);}	
			} else if (fileFormat==3){ // OBJ
				EditorUtility.DisplayDialog("OBJ Export","OBJ Export is currently not implemented.\n\n(It will be soon!)","OK");	
			}			
		}

		public static void ExportSplat(GameObject go, string filename, bool confirmname, int fileFormat){
			// formatid: 0=Raw(win) 1=Raw(mac) 2=PNG 3=OBJ
			string filepath = "";
			TerrainData terdata = go.GetComponent<Terrain>().terrainData;
			if (fileFormat==2){ // PNG
				if(confirmname){ filepath = EditorUtility.SaveFilePanel("Export PNG", "", terdata.name+"_splat", "png");}else{filepath = filename+".png";}
				if(filepath.Length>1){Texture2D imgOut = ToTexture(go,1);SavePNG(filepath,imgOut);}	
			}// else if (fileFormat==3){ EditorUtility.DisplayDialog("OBJ Export","OBJ Export is currently not implemented.\n\n(It will be soon!)","OK");}			
		}		
		
		public static void SavePNG(string fname,Texture2D imageOut){
			string filepath = EditorUtility.SaveFilePanel("Export PNG", "", fname, "png");
			if(filepath.Length>1){
				byte[] pngData = imageOut.EncodeToPNG();
				File.WriteAllBytes(filepath, pngData);
			}
		}		

/*		public static void ExportOBJ(){	
			TerrainData terrain = Selection.activeGameObject.GetComponent<Terrain>().terrainData;
			Vector3 terrainPos = Selection.activeGameObject.transform.position;
			string fileName = EditorUtility.SaveFilePanel("Export .obj file", "", "Terrain", "obj");
			int w = terrain.heightmapWidth;
			int h = terrain.heightmapHeight;
			Vector3 meshScale = terrain.size;
			int tRes = (int)Mathf.Pow(2, (int)saveResolution );
			meshScale = new Vector3(meshScale.x / (w - 1) * tRes, meshScale.y, meshScale.z / (h - 1) * tRes);
			Vector2 uvScale = new Vector2(1.0f / (w - 1), 1.0f / (h - 1));
			float[,] tData = terrain.GetHeights(0, 0, w, h);
			
			w = (w - 1) / tRes + 1;
			h = (h - 1) / tRes + 1;
			Vector3[] tVertices = new Vector3[w * h];
			Vector2[] tUV = new Vector2[w * h];
			
			int[] tPolys;
			
			if (saveFormat == SaveFormat.Triangles)
			{
			 tPolys = new int[(w - 1) * (h - 1) * 6];
			}
			else
			{
			 tPolys = new int[(w - 1) * (h - 1) * 4];
			}
			
			// Build vertices and UVs
			for (int y = 0; y < h; y++)
			{
			 for (int x = 0; x < w; x++)
			 {
			    tVertices[y * w + x] = Vector3.Scale(meshScale, new Vector3(x, tData[x * tRes, y * tRes], y)) + terrainPos;
			    tUV[y * w + x] = Vector2.Scale( new Vector2(x * tRes, y * tRes), uvScale);
			 }
			}
			
			int  index = 0;
			if (saveFormat == SaveFormat.Triangles)
			{
			 // Build triangle indices: 3 indices into vertex array for each triangle
			 for (int y = 0; y < h - 1; y++)
			 {
			    for (int x = 0; x < w - 1; x++)
			    {
			       // For each grid cell output two triangles
			       tPolys[index++] = (y * w) + x;
			       tPolys[index++] = ((y + 1) * w) + x;
			       tPolys[index++] = (y * w) + x + 1;
			
			       tPolys[index++] = ((y + 1) * w) + x;
			       tPolys[index++] = ((y + 1) * w) + x + 1;
			       tPolys[index++] = (y * w) + x + 1;
			    }
			 }
			}
			else
			{
			 // Build quad indices: 4 indices into vertex array for each quad
			 for (int y = 0; y < h - 1; y++)
			 {
			    for (int x = 0; x < w - 1; x++)
			    {
			       // For each grid cell output one quad
			       tPolys[index++] = (y * w) + x;
			       tPolys[index++] = ((y + 1) * w) + x;
			       tPolys[index++] = ((y + 1) * w) + x + 1;
			       tPolys[index++] = (y * w) + x + 1;
			    }
			 }
			}
			
			// Export to .obj
			StreamWriter sw = new StreamWriter(fileName);
			EditorUtility.DisplayProgressBar("Exporting","Exporting to OBJ file",0.5f);
			
			 sw.WriteLine("# Unity terrain OBJ File");
			
			 // Write vertices
			 System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");
			 for (int i = 0; i < tVertices.Length; i++)
			 {
			    StringBuilder sb = new StringBuilder("v ", 20);
			    // StringBuilder stuff is done this way because it's faster than using the "{0} {1} {2}"etc. format
			    // Which is important when you're exporting huge terrains.
			    sb.Append(tVertices[i].x.ToString()).Append(" ").
			       Append(tVertices[i].y.ToString()).Append(" ").
			       Append(tVertices[i].z.ToString());
			    sw.WriteLine(sb);
			 }
			 // Write UVs
			 for (int i = 0; i < tUV.Length; i++)
			 {
			    StringBuilder sb = new StringBuilder("vt ", 22);
			    sb.Append(tUV[i].x.ToString()).Append(" ").
			       Append(tUV[i].y.ToString());
			    sw.WriteLine(sb);
			 }
			 if (saveFormat == SaveFormat.Triangles)
			 {
			    // Write triangles
			    for (int i = 0; i < tPolys.Length; i += 3)
			    {
			       StringBuilder sb = new StringBuilder("f ", 43);
			       sb.Append(tPolys[i] + 1).Append("/").Append(tPolys[i] + 1).Append(" ").
			          Append(tPolys[i + 1] + 1).Append("/").Append(tPolys[i + 1] + 1).Append(" ").
			          Append(tPolys[i + 2] + 1).Append("/").Append(tPolys[i + 2] + 1);
			       sw.WriteLine(sb);
			    }
			 }
			 else
			 {
			    // Write quads
			    for (int i = 0; i < tPolys.Length; i += 4)
			    {
			       StringBuilder sb = new StringBuilder("f ", 57);
			       sb.Append(tPolys[i] + 1).Append("/").Append(tPolys[i] + 1).Append(" ").
			          Append(tPolys[i + 1] + 1).Append("/").Append(tPolys[i + 1] + 1).Append(" ").
			          Append(tPolys[i + 2] + 1).Append("/").Append(tPolys[i + 2] + 1).Append(" ").
			          Append(tPolys[i + 3] + 1).Append("/").Append(tPolys[i + 3] + 1);
			       sw.WriteLine(sb);
			    }
			 }
			sw.Close();
			terrain = null;
			EditorUtility.ClearProgressBar();
		}
		*/
		
		public static Texture2D ToTexture(GameObject go,int mapType,int layer = 0){
			TerrainData terdata = go.GetComponent<Terrain>().terrainData;
			int res = 0;
			Texture2D tmpTexture = new Texture2D(terdata.heightmapResolution,terdata.heightmapResolution);
			Color tmpColor = Color.black;
			switch(mapType){
			case 0: // -- Render Heightmap --
				float[,] tmpHeights = terdata.GetHeights(0,0,terdata.heightmapResolution,terdata.heightmapResolution);
				for(int hY=0;hY<terdata.heightmapResolution;hY++){
				    for(int hX=0;hX<terdata.heightmapResolution;hX++){
				    	tmpTexture.SetPixel(hX,hY,new Color(tmpHeights[hX,hY],tmpHeights[hX,hY],tmpHeights[hX,hY]));
					}
				}
				break;
			case 1: // -- Render Splatmap --
				res = terdata.alphamapResolution;
				tmpTexture = new Texture2D(res,res);
				float[,,] alphadata = terdata.GetAlphamaps(0,0,res,res);
				for(int y=0;y<res;y++){
					for(int x=0;x<res;x++){
						if(terdata.splatPrototypes.Length>1){tmpColor.r = alphadata[x,y,1];}
						if(terdata.splatPrototypes.Length>2){tmpColor.g = alphadata[x,y,2];}else{tmpColor.g = 0.0f;}
						if(terdata.splatPrototypes.Length>3){tmpColor.b = alphadata[x,y,3];}else{tmpColor.b = 0.0f;}
						tmpTexture.SetPixel(x,y,tmpColor);
					}
				}
				break;
			case 2: // -- Detail Layer --
				res = terdata.detailResolution;
				int[,] detaildata = terdata.GetDetailLayer(0,0,res,res,layer);
				tmpTexture = new Texture2D(res,res);
				for(int y=0;y<res;y++){
					for(int x=0;x<res;x++){
						tmpColor.r = (1.0f / 16) * detaildata[x,y];
						tmpTexture.SetPixel(x,y,tmpColor);
					}
				}
				break;
			case 3: // -- Tree Layer --
				res = 256;
				tmpTexture = new Texture2D(res,res);
				for(int y=0;y<res;y++){
					for(int x=0;x<res;x++){
						tmpTexture.SetPixel(x,y,Color.black);
					}
				}	
				for(int tI=0;tI<terdata.treeInstances.Length;tI++){
					TreeInstance treeInst = terdata.treeInstances[tI];
					if(treeInst.prototypeIndex==layer){
						tmpTexture.SetPixel((int)(treeInst.position.x*res),(int)(treeInst.position.z*res),Color.white);
					}
				}
				break;
			}
			tmpTexture.Apply();		
			return tmpTexture;				
		}
		
	}
}