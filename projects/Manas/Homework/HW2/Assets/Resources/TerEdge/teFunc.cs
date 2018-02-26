using UnityEngine;
using UnityEditor;
using LibNoise.Unity;
using LibNoise.Unity.Generator;
using LibNoise.Unity.Operator;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TerEdge
{
    public static class teFunc
    {	
	    public enum Orientation {Horizontal, Vertical}
		
		#region Methods

		public static void terrainPlane(string optName,float pWidth, float pLength,int widthSegments,int lengthSegments){
	        GameObject plane = new GameObject();
            plane.name = optName;
            plane.transform.position = Vector3.zero;
	        string meshPrefabPath = "Assets/Editor/" + plane.name + widthSegments + "x" + lengthSegments + "W" + pWidth + "L" + pLength + "H" + ".asset";
	        MeshFilter meshFilter = (MeshFilter)plane.AddComponent(typeof(MeshFilter));
	        plane.AddComponent(typeof(MeshRenderer));
	        Mesh m = (Mesh)AssetDatabase.LoadAssetAtPath(meshPrefabPath, typeof(Mesh));    
	        if (m == null)
	        {
	            m = new Mesh();
	            m.name = plane.name;
	       
	            int hCount2 = widthSegments+1;
	            int vCount2 = lengthSegments+1;
	            int numTriangles = widthSegments * lengthSegments * 6;
	            int numVertices = hCount2 * vCount2;
	       
	            Vector3[] vertices = new Vector3[numVertices];
	            Vector2[] uvs = new Vector2[numVertices];
	            int[] triangles = new int[numTriangles];
	       
	            int index = 0;
	            //float uvFactorX = 1.0f/widthSegments;
	            //float uvFactorY = 1.0f/lengthSegments;
	            float scaleX = pWidth/widthSegments;
	            float scaleY = pLength/lengthSegments;
	            for (float y = 0.0f; y < vCount2; y++)
	            {
	                for (float x = 0.0f; x < hCount2; x++)
	                {
	                    //if (orientation == Orientation.Horizontal)
	                        vertices[index] = new Vector3(x*scaleX, 0.0f, y*scaleY);
	                   // else
	                       // vertices[index] = new Vector3(x*scaleX, y*scaleY, 0.0f);
	                    //uvs[index++] = new Vector2(x*uvFactorX, y*uvFactorY);
	                }
	            }
	           
	            index = 0;
	            for (int y = 0; y < lengthSegments; y++)
	            {
	                for (int x = 0; x < widthSegments; x++)
	                {
	                    triangles[index]   = (y     * hCount2) + x;
	                    triangles[index+1] = ((y+1) * hCount2) + x;
	                    triangles[index+2] = (y     * hCount2) + x + 1;
	       
	                    triangles[index+3] = ((y+1) * hCount2) + x;
	                    triangles[index+4] = ((y+1) * hCount2) + x + 1;
	                    triangles[index+5] = (y     * hCount2) + x + 1;
	                    index += 6;
	                }
	            }
	       
	            m.vertices = vertices;
	            m.uv = uvs;
	            m.triangles = triangles;
	            m.RecalculateNormals();
	            
	            AssetDatabase.CreateAsset(m, meshPrefabPath);
	            //AssetDatabase.SaveAssets();
	        }
	       
	        meshFilter.sharedMesh = m;
			
	        m.RecalculateBounds();
			//MeshCollider meshc = plane.AddComponent(typeof(MeshCollider)) as MeshCollider;
			//meshc.sharedMesh = m.;
			
			Selection.activeObject = plane;			
		}
		
		
		public static Object[] FindObjectsOfType(System.Type type) { // wrapper method cause alot of the existing code used it
		    return Object.FindObjectsOfType(type);
		}		
		
		public static float clampVal(float valToClamp){ //make sure value is between 0 and 1
			if(valToClamp<0f){return 0f;}else if(valToClamp>1f){return 1f;}else{return valToClamp;}
		}	

		public static Color colorParse(string colorText){
			if(colorText.IndexOf("RGBA")<0){return Color.white;}
			string colorTextStripped = colorText.Replace("RGBA(","").Replace(")","").Replace(" ","");
			string[] colorTextSegs = colorTextStripped.Split(",".ToCharArray());
			return new Color(float.Parse(colorTextSegs[0]),float.Parse(colorTextSegs[1]),float.Parse(colorTextSegs[2]),float.Parse(colorTextSegs[3]));
		}
	
		public static float falloffMult(int calcX,int centerX,int calcZ,int centerZ,int maxVal){
			int offX = calcX - centerX;
			int offZ = calcZ - centerZ;
			if(offX<0){offX=0-offX;}
			if(offZ<0){offZ=0-offZ;}
			if(offX+offZ==0){return 1f;}
			return 1f/(float)(offX+offZ);
		}		

		public static void copyProtos(GameObject goFrom, GameObject goTo){
			goTo.GetComponent<Terrain>().terrainData.splatPrototypes = goFrom.GetComponent<Terrain>().terrainData.splatPrototypes;
			goTo.GetComponent<Terrain>().terrainData.detailPrototypes = goFrom.GetComponent<Terrain>().terrainData.detailPrototypes;
			goTo.GetComponent<Terrain>().terrainData.treePrototypes = goFrom.GetComponent<Terrain>().terrainData.treePrototypes;
			goTo.GetComponent<Terrain>().terrainData.RefreshPrototypes();
		}

		public static void combineSelected(){
			// Only terrains
			for(int i=0;i<4;i++){
				if(Selection.gameObjects[i].GetComponent<Terrain>()==null){
					EditorUtility.DisplayDialog("Selection Error","Selection must contain terrain objects ONLY.","Ok");	
					return;	
				}
			}
			Vector3 topLeft = Selection.gameObjects[0].transform.position;
			Vector3 topRight = Selection.gameObjects[0].transform.position;
			Vector3 bottomLeft = Selection.gameObjects[0].transform.position;
			Vector3 bottomRight = Selection.gameObjects[0].transform.position;
			TerrainData goTL = Selection.gameObjects[0].GetComponent<Terrain>().terrainData;
			TerrainData goTR = Selection.gameObjects[0].GetComponent<Terrain>().terrainData;
			TerrainData goBL = Selection.gameObjects[0].GetComponent<Terrain>().terrainData;
			TerrainData goBR = Selection.gameObjects[0].GetComponent<Terrain>().terrainData;
			float terSize = Selection.gameObjects[0].GetComponent<Terrain>().terrainData.size.x;
			string alignErrors = "";
			// Set positions
			for(int i=0;i<4;i++){
				if(Selection.gameObjects[i].transform){
					if(Selection.gameObjects[i].transform.position.x<=topLeft.x&&Selection.gameObjects[i].transform.position.z<=topLeft.z){topLeft = Selection.gameObjects[i].transform.position;goTL=Selection.gameObjects[i].GetComponent<Terrain>().terrainData;}
					if(Selection.gameObjects[i].transform.position.x>=topRight.x&&Selection.gameObjects[i].transform.position.z<=topRight.z){topRight = Selection.gameObjects[i].transform.position;goTR=Selection.gameObjects[i].GetComponent<Terrain>().terrainData;}
					if(Selection.gameObjects[i].transform.position.x<=bottomLeft.x&&Selection.gameObjects[i].transform.position.z>=bottomLeft.z){bottomLeft = Selection.gameObjects[i].transform.position;goBL=Selection.gameObjects[i].GetComponent<Terrain>().terrainData;}
					if(Selection.gameObjects[i].transform.position.x>=bottomRight.x&&Selection.gameObjects[i].transform.position.z>=bottomRight.z){bottomRight = Selection.gameObjects[i].transform.position;goBR=Selection.gameObjects[i].GetComponent<Terrain>().terrainData;}
				}
			}
			// Check positions
			if(topLeft.x!=topRight.x-terSize||bottomLeft.x!=bottomRight.x-terSize){alignErrors+="\nLeft-Right tiles not one terrain width apart.";}
			if(topLeft.z!=bottomLeft.z-terSize){alignErrors+="\nTop-Bottom (Left) tiles not one terrain width apart.";}
			if(topRight.z!=bottomRight.z-terSize){alignErrors+="\nTop-Bottom (Right) tiles not one terrain width apart.";}
			if(topLeft.x!=bottomLeft.x||topLeft.z!=topRight.z){alignErrors+="\nTop-Left does not match x of Bottom-Left or y of Top-Right.";}
			if(alignErrors!=""){
				EditorUtility.DisplayDialog("Selection Error","You must pick 4 terrains adjacent to each other in a square.\n\n(eg: x,z - x+1,z+1)","Ok");	
				return;
			}
			// Create Terrain & Set Position
			GameObject newGob = newTerrain("Combined",0,0,(float)goTL.size.x*2f,goTL.size.y,1+((goTL.heightmapResolution-1)*2),goTL.alphamapResolution*2, goTL.baseMapResolution*2, goTL.detailResolution*2, 8);
			newGob.transform.position = topLeft;
			// Apply and refresh prototypes
			newGob.GetComponent<Terrain>().terrainData.splatPrototypes = goTL.splatPrototypes;
			newGob.GetComponent<Terrain>().terrainData.detailPrototypes = goTL.detailPrototypes;
			newGob.GetComponent<Terrain>().terrainData.treePrototypes = goTL.treePrototypes;
			newGob.GetComponent<Terrain>().terrainData.RefreshPrototypes();
			newGob.GetComponent<Terrain>().Flush();
			// Apply alphamaps from all 4 terrains
			newGob.GetComponent<Terrain>().terrainData.SetAlphamaps(0,0,goTL.GetAlphamaps(0,0,goTL.alphamapResolution,goTL.alphamapResolution));
			newGob.GetComponent<Terrain>().terrainData.SetAlphamaps(goTL.alphamapResolution,0,goTR.GetAlphamaps(0,0,goTL.alphamapResolution,goTL.alphamapResolution));
			newGob.GetComponent<Terrain>().terrainData.SetAlphamaps(0,goTL.alphamapResolution,goBL.GetAlphamaps(0,0,goTL.alphamapResolution,goTL.alphamapResolution));
			newGob.GetComponent<Terrain>().terrainData.SetAlphamaps(goTL.alphamapResolution,goTL.alphamapResolution,goBR.GetAlphamaps(0,0,goTL.alphamapResolution,goTL.alphamapResolution));
			// Apply heightmaps from all 4 terrains
			newGob.GetComponent<Terrain>().terrainData.SetHeights(0,0,goTL.GetHeights(0,0,goTL.heightmapResolution,goTL.heightmapResolution));
			newGob.GetComponent<Terrain>().terrainData.SetHeights(goTL.heightmapResolution-1,0,goTR.GetHeights(0,0,goTL.heightmapResolution,goTL.heightmapResolution));
			newGob.GetComponent<Terrain>().terrainData.SetHeights(0,goTL.heightmapResolution-1,goBL.GetHeights(0,0,goTL.heightmapResolution,goTL.heightmapResolution));
			newGob.GetComponent<Terrain>().terrainData.SetHeights(goTL.heightmapResolution-1,goTL.heightmapResolution-1,goBR.GetHeights(0,0,goTL.heightmapResolution,goTL.heightmapResolution));
			// Apply detail layers from all 4 terrains
			int detailLayers = newGob.GetComponent<Terrain>().terrainData.detailPrototypes.Length;
			for(int i=0;i<detailLayers;i++){
				newGob.GetComponent<Terrain>().terrainData.SetDetailLayer(0,0,i,goTL.GetDetailLayer(0,0,goTL.detailResolution,goTL.detailResolution,i));
				newGob.GetComponent<Terrain>().terrainData.SetDetailLayer(goTL.detailResolution,0,i,goTR.GetDetailLayer(0,0,goTL.detailResolution,goTL.detailResolution,i));
				newGob.GetComponent<Terrain>().terrainData.SetDetailLayer(0,goTL.detailResolution,i,goBL.GetDetailLayer(0,0,goTL.detailResolution,goTL.detailResolution,i));
				newGob.GetComponent<Terrain>().terrainData.SetDetailLayer(goTL.detailResolution,goTL.detailResolution,i,goBR.GetDetailLayer(0,0,goTL.detailResolution,goTL.detailResolution,i));			
			}
			// Apply tree instances from all 4 terrains
			for(int i=0;i<goTL.treeInstances.Length;i++){
				TreeInstance tmpTreeInstance  = new TreeInstance();
				tmpTreeInstance = goTL.treeInstances[i];
				tmpTreeInstance.position = new Vector3(tmpTreeInstance.position.x/2f,tmpTreeInstance.position.y,tmpTreeInstance.position.z/2f);
				newGob.GetComponent<Terrain>().AddTreeInstance(tmpTreeInstance);
			}
			// Apply tree instances from all 4 terrains
			for(int i=0;i<goTR.treeInstances.Length;i++){
				TreeInstance tmpTreeInstance  = new TreeInstance();
				tmpTreeInstance = goTR.treeInstances[i];
				tmpTreeInstance.position = new Vector3(0.5f+(tmpTreeInstance.position.x/2f),tmpTreeInstance.position.y,tmpTreeInstance.position.z/2f);
				newGob.GetComponent<Terrain>().AddTreeInstance(tmpTreeInstance);
			}
			// Apply tree instances from all 4 terrains
			for(int i=0;i<goBL.treeInstances.Length;i++){
				TreeInstance tmpTreeInstance  = new TreeInstance();
				tmpTreeInstance = goBL.treeInstances[i];
				tmpTreeInstance.position = new Vector3(tmpTreeInstance.position.x/2f,tmpTreeInstance.position.y,0.5f+(tmpTreeInstance.position.z/2f));
				newGob.GetComponent<Terrain>().AddTreeInstance(tmpTreeInstance);
			}
			// Apply tree instances from all 4 terrains
			for(int i=0;i<goBR.treeInstances.Length;i++){
				TreeInstance tmpTreeInstance  = new TreeInstance();
				tmpTreeInstance = goBR.treeInstances[i];
				tmpTreeInstance.position = new Vector3(0.5f+(tmpTreeInstance.position.x/2f),tmpTreeInstance.position.y,0.5f+(tmpTreeInstance.position.z/2f));
				newGob.GetComponent<Terrain>().AddTreeInstance(tmpTreeInstance);
			}
		}
		
		public static void splitSelected(){
			GameObject tGo = Selection.activeGameObject;
			TerrainData tDat = Selection.activeGameObject.GetComponent<Terrain>().terrainData;
			Vector3 tPos = Selection.activeGameObject.transform.position;
			int aRes = tDat.alphamapResolution/2;
			int hRes = ((tDat.heightmapResolution-1)/2)+1;
			int dRes = tDat.detailResolution/2;
			// Create Terrain & Set Position
			GameObject topLeft = newTerrain(Selection.activeGameObject.name+"_split0",0,0,(float)tDat.size.x/2f,tDat.size.y,((tDat.heightmapResolution-1)/2)+1,tDat.alphamapResolution/2, tDat.baseMapResolution/2, tDat.detailResolution/2, 8);
			GameObject topRight = newTerrain(Selection.activeGameObject.name+"_split1",0,0,(float)tDat.size.x/2f,tDat.size.y,((tDat.heightmapResolution-1)/2)+1,tDat.alphamapResolution/2, tDat.baseMapResolution/2, tDat.detailResolution/2, 8);
			GameObject bottomLeft = newTerrain(Selection.activeGameObject.name+"_split2",0,0,(float)tDat.size.x/2f,tDat.size.y,((tDat.heightmapResolution-1)/2)+1,tDat.alphamapResolution/2, tDat.baseMapResolution/2, tDat.detailResolution/2, 8);
			GameObject bottomRight = newTerrain(Selection.activeGameObject.name+"_split3",0,0,(float)tDat.size.x/2f,tDat.size.y,((tDat.heightmapResolution-1)/2)+1,tDat.alphamapResolution/2, tDat.baseMapResolution/2, tDat.detailResolution/2, 8);
			topLeft.transform.position = tPos;
			topRight.transform.position = new Vector3(tPos.x+(tDat.size.x/2),tPos.y,tPos.z);
			bottomLeft.transform.position = new Vector3(tPos.x,tPos.y,tPos.z+(tDat.size.z/2));
			bottomRight.transform.position = new Vector3(tPos.x+(tDat.size.x/2),tPos.y,tPos.z+(tDat.size.z/2));
			teFunc.copyProtos(tGo,topLeft); teFunc.copyProtos(tGo,topRight); teFunc.copyProtos(tGo,bottomLeft); teFunc.copyProtos(tGo,bottomRight);
			// Apply alphamaps to all 4 terrains
			topLeft.GetComponent<Terrain>().terrainData.SetAlphamaps(0,0,tDat.GetAlphamaps(0,0,aRes,aRes));
			topRight.GetComponent<Terrain>().terrainData.SetAlphamaps(0,0,tDat.GetAlphamaps(aRes,0,aRes,aRes));
			bottomLeft.GetComponent<Terrain>().terrainData.SetAlphamaps(0,0,tDat.GetAlphamaps(0,aRes,aRes,aRes));
			bottomRight.GetComponent<Terrain>().terrainData.SetAlphamaps(0,0,tDat.GetAlphamaps(aRes,aRes,aRes,aRes));
			// Apply heightmaps to all 4 terrains
			topLeft.GetComponent<Terrain>().terrainData.SetHeights(0,0,tDat.GetHeights(0,0,hRes,hRes));
			topRight.GetComponent<Terrain>().terrainData.SetHeights(0,0,tDat.GetHeights(hRes-1,0,hRes,hRes));
			bottomLeft.GetComponent<Terrain>().terrainData.SetHeights(0,0,tDat.GetHeights(0,hRes-1,hRes,hRes));
			bottomRight.GetComponent<Terrain>().terrainData.SetHeights(0,0,tDat.GetHeights(hRes-1,hRes-1,hRes,hRes));
			// Apply detail layers to all 4 terrains
			int detailLayers = tDat.detailPrototypes.Length;
			for(int i=0;i<detailLayers;i++){
				topLeft.GetComponent<Terrain>().terrainData.SetDetailLayer(0,0,i,tDat.GetDetailLayer(0,0,dRes,dRes,i));
				topRight.GetComponent<Terrain>().terrainData.SetDetailLayer(0,0,i,tDat.GetDetailLayer(dRes,0,dRes,dRes,i));
				bottomLeft.GetComponent<Terrain>().terrainData.SetDetailLayer(0,0,i,tDat.GetDetailLayer(0,dRes,dRes,dRes,i));
				bottomRight.GetComponent<Terrain>().terrainData.SetDetailLayer(0,0,i,tDat.GetDetailLayer(dRes,dRes,dRes,dRes,i));			
			}
			// Apply tree instances to all 4 terrain
			for(int i=0;i<tDat.treeInstances.Length;i++){
				TreeInstance tmpTreeInstance  = new TreeInstance();
				tmpTreeInstance = tDat.treeInstances[i];
				if(tmpTreeInstance.position.x<0.5&&tmpTreeInstance.position.z<0.5){tmpTreeInstance.position = new Vector3(tmpTreeInstance.position.x*2f,tmpTreeInstance.position.y,tmpTreeInstance.position.z*2f); topLeft.GetComponent<Terrain>().AddTreeInstance(tmpTreeInstance);}
				else if(tmpTreeInstance.position.x>0.5&&tmpTreeInstance.position.z<0.5){tmpTreeInstance.position = new Vector3((tmpTreeInstance.position.x-0.5f)*2f,tmpTreeInstance.position.y,tmpTreeInstance.position.z*2f); topRight.GetComponent<Terrain>().AddTreeInstance(tmpTreeInstance);}
				else if(tmpTreeInstance.position.x<0.5&&tmpTreeInstance.position.z>0.5){tmpTreeInstance.position = new Vector3(tmpTreeInstance.position.x*2f,tmpTreeInstance.position.y,(tmpTreeInstance.position.z-0.5f)*2f); bottomLeft.GetComponent<Terrain>().AddTreeInstance(tmpTreeInstance);}
				else if(tmpTreeInstance.position.x>0.5&&tmpTreeInstance.position.z>0.5){tmpTreeInstance.position = new Vector3((tmpTreeInstance.position.x-0.5f)*2f,tmpTreeInstance.position.y,(tmpTreeInstance.position.z-0.5f)*2f); bottomRight.GetComponent<Terrain>().AddTreeInstance(tmpTreeInstance);}
			}
		}		
		
		public static string[] getProtoList(string prototypeType, TerrainData terdat){
			string[] protoNames = new string[16];
				switch(prototypeType){
				case "1":
					DetailPrototype[] protos = terdat.detailPrototypes;
					if (protos.Length > 0)
			        {
			            for (int i = 0; i < protos.Length; i++)
			            {
			                if (protos[i].prototypeTexture)
			                {
			                    protoNames[i] = protos[i].prototypeTexture.name;
			                }
			                else
			                {
			                    protoNames[i] = protos[i].prototype.name;
			                }
			            }
			        }
					break;
				case "2":
					SplatPrototype[] sprotos = terdat.splatPrototypes;
					if (sprotos.Length > 0)
			        {
			            for (int i = 0; i < sprotos.Length; i++)
			            {
			               protoNames[i] = sprotos[i].texture.name;
			            }
			        }
					break;
				case "3":
					TreePrototype[] tprotos = terdat.treePrototypes;
					if (tprotos.Length > 0)
			        {
			            for (int i = 0; i < tprotos.Length; i++)
			            {
			                protoNames[i] = tprotos[i].prefab.name;
			            }
			        }
					break;
				}
			return protoNames;
		}		

		public static void generateHeightmap(GameObject terrObject, ModuleBase modbase, float alphaAmount, float noiseAmp){
			Vector3 gopos = terrObject.transform.position;
	        float cwidth = terrObject.GetComponent<Terrain>().terrainData.size.x;
	        int resolution = terrObject.GetComponent<Terrain>().terrainData.heightmapResolution;
			float[,] hmap = new float[resolution,resolution];
	        double yoffset = 0 - (gopos.x / cwidth);
	        double xoffset = (gopos.z / cwidth);
	        Noise2D tmpNoiseMap = new Noise2D(resolution, resolution, modbase);
	        tmpNoiseMap.GeneratePlanar(xoffset, (xoffset) + (1f / resolution) * (resolution + 1), -yoffset, (-yoffset) + (1f / resolution) * (resolution + 1));
	        if (alphaAmount == 1.0f)
	        {
	            for (int hY = 0; hY < resolution; hY++)
	            {
	                for (int hX = 0; hX < resolution; hX++)
	                {
	                    hmap[hX, hY] = ((tmpNoiseMap[hX, hY]*0.5f) + 0.5f) * noiseAmp;
	                }
	            }
	        }
	        else
	        {
	            hmap = terrObject.GetComponent<Terrain>().terrainData.GetHeights(0, 0, resolution, resolution);
	            for (int hY = 0; hY < resolution; hY++)
	            {
	                for (int hX = 0; hX < resolution; hX++)
	                {
	                    hmap[hX, hY] = ((1.0f - alphaAmount) * hmap[hX, hY]) + (alphaAmount * (((tmpNoiseMap[hX, hY]*0.5f) + 0.5f) * noiseAmp));
	                }
	            }
	        }
	        terrObject.GetComponent<Terrain>().terrainData.SetHeights(0, 0, hmap);
	    }
		
		
		public static GameObject newTerrain(string name,int x, int z, float hMapWidth, float hMapHeight, int hMapRes, int aMapRes, int bMapRes, int dRes, int dResPP){
			TerrainData terrainData = new TerrainData();
			terrainData.heightmapResolution = hMapRes;
			terrainData.alphamapResolution = aMapRes;
			terrainData.baseMapResolution = bMapRes;
			terrainData.SetDetailResolution(dRes,dResPP);
			terrainData.name = "Terrain"+x+"_"+z;
			Vector3 size;
			size.x = hMapWidth;
			size.y = hMapHeight;
			size.z = hMapWidth;
			Vector3 pos;
			pos.x = hMapWidth * x;
			pos.y = 0.0f;
			pos.z = hMapWidth * z;
	        terrainData.size = size;
			terrainData.RefreshPrototypes();
			GameObject terrObj = Terrain.CreateTerrainGameObject(terrainData);
			terrObj.transform.position=pos;
			terrObj.name = name;
			terrObj.GetComponent<Terrain>().Flush();
			AssetDatabase.CreateAsset(terrainData, "Assets/"+name+".asset");
			return terrObj;
		}			
		
		public static void newTerrainAutoName(int x, int z, float hMapWidth, float hMapHeight, int hMapRes, int aMapRes, int bMapRes, int dRes, int dResPP){
			TerrainData terrainData = new TerrainData();
			terrainData.heightmapResolution = hMapRes;
			terrainData.alphamapResolution = aMapRes;
			terrainData.baseMapResolution = bMapRes;
			terrainData.SetDetailResolution(dRes,dResPP);
			terrainData.name = "Terrain"+x+"_"+z;
			Vector3 size;
			size.x = hMapWidth;
			size.y = hMapHeight;
			size.z = hMapWidth;
			Vector3 pos;
			pos.x = hMapWidth * x;
			pos.y = 0.0f;
			pos.z = hMapWidth * z;
	        terrainData.size = size;
			terrainData.RefreshPrototypes();
			GameObject terrObj = Terrain.CreateTerrainGameObject(terrainData);
			terrObj.transform.position=pos;
			terrObj.name = "t"+x+"."+z;
			terrObj.GetComponent<Terrain>().Flush();
			AssetDatabase.CreateAsset(terrainData, "Assets/Terrain"+x+"_"+z+".asset");
		}		

		public static void makeTerrainGrid(int gridX, int gridY, int preset){
			bool bailout = false;
			if(gridX*gridY>9){
				if(!EditorUtility.DisplayDialog("Bulk Processing","This will create " + gridX*gridY + " terrain objects.  Are you sure you wish to proceed?","Okay","Cancel")){
					bailout = true;
				}
			}
			if(bailout==false){
				for(int y=0;y<gridY;y++){
					for(int x=0;x<gridX;x++){
						switch(preset){
							case 0: newTerrainAutoName(x,y,1024,512,257,256,256,256,8);break;
							case 1: newTerrainAutoName(x,y,1024,512,513,512,512,512,8);break;
							case 2: newTerrainAutoName(x,y,512,512,257,257,257,257,8);break;
							case 3: newTerrainAutoName(x,y,512,512,129,129,129,129,8);break;
						}
					}
				}
			}		
		}

		public static byte[] dimFloatToBytes(float[,] floatArray,bool bigEndian){
			int res = (int)Mathf.Sqrt((float)floatArray.Length);
			byte[] bytesOut = new byte[floatArray.Length*2];
			int bIndex = 0;
			for(int y=0; y<res; y++){
				for(int x=0; x<res; x++){
					int wVal = (int)((floatArray[y,x]*65535f)/256f);
					int bVal = ((int)(floatArray[y,x]*65535f)) - (wVal*256);
					if(bigEndian==true){ //bigendian
						bytesOut[bIndex++] = (byte)wVal;
						bytesOut[bIndex++] = (byte)bVal;
					}else{
						bytesOut[bIndex++] = (byte)bVal;
						bytesOut[bIndex++] = (byte)wVal;
					}
				}
			}
			return bytesOut;
		}
			
		public static float[,] bytesToDimFloat(byte[] bytes,bool bigEndian){
			int res = (int)Mathf.Sqrt((float)(bytes.Length)/2.0f);
			float[,] floatOut = new float[res,res];
			int bIndex = 0; int tmpVal=0;
			for(int y=0; y<res; y++){
				for(int x=0; x<res; x++){
					if(bigEndian==true){
						tmpVal = (256*(int)bytes[bIndex++]) + (int)bytes[bIndex++];
					}else{
						tmpVal = (int)bytes[bIndex++] + (256*(int)bytes[bIndex++]);
					}
					floatOut[y,x]=(float)tmpVal/65535f;
				}
			}
			return floatOut;
		}		
		
		public static void moveSceneCam(Vector3 position,Vector3 rotation){
			SceneView.lastActiveSceneView.pivot = position;
			SceneView.lastActiveSceneView.rotation = Quaternion.Euler(rotation);
			SceneView.RepaintAll();
		}

		public static void reRes(GameObject go,int detailRes, int heightRes, int alphaRes)
		{
			TerrainData tdat = go.GetComponent<Terrain>().terrainData;
			TerrainData terrainData = new TerrainData();
			string goName = go.name+"_new";
			string terName = tdat.name;
			terrainData.heightmapResolution = heightRes;
			terrainData.alphamapResolution = alphaRes;
			terrainData.SetDetailResolution(detailRes,8);
			terrainData.baseMapResolution = tdat.baseMapResolution;
			terrainData.detailPrototypes = tdat.detailPrototypes;
			terrainData.splatPrototypes	 = tdat.splatPrototypes;
			terrainData.treePrototypes   = tdat.treePrototypes;
			terrainData.treeInstances    = tdat.treeInstances;
			terrainData.size = tdat.size;
			terrainData.RefreshPrototypes();
			GameObject terrObj = Terrain.CreateTerrainGameObject(terrainData);
			terrObj.transform.position=new Vector3(0f,0f,0f);
			terrObj.name = goName;	
			terrObj.GetComponent<Terrain>().Flush();
	
			float mult = 0.0f;	
			int destres = 0;
	
			destres = detailRes;
			int[,] detaildatasource = new int[tdat.detailResolution,tdat.detailResolution];
			int[,] detaildatadest = new int[destres,destres];
			for(int layer=0; layer<tdat.detailPrototypes.Length; layer++){
				detaildatasource = tdat.GetDetailLayer(0,0,tdat.detailResolution,tdat.detailResolution,layer);
				mult=(float)tdat.detailResolution/(float)destres;
				for(int y=0;y<destres;y++){
					for(int x=0;x<destres;x++){
						int xold = (int)(mult * (float)x);
						int yold = (int)(mult * (float)y);
						detaildatadest[x,y]=detaildatasource[xold,yold];
					}
				}
				terrainData.SetDetailLayer(0,0,layer,detaildatadest);
			}
			destres = heightRes;
			float[,] heightdatasource = new float[tdat.heightmapResolution,tdat.heightmapResolution];
			float[,] heightdatadest = new float[destres,destres];
			heightdatasource = tdat.GetHeights(0,0,tdat.heightmapResolution,tdat.heightmapResolution);
			mult=((float)tdat.heightmapResolution-1.0f)/((float)destres-1.0f);
			for(int y=0;y<destres;y++){
				for(int x=0;x<destres;x++){
					int xold = (int)(mult * (float)x);
					int yold = (int)(mult * (float)y);
					heightdatadest[x,y]=heightdatasource[xold,yold];
				}
			}
			destres = alphaRes;
			float[,,] alphadata = tdat.GetAlphamaps(0,0,tdat.alphamapResolution,tdat.alphamapResolution);
			float[,,] alphadatadest = new float[destres,destres,tdat.splatPrototypes.Length];
			mult=(float)tdat.alphamapResolution/(float)destres;
			for(int i=0;i<tdat.splatPrototypes.Length;i++){
				for(int y=0;y<destres;y++){
					for(int x=0;x<destres;x++){
						int xold = (int)(mult * (float)x);
						int yold = (int)(mult * (float)y);
						alphadatadest[x,y,i] = alphadata[xold,yold,i];
					}
				}
			}
			terrObj.GetComponent<Terrain>().terrainData.SetHeights(0,0,heightdatadest);
			AssetDatabase.CreateAsset(terrainData, "Assets/"+terName+"_new.asset");
			AssetDatabase.SaveAssets();
			TerrainData terrainData2Splat = (TerrainData)AssetDatabase.LoadAssetAtPath("Assets/"+terName+"_new.asset", typeof(TerrainData));
			terrainData2Splat.splatPrototypes = tdat.splatPrototypes;
	        terrainData2Splat.RefreshPrototypes();
	        terrainData2Splat.SetAlphamaps (0, 0, alphadatadest);
			AssetDatabase.SaveAssets();
			go.active = false;
		}	
	
		public static bool isTerrain(GameObject go){
			if(!go){return false;}
			if(go.GetComponent<Terrain>()!=null){return true;}
			return false;
		}		

		public static GameObject[] getNeighborhood(GameObject go)
		{
			GameObject[] daHood = new GameObject[9];
			daHood[4]=go;
			Terrain[] terrains = FindObjectsOfType(typeof(Terrain)) as Terrain[];
	        Vector3 gopos = go.transform.position;
	        float cwidth = go.GetComponent<Terrain>().terrainData.size.x;
	        foreach (Terrain terrain in terrains)
	        {
	            Vector3 tpos = terrain.transform.position;
	            if (tpos.x == gopos.x - cwidth && tpos.z == gopos.z) { daHood[1] = terrain.gameObject; }
	            if (tpos.x == gopos.x + cwidth && tpos.z == gopos.z) { daHood[7] = terrain.gameObject; }
	            if (tpos.z == gopos.z - cwidth && tpos.x == gopos.x) { daHood[3] = terrain.gameObject; }
	            if (tpos.z == gopos.z + cwidth && tpos.x == gopos.x) { daHood[5] = terrain.gameObject; }
	            if (tpos.x == gopos.x - cwidth && tpos.z == gopos.z - cwidth) { daHood[0] = terrain.gameObject; }
	            if (tpos.x == gopos.x - cwidth && tpos.z == gopos.z + cwidth) { daHood[2] = terrain.gameObject; }
	            if (tpos.x == gopos.x + cwidth && tpos.z == gopos.z - cwidth) { daHood[6] = terrain.gameObject; }
	            if (tpos.x == gopos.x + cwidth && tpos.z == gopos.z + cwidth) { daHood[8] = terrain.gameObject; }
	        }
			return daHood;
		}

		public static void doSync(bool syncSettings,bool syncSplat,bool syncTree,bool syncDetail)
	    {
	        Terrain[] terrains = FindObjectsOfType(typeof(Terrain)) as Terrain[];
	        Terrain activeTerrain = Selection.activeGameObject.GetComponent<Terrain>();
	        foreach (Terrain terrain in terrains)
	        {
	            if (terrain.gameObject != Selection.activeGameObject)
	            {
	                if (syncSettings)
	                {
	                    terrain.basemapDistance = activeTerrain.basemapDistance;
	                    terrain.castShadows = activeTerrain.castShadows;
	                    terrain.detailObjectDensity = activeTerrain.detailObjectDensity;
	                    terrain.detailObjectDistance = activeTerrain.detailObjectDistance;
	                    terrain.heightmapPixelError = activeTerrain.heightmapPixelError;
	                    terrain.treeBillboardDistance = activeTerrain.treeBillboardDistance;
	                    terrain.treeCrossFadeLength = activeTerrain.treeCrossFadeLength;
	                    terrain.treeDistance = activeTerrain.treeDistance;
	                }
	                if (syncSplat){terrain.terrainData.splatPrototypes = activeTerrain.terrainData.splatPrototypes;}
	                if (syncTree){terrain.terrainData.treePrototypes = activeTerrain.terrainData.treePrototypes;}
	                if (syncDetail){terrain.terrainData.detailPrototypes = activeTerrain.terrainData.detailPrototypes;}
	            }
	        }
	    }		

	
		public static void experiment001(){
			GameObject go = Selection.activeGameObject;
			TerrainData terdata = go.GetComponent<Terrain>().terrainData;
			float[,] heights = terdata.GetHeights(0,0,terdata.heightmapResolution,terdata.heightmapResolution);	
			Undo.RegisterUndo(terdata,"Experiment001");	
			float[] flowTexParams = new float[4]{20f,0.004f,0.004f,1.0f};
			//float[,] flows = new float[terdata.heightmapResolution,terdata.heightmapResolution];
			
			for(int i = 0;i<(int)flowTexParams[0];i++){
				EditorUtility.DisplayProgressBar("ThinFlowCut","ThinFlowCut "+i+" / "+flowTexParams[0].ToString("N0"),(float)i*(1f/flowTexParams[0]));
			    
			    for(int hY=1;hY<terdata.heightmapResolution-1;hY++){
			    	for(int hX=1;hX<terdata.heightmapResolution-1;hX++){
						int heightres = Selection.activeGameObject.GetComponent<Terrain>().terrainData.heightmapResolution;
						float cumulativedrop = 0f; 
						float nextHighest = 0f;
						if(terdata.GetSteepness(1f/(float)hY,1f/(float)hX)>45f){
							for(int nY = hY-1; nY < hY+2; nY++){
								for(int nX = hX-1; nX < hX+2; nX++){
									if(nY>0&&nX>0&&nX<heightres&&nY<heightres){
										if(heights[nX,nY]<heights[hX,hY]){
											cumulativedrop += heights[hX,hY]-heights[nX,nY];
										}
										if(heights[nX,nY]>heights[hX,hY]&&heights[nX,nY]<nextHighest){
											nextHighest = heights[nX,nY];
										}
									}
								}	
							}	
							if(cumulativedrop>0f){
								for(int nY = hY-1; nY < hY+2; nY++){
									for(int nX = hX-1; nX < hX+2; nX++){
										if(nY>0&&nX>0&&nX<heightres&&nY<heightres){
											if(heights[nX,nY]<heights[hX,hY]){
												heights[nX,nY] -= 0.7f*((heights[hX,hY]-heights[nX,nY]) * cumulativedrop);
												heights[hX,hY] -= 0.7f*((heights[hX,hY]-heights[nX,nY]) * cumulativedrop);
											}
										}
									}	
								}
							}
						}
					}
				}
				for(int hY=1;hY<terdata.heightmapResolution-1;hY++){
				    for(int hX=1;hX<terdata.heightmapResolution-1;hX++){
						heights[hX,hY]=(heights[hX,hY]);
					}
				}
			}
			terdata.SetHeights(0,0,heights);
			EditorUtility.ClearProgressBar();
		}
			
		
		#endregion
	}
}
