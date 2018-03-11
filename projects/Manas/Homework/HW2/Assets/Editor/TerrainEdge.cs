using UnityEngine;
using UnityEditor;
using TerEdge;
using System.Collections;
using LibNoise.Unity;
using LibNoise.Unity.Generator;
using LibNoise.Unity.Operator;
using System.IO;
using System.Collections.Generic;
using System.Text;

// TerrainEdge GUI/Interface

public abstract class TEGroup 
{
    public string Description { get; private set; }
    public string Name { get; private set; }
    private bool m_initialized = false;
	
    public TEGroup(string name, string description)
    {
        Name = name;
        Description = description;
    }

    public void Init()
    {
        if (!m_initialized)
        {
            m_initialized = true;
            OnInit(); // make sure the protected init only gets called ONCE.
        }
    }
    protected abstract void OnInit(); // protected so outside users don't call it accidentally.
    public abstract void OnGUI();
    public abstract void Generate(GameObject go);
	public abstract void sceneEvents(SceneView sceneview);
	public static Object[] FindObjectsOfType(System.Type type) { // wrapper method cause alot of the existing code used it
		return Object.FindObjectsOfType(type);
	}
    
}

// === TE:About ===============================================================================

public class TEAbout: TEGroup
{
    public TEAbout() : base("About TerrainEdge", "Version: " + TerrainEdge.vers + "\n\nCheck homepage link below for credits.") { }
    protected override void OnInit(){}
    public override void OnGUI(){
		GUILayout.BeginVertical(EditorStyles.textField);
		GUILayout.Label("Useful hyperlinks:");
		GUILayout.BeginHorizontal();
		if(teUI.MiniButton("Homepage")){Application.OpenURL("http://www.creativeinsomnia.net/tl/unity/te.html");}
		if(teUI.MiniButton("Discuss")){Application.OpenURL("http://forum.unity3d.com/threads/118820-TerrainEdge-3-%28need-testers-feedback-ideas-etc%29");	}
		GUILayout.EndHorizontal();
		GUILayout.Space(4f);
		GUILayout.EndVertical();
	}
    public override void Generate(GameObject go){}
	public override void sceneEvents(SceneView sceneview){}
}

// === TE:Detail ===============================================================================

public class TEDetail : TEGroup
{
	public static string[] teDetailRules = new string[10] {"Detail Rule 1","Detail Rule 2","Detail Rule 3","Detail Rule 4","Detail Rule 5","Detail Rule 6","Detail Rule 7","Detail Rule 8","Detail Rule 9","Detail Rule 10"};
	public static int teDetailRuleIndex = 0;
	public static int[] teDetailRuleProtoId = new int[10];
	public static float[,] teDetailRuleParams = new float[10,8];
	public static bool[] teDetailSplatPrototypeEnable = new bool[10];
	public static int[] teDetailSplatPrototypeMatch = new int[10];
	public static float[] teDetailSplatPrototypeAmount = new float[10];
	
    public TEDetail() : base("TE:Detail", "Generate detail layers for your terrain(s) with a stack of 10 height/slope rules.") { }
    protected override void OnInit()
    {
        for (int i = 0; i < 20; i++)
        {
            if (i < 10)
            {
                TEDetail.teDetailRuleParams[i, 1] = 1.0f;
                TEDetail.teDetailRuleParams[i, 3] = 90.0f;
                TEDetail.teDetailRuleParams[i, 6] = 0.5f;
            }
        }
    }
    public override void OnGUI()
    {
        GUILayout.BeginVertical(EditorStyles.textField);
        if (TerrainEdge.isObjectTerrain == true)
        {
            GUILayout.BeginHorizontal();
            teDetailRuleIndex = EditorGUILayout.Popup(teDetailRuleIndex, teDetailRules);
            DetailPrototype[] detaildata = TerrainEdge.selectedObject.GetComponent<Terrain>().terrainData.detailPrototypes;
            string[] teDetailOptions = new string[detaildata.Length + 1];
            teDetailOptions[0] = "(none)";
            if (detaildata.Length > 0)
            {
                for (int i = 0; i < detaildata.Length; i++)
                {
					
                    if (detaildata[i].prototypeTexture)
                    {
                        teDetailOptions[i + 1] = detaildata[i].prototypeTexture.name;
                    } else {
						teDetailOptions[i + 1] = detaildata[i].prototype.name;	
					}

                }
            }
            teDetailRuleProtoId[teDetailRuleIndex] = EditorGUILayout.Popup(teDetailRuleProtoId[teDetailRuleIndex], teDetailOptions, GUILayout.Width(120));
            GUILayout.EndHorizontal();
            if (teDetailRuleProtoId[teDetailRuleIndex] > 0)
            {

                GUILayout.BeginHorizontal();
                GUILayout.Label("Height:", GUILayout.Width(50));
                GUILayout.Label(teDetailRuleParams[teDetailRuleIndex, 0].ToString("N2"), GUILayout.Width(40));
                EditorGUILayout.MinMaxSlider(ref teDetailRuleParams[teDetailRuleIndex, 0], ref teDetailRuleParams[teDetailRuleIndex, 1], 0.0f, 1.0f, GUILayout.MinWidth(40));
                GUILayout.Label(teDetailRuleParams[teDetailRuleIndex, 1].ToString("N2"), GUILayout.Width(40));
                GUILayout.EndHorizontal();

                GUILayout.BeginHorizontal();
                GUILayout.Label("Slope:", GUILayout.Width(50));
                GUILayout.Label(teDetailRuleParams[teDetailRuleIndex, 2].ToString("N2"), GUILayout.Width(40));
                EditorGUILayout.MinMaxSlider(ref teDetailRuleParams[teDetailRuleIndex, 2], ref teDetailRuleParams[teDetailRuleIndex, 3], 0.0f, 90.0f, GUILayout.MinWidth(40));
                GUILayout.Label(teDetailRuleParams[teDetailRuleIndex, 3].ToString("N2"), GUILayout.Width(40));
                GUILayout.EndHorizontal();

				GUILayout.BeginHorizontal();
				GUILayout.Label ("Coverage:", GUILayout.Width(96));
				teDetailRuleParams[teDetailRuleIndex,7] = GUILayout.HorizontalSlider(teDetailRuleParams[teDetailRuleIndex,7],0.0f,1.0f,GUILayout.MinWidth(40));
				GUILayout.Label (teDetailRuleParams[teDetailRuleIndex,7].ToString("N2"), GUILayout.Width(40));
				GUILayout.EndHorizontal();				
				
                GUILayout.BeginHorizontal();
                GUILayout.Label("Strength:", GUILayout.Width(96));
                teDetailRuleParams[teDetailRuleIndex, 6] = GUILayout.HorizontalSlider(teDetailRuleParams[teDetailRuleIndex, 6], 1.0f, 15.0f, GUILayout.MinWidth(40));
                GUILayout.Label(teDetailRuleParams[teDetailRuleIndex, 6].ToString("N0"), GUILayout.Width(40));
                GUILayout.EndHorizontal();
				
				teDetailSplatPrototypeEnable[teDetailRuleIndex] = GUILayout.Toggle(teDetailSplatPrototypeEnable[teDetailRuleIndex],"Filter By Splatmap");
				if(teDetailSplatPrototypeEnable[teDetailRuleIndex]==true){
				GUILayout.BeginHorizontal();
				GUILayout.Label ("Prototype:", GUILayout.Width(96));
				if(TerrainEdge.selectedObject.GetComponent<Terrain>().terrainData.splatPrototypes.Length>0){
					SplatPrototype[] splatdata = TerrainEdge.selectedObject.GetComponent<Terrain>().terrainData.splatPrototypes;
					string[] teSplatTextures = new string[splatdata.Length];
					if(splatdata.Length>0){
						for(int i=0;i<splatdata.Length;i++){
							teSplatTextures[i] = splatdata[i].texture.name;
						}
						teDetailSplatPrototypeMatch[teDetailRuleIndex] = EditorGUILayout.Popup(teDetailSplatPrototypeMatch[teDetailRuleIndex],teSplatTextures,GUILayout.MinWidth(40));
					} 
				} else {
					GUILayout.Label ("No textures on this terrain!");	
				}
				GUILayout.EndHorizontal();
				GUILayout.BeginHorizontal();
					GUILayout.Label ("Threshold:", GUILayout.Width(96));
					teDetailSplatPrototypeAmount[teDetailRuleIndex] = GUILayout.HorizontalSlider(teDetailSplatPrototypeAmount[teDetailRuleIndex],0.0f,1.0f,GUILayout.MinWidth(40));
					GUILayout.Label (teDetailSplatPrototypeAmount[teDetailRuleIndex].ToString("N2"), GUILayout.Width(40));
				GUILayout.EndHorizontal();
				}
            }
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Selected"))
            {
				Undo.RegisterUndo(TerrainEdge.selectedObject.GetComponent<Terrain>().terrainData,"te:Apply detail to selected terrain.");
                Generate(TerrainEdge.selectedObject);
            }
            if (GUILayout.Button("Full Scene"))
            {
                Terrain[] terrs = FindObjectsOfType(typeof(Terrain)) as Terrain[];
				Undo.RegisterUndo(terrs,"te:Apply detail to all terrains");
                int terrIndex = 0;
                foreach (Terrain terr in terrs)
                {
					EditorUtility.DisplayProgressBar("Generating Detail Layers", "Generating " + terr.gameObject.name + " (" + (terrIndex + 1) + " of " + terrs.Length + ")....", (float)(terrIndex * (1.0f / terrs.Length)));
                    Generate(terr.gameObject);
                    EditorUtility.ClearProgressBar();
                    terrIndex++;
                }
            }
            GUILayout.EndHorizontal();
        }
        else
        {
            GUILayout.Label("Select a terrain object.", EditorStyles.miniBoldLabel);
        }
        GUILayout.EndVertical();
    }

	public static void genIt(GameObject go){
		TerrainData terdata = go.GetComponent<Terrain>().terrainData;
		int res = terdata.detailResolution;
		int[,] detaildata = new int[res,res];
		float tmpHeight = 0.0f;
		float tmpSlope = 0.0f;
		float strengthmult = 1.0f;
		for(int layer=0; layer<terdata.detailPrototypes.Length; layer++){
			terdata.SetDetailLayer(0,0,layer,detaildata);
		}
		float addAmount = 0.0f;
		float tmpSplatAmount = 0f;
		for(int ruleId=0;ruleId<10;ruleId++){
			if(teDetailRuleProtoId[ruleId]>0){
				detaildata = terdata.GetDetailLayer(0,0,res,res,teDetailRuleProtoId[ruleId]);
				float[,,] splatMaps = terdata.GetAlphamaps(0,0,terdata.alphamapResolution,terdata.alphamapResolution);
				float resmult = (1.0f / (float)res)*(float)terdata.alphamapResolution;
				for(int y=0;y<res;y++){
					for(int x=0;x<res;x++){
						strengthmult = teDetailRuleParams[ruleId,6];
						tmpHeight = (terdata.GetHeight(y,x)/(terdata.size.y));		
						tmpSlope = terdata.GetSteepness(((1.0f/(float)res)*y)+(0.5f/(float)res),((1.0f/(float)res)*x)+(0.5f/(float)res));
						if(teDetailSplatPrototypeEnable[ruleId]==true){
							tmpSplatAmount = splatMaps[(int)(resmult*(float)x),(int)(resmult*(float)y),teDetailSplatPrototypeMatch[ruleId]]; 
							if(tmpSplatAmount<teDetailSplatPrototypeAmount[ruleId]){strengthmult = 0f;}
						}
						if(tmpHeight<teDetailRuleParams[ruleId,0]){strengthmult = 0.0f;}
						if(tmpHeight>teDetailRuleParams[ruleId,1]){strengthmult = 0.0f;}
						if(tmpSlope<teDetailRuleParams[ruleId,2]){strengthmult = 0.0f;}
						if(tmpSlope>teDetailRuleParams[ruleId,3]){strengthmult = 0.0f;}
						if(UnityEngine.Random.value<teDetailRuleParams[ruleId,7]){
							addAmount = strengthmult;
							int tmpval = detaildata[x,y]+(int)addAmount;
							if(tmpval>15){tmpval=15;}
							detaildata[x,y]=tmpval;
						}
					}
				}

				if(teDetailRuleProtoId[ruleId]-1<terdata.detailPrototypes.Length){
					terdata.SetDetailLayer(0,0,teDetailRuleProtoId[ruleId]-1,detaildata);
				}
			}	
		}
	}
	
    public override void Generate(GameObject go){genIt(go);}
	public override void sceneEvents(SceneView sceneview){}
}

// === TE:Expand ===============================================================================

public class TEExpand : TEGroup
{
	public static int teTerObjGenGridX;
	public static int teTerObjGenGridY;
	public static int teTerObjGenPresetIndex;
	
    public static bool[] teExpandOpts = new bool[4] { true, true, true, true };

    public TEExpand() : base("TE:Expand", "Create new terrain tiles (based on selected terrain's settings) with a single click.") { }
    protected override void OnInit() {teTerObjGenGridX=1;teTerObjGenGridY=1;}
	public override void sceneEvents(SceneView sceneview){}
	public override void Generate(GameObject go){}
	public override void OnGUI()
    {
        GUILayout.BeginVertical(EditorStyles.textField);
		if(SceneView.FindObjectsOfType(typeof(Terrain)).Length==0)
		{
			GUILayout.BeginVertical();
			teUI.LabelBold("Please select:");
			teTerObjGenPresetIndex = EditorGUILayout.Popup(teTerObjGenPresetIndex,new string[4]{"1024x512x1024 (256 res)","1024x512x1024 (512 res)","512x512x512 (256 res)","256x256x256 (128 res)"});
			teTerObjGenGridX = (int)TerrainEdge.teFloatSlider("Tiles X",(float)teTerObjGenGridX,1f,50f);
			teTerObjGenGridY = (int)TerrainEdge.teFloatSlider("Tiles X",(float)teTerObjGenGridY,1f,50f);
			if(GUILayout.Button("Generate Terrain Objects")){teFunc.makeTerrainGrid(teTerObjGenGridX,teTerObjGenGridY,teTerObjGenPresetIndex);}
		} else {
			if (Selection.activeGameObject)
	        {
	            float gridX = Selection.activeGameObject.transform.position.x / Selection.activeGameObject.GetComponent<Terrain>().terrainData.size.x;
	            float gridZ = Selection.activeGameObject.transform.position.z / Selection.activeGameObject.GetComponent<Terrain>().terrainData.size.x;
	            if (Selection.activeGameObject.GetComponent<Terrain>())
	            {
	                GUILayout.BeginHorizontal();
	                GUILayout.BeginVertical();
	                GUILayout.Label("Terrain Neighborhood", EditorStyles.miniBoldLabel);
	                GUILayout.BeginHorizontal();
	                if (TerrainEdge.tileSW) { GUI.enabled = false; GUILayout.Button(TerrainEdge.tileSW.name, EditorStyles.miniButton, GUILayout.Height(24), GUILayout.Width(Screen.width / 5)); GUI.enabled = true; } else { if (GUILayout.Button("(create)", EditorStyles.miniButton, GUILayout.Height(24), GUILayout.Width(Screen.width / 5))) { makeTerrain((int)gridX - 1, (int)gridZ - 1); };;}
	                if (TerrainEdge.tileS) { GUI.enabled = false; GUILayout.Button(TerrainEdge.tileS.name, EditorStyles.miniButton, GUILayout.Height(24), GUILayout.Width(Screen.width / 5)); GUI.enabled = true; } else { if (GUILayout.Button("(create)", EditorStyles.miniButton, GUILayout.Height(24), GUILayout.Width(Screen.width / 5))) { makeTerrain((int)gridX - 1, (int)gridZ); };}
	                if (TerrainEdge.tileSE) { GUI.enabled = false; GUILayout.Button(TerrainEdge.tileSE.name, EditorStyles.miniButton, GUILayout.Height(24), GUILayout.Width(Screen.width / 5)); GUI.enabled = true; } else { if (GUILayout.Button("(create)", EditorStyles.miniButton, GUILayout.Height(24), GUILayout.Width(Screen.width / 5))) { makeTerrain((int)gridX - 1, (int)gridZ + 1); };}
	                GUILayout.EndHorizontal();
	                GUILayout.BeginHorizontal();
	                if (TerrainEdge.tileW) { GUI.enabled = false; GUILayout.Button(TerrainEdge.tileW.name, EditorStyles.miniButton, GUILayout.Height(24), GUILayout.Width(Screen.width / 5)); GUI.enabled = true; } else { if (GUILayout.Button("(create)", EditorStyles.miniButton, GUILayout.Height(24), GUILayout.Width(Screen.width / 5))) { makeTerrain((int)gridX, (int)gridZ - 1); };}
	                GUI.enabled = false; GUILayout.Button(Selection.activeGameObject.name, EditorStyles.miniButton, GUILayout.Height(24), GUILayout.Width(Screen.width / 5)); GUI.enabled = true;
	                if (TerrainEdge.tileE) { GUI.enabled = false; GUILayout.Button(TerrainEdge.tileE.name, EditorStyles.miniButton, GUILayout.Height(24), GUILayout.Width(Screen.width / 5)); GUI.enabled = true; } else { if (GUILayout.Button("(create)", EditorStyles.miniButton, GUILayout.Height(24), GUILayout.Width(Screen.width / 5))) { makeTerrain((int)gridX, (int)gridZ + 1); };}
	                GUILayout.EndHorizontal();
	                GUILayout.BeginHorizontal();
	                if (TerrainEdge.tileNW) { GUI.enabled = false; GUILayout.Button(TerrainEdge.tileNW.name, EditorStyles.miniButton, GUILayout.Height(24), GUILayout.Width(Screen.width / 5)); GUI.enabled = true; } else { if (GUILayout.Button("(create)", EditorStyles.miniButton, GUILayout.Height(24), GUILayout.Width(Screen.width / 5))) { makeTerrain((int)gridX + 1, (int)gridZ - 1); };}
	                if (TerrainEdge.tileN) { GUI.enabled = false; GUILayout.Button(TerrainEdge.tileN.name, EditorStyles.miniButton, GUILayout.Height(24), GUILayout.Width(Screen.width / 5)); GUI.enabled = true; } else { if (GUILayout.Button("(create)", EditorStyles.miniButton, GUILayout.Height(24), GUILayout.Width(Screen.width / 5))) { makeTerrain((int)gridX + 1, (int)gridZ); };}
	                if (TerrainEdge.tileNE) { GUI.enabled = false; GUILayout.Button(TerrainEdge.tileNE.name, EditorStyles.miniButton, GUILayout.Height(24), GUILayout.Width(Screen.width / 5)); GUI.enabled = true; } else { if (GUILayout.Button("(create)", EditorStyles.miniButton, GUILayout.Height(24), GUILayout.Width(Screen.width / 5))) { makeTerrain((int)gridX + 1, (int)gridZ + 1); };}
	                GUILayout.EndHorizontal();
					if(GUILayout.Button("Align Camera")){
						if(SceneView.lastActiveSceneView){
							Vector3 tPos = TerrainEdge.selectedObject.transform.position;
							TerrainData tData = TerrainEdge.selectedObject.GetComponent<Terrain>().terrainData;
							TerEdge.teFunc.moveSceneCam(new Vector3(tPos.x+(tData.size.x*0.5f),tData.size.y*4.0f,tPos.z+(tData.size.z*0.5f)),new Vector3(90.0f,270.0f,0.0f));
						}
					}
	                GUILayout.EndVertical();
	                GUILayout.BeginVertical();
	                GUILayout.Label("Apply", EditorStyles.miniBoldLabel);
	                teExpandOpts[0] = GUILayout.Toggle(teExpandOpts[0], "NoiseLab");
	                teExpandOpts[1] = GUILayout.Toggle(teExpandOpts[1], "Textures");
	                teExpandOpts[2] = GUILayout.Toggle(teExpandOpts[2], "Details");
	                teExpandOpts[3] = GUILayout.Toggle(teExpandOpts[3], "Trees");
	                GUILayout.EndVertical();
	                GUILayout.EndHorizontal();
	            }
	            else
	            {
	                GUILayout.Label("Select a terrain object to proceed.", EditorStyles.miniBoldLabel);
	            }
	        }
	        else
	        {
	            GUILayout.Label("Select a terrain object to proceed.", EditorStyles.miniBoldLabel);
	        }
		}
        GUILayout.EndVertical();
    }
	



	
    void makeTerrain(int x, int z)
    {
        EditorUtility.DisplayProgressBar("Expand Terrain", "Creating GameObject...", 0.5f);
        TerrainData terrainData = new TerrainData();
        TerrainData selTerrainData = Selection.activeGameObject.GetComponent<Terrain>().terrainData;
        terrainData.heightmapResolution = selTerrainData.heightmapResolution;
        terrainData.alphamapResolution = selTerrainData.alphamapResolution;
        terrainData.baseMapResolution = selTerrainData.baseMapResolution;
        terrainData.SetDetailResolution(selTerrainData.detailResolution, 8);
        terrainData.name = "Terrain" + x + "_" + z;
        Vector3 pos;
        pos.x = selTerrainData.size.x * x;
        pos.y = 0.0f;
        pos.z = selTerrainData.size.z * z;
        terrainData.size = selTerrainData.size;
        terrainData.RefreshPrototypes();
        GameObject terrObj = Terrain.CreateTerrainGameObject(terrainData);
        terrObj.transform.position = pos;
        terrObj.name = "t" + x + "." + z;
        terrObj.GetComponent<Terrain>().Flush();
        AssetDatabase.CreateAsset(terrainData, "Assets/Terrain" + x + "_" + z + ".asset");
        if (teExpandOpts[0] == true) { EditorUtility.DisplayProgressBar("Expand Terrain", "Generating Heightmap...", 0.5f); TENoiseLab.genIt(terrObj); }
        if (teExpandOpts[1] == true) { EditorUtility.DisplayProgressBar("Expand Terrain", "Generating Textures...", 0.5f); terrainData.splatPrototypes = selTerrainData.splatPrototypes; TETextures.genIt(terrObj); }
        if (teExpandOpts[2] == true) { EditorUtility.DisplayProgressBar("Expand Terrain", "Generating Detail...", 0.5f); terrainData.detailPrototypes = selTerrainData.detailPrototypes; TEDetail.genIt(terrObj); }
        if (teExpandOpts[3] == true) { EditorUtility.DisplayProgressBar("Expand Terrain", "Generating Trees...", 0.5f); terrainData.treePrototypes = selTerrainData.treePrototypes; TETrees.genIt(terrObj); }
        EditorUtility.ClearProgressBar();
		TerrainEdge.getNeighbors(Selection.activeGameObject);
    }
}

// === TE:FoliageGroup ===============================================================================

public class TEFoliageGroup : TEGroup
{
	// foliageParams:
	/* (0) foliage type  {0 = none, 1 = detail, 2 = texture, 3 = trees}
	 * (1) foliage parameters  (See below)
	 * typedesc : prototype : target_strength : density/dither 
	 * typedesc : prototype : target_strength : density/dither : color : lightmap color width:height:widthvariance:heightvariance
	 */
	
	public static string[,] teFoliageParams = new string[10,10]; // foliageGroupId,slot = paramslist
	public int teFoliageGroupIndex;
	public int teFoliageSlotIndex;
	public int teFoliageProtoIndex;
	public string[] teFoliageGroupNames = new string[10] {"Group 1","Group 2","Group 3","Group 4","Group 5","Group 6","Group 7","Group 8","Group 9","Group 10"};
	public string[] teFoliageSlotNames = new string[10] {"Slot 1","Slot 2","Slot 3","Slot 4","Slot 5","Slot 6","Slot 7","Slot 8","Slot 9","Slot 10"};
	public string[] teFoliageGroupOptions = new string[4] {"(none)","Detail","Textures","Trees"};
	public string[] teFoliageGroupDetailProtoNames = new string[16];
	public string[] teFoliageGroupTreeProtoNames = new string[16];
	public string[] teFoliageGroupTextureProtoNames = new string[16];
	private List<TreeInstance> TreeInstances;
	public int teFoliageBrushSize = 1;
	public bool teFoliageReplaceMode = false;
	public int teFoliageOptionIndex;
	public bool paintMode = false;
	
	
	public TEFoliageGroup() : base("TE:FoliageGroup", "Create a group consisting of splat textures, detail layers and/or trees which can then be painted onto terrain.") { }
	
	protected override void OnInit() {
		for(int i=0;i<10;i++){
			for(int i2=0;i2<10;i2++){
				teFoliageParams[i,i2]="0:0:0:0:0:0:0:0:0:0:0:0:0";
			}
		}
	}
		
	public override void sceneEvents(SceneView sceneview){
		if(paintMode==false){return;}
		
		if(Event.current.type==EventType.MouseDown){
			Ray ray = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition);
			RaycastHit hit = new RaycastHit();
			if(Physics.Raycast(ray, out hit, 7000.0f)){
				GameObject go = hit.collider.gameObject;
				TerrainData td = go.GetComponent<Terrain>().terrainData;
				Undo.RegisterUndo(td,"Paste Foliage");
				// Need to make alpha and detail res maps 3x size so that we can manage neighbourhood but we'll do that in 2.68! 
				int x = (int)hit.point.x - (int)go.transform.position.x;
				int z = (int)hit.point.z - (int)go.transform.position.z;
				int detailres = td.detailResolution;
				int alphares = td.alphamapResolution;
				float detailMult = (1.0f / td.size.x)*(float)detailres;
				float alphaMult = (1.0f / td.size.x)*(float)alphares;
				int detX = (int)(detailMult*(float)x); //x:z coords for other map resolutions
				int detZ = (int)(detailMult*(float)z);
				int alphaX = (int)(alphaMult*(float)x);
				int alphaZ = (int)(alphaMult*(float)z); 
				Event.current.Use();
				int[,] detaildata = new int[detailres,detailres]; 
				float strengthmult =1.0f;
				bool[,] posTaken = new bool[(int)td.size.x,(int)td.size.x];
				
				if(td.detailPrototypes.Length>0&&teFoliageReplaceMode==true){
					// If this is replace mode, clear the region first...
					int pasteregionoffset = (int)((float)teFoliageBrushSize*detailMult);		
					for(int i=0;i<td.detailPrototypes.Length;i++){
						detaildata = td.GetDetailLayer(0,0,detailres,detailres,i);
						for(int tZ=(alphaZ)-pasteregionoffset;tZ<(alphaZ)+(pasteregionoffset+1);tZ++){
							for(int tX=(alphaX)-pasteregionoffset;tX<(alphaX)+(pasteregionoffset+1);tX++){
								if(tZ>=0&&tZ<=alphares-1&&tX>=0&&tX<=alphares-1){
									detaildata[tZ,tX]=0;
								}
							}						
						}
						td.SetDetailLayer(0,0,i,detaildata);
					}
				}
				
				
				for(int i=0;i<10;i++){
					float[,,] alphadata = td.GetAlphamaps(0,0,alphares,alphares);
					string paramlist = teFoliageParams[teFoliageGroupIndex,i];
					if(paramlist!=null){
						string[] param = paramlist.Split(":".ToCharArray()[0]);
						if(param[0]=="1"){
							// Detail -------------------------------------------
							detaildata = td.GetDetailLayer(0,0,detailres,detailres,int.Parse(param[1]));
							int pasteregionoffset = (int)((float)teFoliageBrushSize*detailMult);
							for(int tZ=(detZ)-pasteregionoffset;tZ<(detZ)+(pasteregionoffset+1);tZ++){
								for(int tX=(detX)-pasteregionoffset;tX<(detX)+(pasteregionoffset+1);tX++){
									strengthmult = (float.Parse(param[2])*15f)*teFunc.falloffMult(tX,detX,tZ,detZ,pasteregionoffset);
									/* We might add slope/elevation rules to the foliage painting system so I will leave this here for now...
									 * 
									tmpHeight = (terdata.GetHeight(y,x)/(terdata.heightmapHeight*2.0f));		
									tmpSlope = terdata.GetSteepness(((1.0f/(float)res)*y)+(0.2f/(float)res),((1.0f/(float)res)*x)+(0.2f/(float)res));
									if(teDetailSplatPrototypeEnable[ruleId]==true){
										tmpSplatAmount = splatMaps[(int)(resmult*(float)x),(int)(resmult*(float)y),teDetailSplatPrototypeMatch[ruleId]]; 
										if(tmpSplatAmount<teDetailSplatPrototypeAmount[ruleId]){strengthmult = 0f;}
									}
									if(tmpHeight<teDetailRuleParams[ruleId,0]){strengthmult = 0.0f;}
									if(tmpHeight>teDetailRuleParams[ruleId,1]){strengthmult = 0.0f;}
									if(tmpSlope<teDetailRuleParams[ruleId,2]){strengthmult = 0.0f;}
									if(tmpSlope>teDetailRuleParams[ruleId,3]){strengthmult = 0.0f;}*/
									if(tZ>=0&&tZ<=detailres-1&&tX>=0&&tX<=detailres-1){
										if(UnityEngine.Random.value<float.Parse(param[3])){;
											int tmpval = detaildata[tZ,tX]+(int)strengthmult;
											if(tmpval>15){tmpval=15;}
											detaildata[tZ,tX]=tmpval;
										}
									}
								}
							}
							td.SetDetailLayer(0,0,int.Parse(param[1]),detaildata);
						}
						if(param[0]=="2"){
							// Splat/Texture  -------------------------------------------
							int pasteregionoffset = (int)((float)teFoliageBrushSize*alphaMult);
							for(int tZ=(alphaZ)-pasteregionoffset;tZ<(alphaZ)+(pasteregionoffset+1);tZ++){
								for(int tX=(alphaX)-pasteregionoffset;tX<(alphaX)+(pasteregionoffset+1);tX++){
									strengthmult = float.Parse(param[3])*teFunc.falloffMult(tX,alphaX,tZ,alphaZ,pasteregionoffset);
									/* We might add slope/elevation rules to the foliage painting system so I will leave this here for now...
									 * 
									tmpHeight = (terdata.GetHeight(y,x)/(terdata.heightmapHeight*2.0f));		
									tmpSlope = terdata.GetSteepness(((1.0f/(float)res)*y)+(0.2f/(float)res),((1.0f/(float)res)*x)+(0.2f/(float)res));
									if(teDetailSplatPrototypeEnable[ruleId]==true){
										tmpSplatAmount = splatMaps[(int)(resmult*(float)x),(int)(resmult*(float)y),teDetailSplatPrototypeMatch[ruleId]]; 
										if(tmpSplatAmount<teDetailSplatPrototypeAmount[ruleId]){strengthmult = 0f;}
									}
									if(tmpHeight<teDetailRuleParams[ruleId,0]){strengthmult = 0.0f;}
									if(tmpHeight>teDetailRuleParams[ruleId,1]){strengthmult = 0.0f;}
									if(tmpSlope<teDetailRuleParams[ruleId,2]){strengthmult = 0.0f;}
									if(tmpSlope>teDetailRuleParams[ruleId,3]){strengthmult = 0.0f;}*/
									if(tZ>=0&&tZ<=alphares-1&&tX>=0&&tX<=alphares-1){
										if(UnityEngine.Random.value<float.Parse(param[2])){
											float addAmount = strengthmult;
											float remainder = 1.0f-addAmount;
											// Now we tally up the total value shared by other splat channels...
											float cumulativeAmount = 0.0f;
											for(int i2=0;i2<td.splatPrototypes.Length;i2++){
												if(i2!=int.Parse(param[1])){
													cumulativeAmount=cumulativeAmount+alphadata[tZ,tX,i2];
												}
											}
											if(cumulativeAmount>0.0f){
												float fixLayerMult = remainder / cumulativeAmount; // we multiple the other layer's splat values by this
												// Now we re-apply the splat values..
												for(int i2=0;i2<td.splatPrototypes.Length;i2++){
													if(i2!=int.Parse(param[1])){
														alphadata[tZ,tX,i2] = fixLayerMult*alphadata[tZ,tX,i2];
													} else {
														alphadata[tZ,tX,i2] = addAmount;
													}
												}
											} else {
												alphadata[tZ,tX,int.Parse(param[1])] = 1.0f;
											}
										}
									}
								}
							}
						}
						if(param[0]=="3"){
							// Trees -------------------------------------------------------
							int pasteregionoffset = teFoliageBrushSize;
							TreeInstance[] trees = td.treeInstances;
							TreeInstances = new List<TreeInstance>(go.GetComponent<Terrain>().terrainData.treeInstances);
							if(TreeInstances.Count>0&&teFoliageReplaceMode==true){
								for(int i2=0;i2<TreeInstances.Count;i2++){
									if(TreeInstances[i2].position.x*td.size.x>x-pasteregionoffset&&trees[i2].position.x*td.size.x<x+pasteregionoffset&&trees[i2].position.z*td.size.z>z-pasteregionoffset&&trees[i2].position.z*td.size.z<z+pasteregionoffset){
										TreeInstances.RemoveAt(i2);
									}
								}
								td.treeInstances = TreeInstances.ToArray();
							}
							strengthmult = float.Parse(param[3]);
							for(int tZ=z-pasteregionoffset;tZ<z+pasteregionoffset;tZ++){
								for(int tX=x-pasteregionoffset;tX<x+pasteregionoffset;tX++){
									if(tZ>=0&&tZ<=td.size.z-1&&tX>=0&&tX<=td.size.x-1){
										
										if(posTaken[tX,tZ]!=true&&UnityEngine.Random.value<(0.1f*strengthmult*teFunc.falloffMult(tX,x,tZ,z,pasteregionoffset))){
											posTaken[tX,tZ]=true;
											TreeInstance newTree = new TreeInstance();
											newTree.prototypeIndex = int.Parse(param[1]);
											float treeX = (float)tX/td.size.x;
											float treeZ = (float)tZ/td.size.z;
											newTree.position = new Vector3(treeX,0,treeZ);
											newTree.widthScale = float.Parse(param[6]) + (float.Parse(param[7])*(UnityEngine.Random.value-0.75f));
											newTree.heightScale = float.Parse(param[8]) + (float.Parse(param[9])*(UnityEngine.Random.value-0.75f));
											newTree.color = teFunc.colorParse(param[4]);
											newTree.lightmapColor = teFunc.colorParse(param[5]);
											go.GetComponent<Terrain>().AddTreeInstance(newTree);
										}
									}
								}
							}
							// Get rid of tree colliders..
							float[,] tmpheights = td.GetHeights(0, 0, 0, 0);
                			td.SetHeights(0, 0, tmpheights);
						}
					}
					td.SetAlphamaps(0,0,alphadata);
				}
			}
		}
	}	
	
	public override void Generate(GameObject terrObject){}
	public override void OnGUI(){
		if(!TerrainEdge.isObjectTerrain){GUILayout.Label("Selected A Terrain..");return;}
		TerrainData terdat = TerrainEdge.selectedObject.GetComponent<Terrain>().terrainData;
		GUILayout.BeginHorizontal();
        teFoliageGroupIndex = EditorGUILayout.Popup(teFoliageGroupIndex, teFoliageGroupNames);
        teFoliageSlotIndex = EditorGUILayout.Popup(teFoliageSlotIndex, teFoliageSlotNames);
		string paramlist = teFoliageParams[teFoliageGroupIndex,teFoliageSlotIndex];
		if(paramlist==null){paramlist="0:0:0:0:0:0:0:0:0:0:0";}
		string[] param = paramlist.Split(":".ToCharArray()[0]);
		if(param[0].Length==0){
			paramlist="0:0:0:0:0:0:0:0:0:0:0";
			param = paramlist.Split(":".ToCharArray()[0]);
		}
		param[0] = EditorGUILayout.Popup(int.Parse(param[0]), teFoliageGroupOptions).ToString();
		GUILayout.EndHorizontal();
		
		if(param[0]=="0"){
			GUILayout.Label("No foliage type selected.");
		} else {
			string[] teFoliageGroupProtoNames = new string[16]; //List of available prototypes
			teFoliageGroupProtoNames = teFunc.getProtoList(param[0],terdat);
			if(""+teFoliageGroupProtoNames[0]!=""){
				param[1] = EditorGUILayout.Popup(int.Parse(param[1]), teFoliageGroupProtoNames).ToString(); // Proto menu
				if(param[0]!="3"){param[2] = teUI.floatSlider("Strength",float.Parse(param[2]),0f,1f).ToString();}
			    param[3] = teUI.floatSlider("Density",float.Parse(param[3]),0f,1f).ToString();
				if(param[0]=="3"){
					param[4] = teUI.colorPicker("Color:",param[4]);
					param[5] = teUI.colorPicker("Lightmap:",param[5]);
					param[6] = teUI.floatSlider("Width",float.Parse(param[6]),1f,4f).ToString();
					param[7] = teUI.floatSlider("Variation",float.Parse(param[7]),0f,0.9f).ToString();
					param[8] = teUI.floatSlider("Height",float.Parse(param[8]),1f,4f).ToString();
					param[9] = teUI.floatSlider("Variation",float.Parse(param[9]),0f,0.9f).ToString();			
				}
			} else {
				GUILayout.Label("No prototypes found.");	
			}
		}
		
		GUI.backgroundColor = new Color(0.9f,0.9f,1f,1f);
		GUILayout.BeginVertical(EditorStyles.textField);
		paintMode = GUILayout.Toggle(paintMode,"Enable Foliage Painting");
		if(paintMode==true){
			teFoliageBrushSize = (int)teUI.floatSlider("Brush Size:",(float)teFoliageBrushSize,1f,25f);
			GUILayout.Label("Use right-mouse button to click where you would like the foliage applied.",EditorStyles.wordWrappedLabel,GUILayout.Height(60));
		}
		GUILayout.EndVertical();
		GUI.backgroundColor = Color.white;
		paramlist = "";
		string paramlistdelimit = "";
		for(int i=0;i<10;i++){
			paramlist = paramlist + paramlistdelimit + param[i];
			paramlistdelimit = ":";
		}
		teFoliageParams[teFoliageGroupIndex,teFoliageSlotIndex] = paramlist;
	}
	

	
	
	
	
}

// === TE:MapInspector ===============================================================================


public class TEMapInspector : TEGroup
{
	public int mapViewIndex;
	public string importFolder = "C:\\";
	public int layerViewIndex;
	public Texture2D teMapInspectorImage;
	public Texture2D teMapImportedImage;
	public int tempInt = 0;
	public int terSize = 1000;
	public int terHeight = 500;
	public float[,] heights; 
	public string[] formats = new string[4] {"Raw 16-bit (Win)","Raw 16-bit (Mac)","PNG Image","OBJ Mesh"};
	public string[] exts = new string[3] {"raw","raw","png"};
	public int formatindex = 0;
	public TEMapInspector() : base("TE:MapInspector", "View/Import/Export Maps") { }
	public bool batchOpt = false;
	public string filePattern = "Tile_%X-%Z.raw";
	public float[,] flows;
	public int[] splatAppIndex = new int[4]{0,0,0,0};
	public string[] splatChannelNames = new string[5]{"Black","Red","Green","Blue","Alpha"};
	
	protected override void OnInit() {importFolder = Application.dataPath;}
	public override void Generate(GameObject terrObject){}
	public override void sceneEvents(SceneView sceneview){}

	public override void OnGUI(){
		bool autogen = false;
		bool nolayers = true;
		GUILayout.BeginVertical(EditorStyles.textField);
		int tempInt = mapViewIndex;
		if(teMapInspectorImage==null){autogen=true;}
		// Map-Type and Layer Selectors
		mapViewIndex = teUI.DropDown("Map Type:",mapViewIndex,new string[4]{"Heightmap","Splatmap","Detail","Trees"});
		if(TerrainEdge.selectedObject){
			if(TerrainEdge.selectedObject.GetComponent<Terrain>()){
				TerrainData terdata = TerrainEdge.selectedObject.GetComponent<Terrain>().terrainData;
				if(tempInt!=mapViewIndex){autogen = true; teMapInspectorImage = null;}
				if(mapViewIndex==2){
					if(terdata.detailPrototypes.Length>0){
						string[] layerOpts = new string[terdata.detailPrototypes.Length];
						for(int i=0;i<terdata.detailPrototypes.Length;i++){layerOpts[i] = "Layer "+i;}
						tempInt = layerViewIndex;
						layerViewIndex = teUI.DropDown("Layer:",layerViewIndex,layerOpts);
						if(tempInt!=layerViewIndex){ autogen = true; teMapInspectorImage = null; }
						nolayers = false;
					}
				} else if(mapViewIndex==3){
					if(terdata.treePrototypes.Length>0){
						string[] layerOpts = new string[terdata.treePrototypes.Length];
						for(int i=0;i<terdata.treePrototypes.Length;i++){layerOpts[i] = "Layer "+i;}
						tempInt = layerViewIndex;
						layerViewIndex = teUI.DropDown("Layer:",layerViewIndex,layerOpts);
						if(tempInt!=layerViewIndex){ autogen = true; teMapInspectorImage = null;}
						nolayers = false;
					}
				}
			}
		} 
		
		// Batch function controls
		if(nolayers==true){layerViewIndex = 0;}
		formatindex = teUI.DropDown("Format:",formatindex,formats);
		if(mapViewIndex>0){GUI.enabled=false;}
		GUI.backgroundColor = new Color(0.8f,0.8f,1.0f,1f);		
		GUILayout.BeginVertical(EditorStyles.textField);
		batchOpt = teUI.CheckBox("Batched:",batchOpt,"Batch import/export?");
		if(batchOpt){
			filePattern = teUI.TextBox("File Pattern:",filePattern);
			importFolder = teUI.FolderPicker("Import Folder",importFolder,"Select folder to import from");
		}
		GUILayout.EndVertical();
		GUI.backgroundColor = Color.white;
		GUI.enabled =true;
		
		// Import / Export functions
		GUILayout.BeginHorizontal();
		GUILayout.Label("Action:",GUILayout.Width(80));
		if(!batchOpt&&!TerrainEdge.isObjectTerrain){GUI.enabled=false;}
		if(mapViewIndex>1||(mapViewIndex==1&&formatindex!=2)){GUI.enabled = false;}
		if(GUILayout.Button("Import",EditorStyles.miniButton)){
			if(batchOpt&&mapViewIndex==0){batchImport();}
			if(!batchOpt&&mapViewIndex==0){teMap.ImportHeights(TerrainEdge.selectedObject,"",mapViewIndex,formatindex);}
			if(mapViewIndex==1){
				teMapImportedImage = teMap.loadSplatmap();	
			}
		}
		GUI.enabled = true;
	
		
		
		if(mapViewIndex>0&&formatindex!=2){GUI.enabled=false;} // If it's not heightmaps and the format isn't PNG, block the export button.
		if(!batchOpt&&!teFunc.isTerrain(Selection.activeGameObject)){GUI.enabled=false;} // If it's not batch mode and no terrain is selected, block the export button.
		if(mapViewIndex>0&&!teFunc.isTerrain(Selection.activeGameObject)){GUI.enabled=false;} // If it's a map type besides heightmaps and no terrain is selected, block the export button.
		if(GUILayout.Button("Export",EditorStyles.miniButton)){
			if(batchOpt&&mapViewIndex==0){batchExport();} else {
				//Individual Export
				if(mapViewIndex==0){teMap.ExportHeights(TerrainEdge.selectedObject, TerrainEdge.selectedObject.name, true, formatindex);}
				if(mapViewIndex==1){teMap.ExportSplat(TerrainEdge.selectedObject, TerrainEdge.selectedObject.name, true, formatindex);}
			}
		}	
		GUI.enabled=true;
		GUILayout.EndHorizontal();			

		// Imported Image
		if(teMapImportedImage){
			GUI.backgroundColor = new Color(0.8f,0.8f,1.0f,1f);		
			GUILayout.BeginVertical(EditorStyles.textField);
			GUILayout.Label("Imported Image - Channel Assignment");
			GUILayout.BeginHorizontal();
			GUILayout.Box(teMapImportedImage,GUILayout.Width(64),GUILayout.Height(64));
			GUILayout.BeginVertical();
			if(GUILayout.Button("Apply Splat Channels")){
				teMap.AssignImportedChannels(teMapImportedImage,splatAppIndex);
			}
			if(GUILayout.Button("Dismiss / Discard")){
				teMapImportedImage = null;
			}
			GUILayout.EndVertical();
			GUILayout.EndHorizontal();
			
			if(TerrainEdge.isObjectTerrain){
				TerrainData tdat = Selection.activeGameObject.GetComponent<Terrain>().terrainData;
				if(tdat.splatPrototypes.Length>0){
					
					string[] splatProtos = new string[tdat.splatPrototypes.Length+1];
					splatProtos[0]="(Unassigned)";
					for(int i=0;i<tdat.splatPrototypes.Length;i++){
						splatProtos[i+1] = tdat.splatPrototypes[i].texture.name;
					}
					for(int i=0;i<4;i++){
						GUILayout.BeginHorizontal();
						if(splatAppIndex[i]>tdat.splatPrototypes.Length){splatAppIndex[i]=0;}
						splatAppIndex[i] = teUI.DropDown(splatChannelNames[i]+":",splatAppIndex[i],splatProtos);
						GUILayout.EndHorizontal();
					}
	
				} else {
					GUILayout.Label("Selected terrain has no splat textures.");	
				}
			} else {
				GUILayout.Label("Selected a terrain...");	
			}

			GUILayout.EndVertical();	
		}
		
		// Preview Image
		if(teFunc.isTerrain(Selection.activeGameObject)){
			GUILayout.BeginHorizontal();
			GUILayout.Label("Preview:",GUILayout.Width(80));
			if(teMapInspectorImage){
				if(teMapInspectorImage.width<(int)(Screen.width-100f)){
					GUILayout.Box(teMapInspectorImage);
				} else {
					GUILayout.Box(teMapInspectorImage,GUILayout.Width((int)(Screen.width-100f)),GUILayout.Height((int)(Screen.width-100f)));	
				}
				GUILayout.EndHorizontal();
			} else {
				GUILayout.Box("No image to display!");
			}
		
			if(autogen==true&&TerrainEdge.isObjectTerrain){;
				teMapInspectorImage = teMap.ToTexture(TerrainEdge.selectedObject,mapViewIndex,layerViewIndex);
				teMapInspectorImage.Apply();
			}
		} else {
			GUILayout.Label("Select terrain object to see preview.");	
		}
		GUILayout.EndVertical();
	}	

	void batchImport(){
		if(filePattern.IndexOf("%X")==-1||filePattern.IndexOf("%Z")==-1){
			EditorUtility.DisplayDialog("Invalid File Pattern","The file patterns must contain %X and %Z.","OK");	
			return;
		}
		
		if(EditorUtility.DisplayDialog("Replace All Scene Terrain","The batch import function will remove terrain from your scene and create the imported terrain in new gameobjects.\n\nDo you wish to proceed?","Yes","No")){
			GameObject[] tmpgobs = FindObjectsOfType(typeof(GameObject)) as GameObject[];
	        foreach (GameObject tmpgob in tmpgobs){ 
				Terrain terrain = tmpgob.GetComponent<Terrain>();
				if(terrain){
					GameObject.DestroyImmediate(tmpgob);
				}
			}
			string patternLeft = "";
			string patternMid = "";
			string patternRight = "";
			importFolder = "C:/";
			if(filePattern.IndexOf("%Z")>filePattern.IndexOf("%X")){ // %X is first
				patternLeft = filePattern.Substring(0,filePattern.IndexOf("%X"));
				patternMid = filePattern.Substring(filePattern.IndexOf("%X")+2,(filePattern.IndexOf("%Z")-2)-filePattern.IndexOf("%X"));
				patternRight = filePattern.Substring(filePattern.IndexOf("%Z")+2,filePattern.Length-(filePattern.IndexOf("%Z")+2));
			} else {  // %Z is first
				patternLeft = filePattern.Substring(0,filePattern.IndexOf("%Z"));
				patternMid = filePattern.Substring(filePattern.IndexOf("%Z")+2,(filePattern.IndexOf("%X")-2)-filePattern.IndexOf("%Z"));
				patternRight = filePattern.Substring(filePattern.IndexOf("%X")+2,filePattern.Length-(filePattern.IndexOf("%X")+2));
			}
			foreach (string file in Directory.GetFiles(importFolder.Replace("/","\\"))){
				int c1 = file.IndexOf(patternLeft);
				int c2 = file.IndexOf(patternMid);
				int c3 = file.IndexOf(patternRight);
				if(c1>-1&&c2>=c1+patternLeft.Length&&c3>=c2+patternMid.Length){
					string v1 = file.Substring(c1+patternLeft.Length,c2-(c1+patternLeft.Length));
					string v2 = file.Substring(c2+patternMid.Length,c3-(c2+patternMid.Length));
					int gridx = int.Parse(v1);
					int gridz = int.Parse(v2);
					if(filePattern.IndexOf("%X")>filePattern.IndexOf("%Z")){ gridx = int.Parse(v2); gridz = int.Parse(v1); }
					FileInfo f = new FileInfo(file);
					int res = (int)Mathf.Sqrt((float)(f.Length/2));
					GameObject tmpTerrainObject = TerEdge.teFunc.newTerrain("Imported"+gridx+"_"+gridz,gridx,gridz,terSize,terHeight,res,res-1,res-1,res-1,8);
					teMap.ImportHeights(tmpTerrainObject,file,mapViewIndex,formatindex);
				}
			}
		}			
	}

	void batchExport(){
		GameObject[] tmpgobs = FindObjectsOfType(typeof(GameObject)) as GameObject[];
        foreach (GameObject tmpgob in tmpgobs){ 
			Terrain terrain = tmpgob.GetComponent<Terrain>();
			if(terrain){
				int gridx = (int)(tmpgob.transform.position.x/terrain.terrainData.size.x);
				int gridz = (int)(tmpgob.transform.position.z/terrain.terrainData.size.z);
				string fileout = filePattern;
				string fileoutrep1 = fileout.Replace("%X",gridx.ToString());
				string fileoutrep2 = fileoutrep1.Replace("%Z",gridz.ToString());
				EditorUtility.DisplayProgressBar("Exporting","Exporting file: "+fileoutrep2,0.5f);
				teMap.ExportHeights(tmpgob, fileoutrep2, false, formatindex);
			}
		}
		EditorUtility.ClearProgressBar();		
	}
	
}


// === TE:NoiseLab ===============================================================================
[System.SerializableAttribute] 
public class TENoiseLab : TEGroup
{
	
    public static float[] noiseFuncMin = new float[20];
    public static float[] noiseFuncMax = new float[20];
    public static int teNoiseChanIndex = 0;
    public static int[] teNoiseChanTypeIndex = new int[20];
	public static string[] teFunctionTypes = new string[18] {"Add","Subtract","Multiply","Min","Max","Blend","Clamp","Power","Curve","Terrace","Abs","Exponent","Invert","ScaleBias","Turbulence","Select","TEWarp","WindexWarp"};
    public static string[] teNoiseChannels = new string[20] {"Channel 0","Channel 1","Channel 2","Channel 3","Channel 4","Channel 5","Channel 6","Channel 7","Channel 8","Channel 9","Channel 10","Channel 11","Channel 12","Channel 13","Channel 14","Channel 15","Channel 16","Channel 17","Channel 18","Channel 19"};
    public static string[] teNoiseChannelTypes = new string[3] { "(none)", "Generator", "Function" };
    public static string[] teNoiseTypes = new string[7] { "Perlin", "Billow", "Ridged", "Voronoi" , "fBm", "HeterogeneousMultiFractal", "HybridMultiFractal"};
    public static int[] teFunctionTypeIndex = new int[20];
    public static int[] teNoiseTypeIndex = new int[20];
    public static int[] srcChannel1Id = new int[20];
    public static int[] srcChannel2Id = new int[20];
    public static int[] srcChannel3Id = new int[20];
    public static int previewRedrawTime = 0;
    public static double[] lacunarity = new double[20] {2.0,2.0,2.0,2.0,2.0,2.0,2.0,2.0,2.0,2.0,2.0,2.0,2.0,2.0,2.0,2.0,2.0,2.0,2.0,2.0};
    public static double[] persistance = new double[20] {0.5,0.5,0.5,0.5,0.5,0.5,0.5,0.5,0.5,0.5,0.5,0.5,0.5,0.5,0.5,0.5,0.5,0.5,0.5,0.5};
    public static double[] displacement = new double[20] {1.0,1.0,1.0,1.0,1.0,1.0,1.0,1.0,1.0,1.0,1.0,1.0,1.0,1.0,1.0,1.0,1.0,1.0,1.0,1.0};
    public static double[] frequency = new double[20] {2.0,2.0,2.0,2.0,2.0,2.0,2.0,2.0,2.0,2.0,2.0,2.0,2.0,2.0,2.0,2.0,2.0,2.0,2.0,2.0};
    public static bool[] distance = new bool[20];
    public static double[] exponent = new double[20];
    public static double[] offset = new double[20];
    public static double[] gain = new double[20];
	public static double[] scale = new double[20];
	public static double[] bias = new double[20];
	public static double[] power = new double[20];
	public static double[] falloff = new double[20];
    public static int[] seed = new int[20] {1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1};
    public static int[] octaves = new int[20] {6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6};
    public static float[] zoom = new float[20] {1f,1f,1f,1f,1f,1f,1f,1f,1f,1f,1f,1f,1f,1f,1f,1f,1f,1f,1f,1f};
    public static ModuleBase[] moduleBase = new ModuleBase[20];
    public static Noise2D[] m_noiseMap = new Noise2D[20];
    public static Noise2D tmpNoiseMap;
    public static Texture2D[] m_textures = new Texture2D[20];
    public static Texture2D tmpTexture;
    public static float noiseAmp = 0.5f;
    public static float alphaStrength = 1.0f;
    public static float xoffset = 0f;
    public static float yoffset = 0f;
    private static float[,] heights;
	public static float[,] cpval = new float[20,10];
	public static int[] controlpointcount = new int[20] {4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4};
	public static bool[] invertTerrace = new bool[20];
	public static bool teNoisePaint = false;
	public static int teNoisePaintMode = 0;
	public static string[] teNoisePaintModes = new string[3]{"Replace","Subtract","Add"};
	public static int teNoiseBrushSize = 10;
	
    int tempInt = 0;
    double tempDouble = 0;
    float tempFloat2 = 0;
    float tempFloat = 0;
    bool tempBool = false;

    public TENoiseLab() : base("TE:NoiseLab", "Generate terrain using combinations of the noiselib generateors and functions in up to 20 channels.") { }

	protected override void OnInit()
    {
        for (int i = 0; i < 20; i++)
        {
            teNoiseChannels[i] = "Channel " + i.ToString();
            moduleBase[i] = new Const(0.5);
            m_textures[i] = new Texture2D(64, 64);
            m_textures[i].Apply();
        }
    }

	public override void sceneEvents(SceneView sceneview){
		if(teNoisePaint==false){return;}
		if(Event.current.type==EventType.MouseDown){
			Ray ray = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition);
			RaycastHit hit = new RaycastHit();
			if(Physics.Raycast(ray, out hit, 7000.0f)){
				GameObject go = hit.collider.gameObject;
				TerrainData td = go.GetComponent<Terrain>().terrainData;
				Undo.RegisterUndo(td,"Paste Noise");
				int x = (int)hit.point.x - (int)go.transform.position.x;
				int z = (int)hit.point.z - (int)go.transform.position.z;
				int heightres = td.heightmapResolution;
				float heightMult = (1.0f / td.size.x)*(float)heightres;
				int heiX = ((int)(heightMult*(float)x))+heightres; //x:z coords for other map resolutions
				int heiZ = ((int)(heightMult*(float)z))+heightres;
				Event.current.Use();
				heights = TerrainEdge.getNeighborhoodHeights(go);
				float strengthmult =1.0f;
				int channelId = teNoiseChanIndex;
				Vector3 gopos = go.transform.position;
		        float cwidth = go.GetComponent<Terrain>().terrainData.size.x;
		        yoffset = (1f) - (gopos.x / cwidth);
		        xoffset = (-1f) + (gopos.z / cwidth);
		        tmpNoiseMap = new Noise2D(heightres*3, heightres*3, moduleBase[channelId]);
		        tmpNoiseMap.GeneratePlanar(xoffset, xoffset + (1f / heightres) * (heightres*3) + 3, -yoffset, -yoffset + (1f / heightres) * (heightres*3) + 3);
		        tmpTexture = tmpNoiseMap.GetTexture();
		        tmpTexture.Apply();
	        	int pasteregionoffset = (int)((float)teNoiseBrushSize);
				for(int paintZ=(heiZ)-pasteregionoffset;paintZ<(heiZ)+(pasteregionoffset+1);paintZ++){
					for(int paintX=(heiX)-pasteregionoffset;paintX<(heiX)+(pasteregionoffset+1);paintX++){
						if(paintZ>=0&&paintZ<=(heightres*3)-2&&paintX>=0&&paintX<=(heightres*3)-2){
							strengthmult = falloffMult(paintX,heiX,paintZ,heiZ,pasteregionoffset);
							float tmpval = (tmpNoiseMap[paintZ, paintX] + 0.5f) * noiseAmp;
							if(heights[paintZ,paintX]>tmpval){ 
								heights[paintZ,paintX]-=(heights[paintZ,paintX]-tmpval) * strengthmult;
							} else {
								heights[paintZ,paintX]+=(tmpval-heights[paintZ,paintX]) * strengthmult;
							}
						}
					}
				}
				TerrainEdge.setNeighborhoodHeights(go,heights);
			}
		}
	}

	public float falloffMult(int calcX,int centerX,int calcZ,int centerZ,int maxVal){
		int offX = calcX - centerX;
		int offZ = calcZ - centerZ;
		float retVal = 0f;
		if(offX<0){offX=0-offX;}
		if(offZ<0){offZ=0-offZ;}
		float strength = maxVal - Mathf.Sqrt( (float)(offX*offX) + (float)(offZ*offZ));
		if(strength<0f){strength=0f;}
		retVal = (0.5f/(float)maxVal) * strength;
		return TerEdge.teFunc.clampVal(retVal);
	}
	
	
	
	public override void OnGUI()
    {
        GUILayout.BeginVertical(EditorStyles.textField);
        GUILayout.BeginHorizontal();
        tempInt = teNoiseChanIndex;
        teNoiseChanIndex = EditorGUILayout.Popup(teNoiseChanIndex, teNoiseChannels);
        if (teNoiseChanIndex != tempInt) { genNoise(teNoiseChanIndex); }
        tempInt = teNoiseChanTypeIndex[teNoiseChanIndex];
        teNoiseChanTypeIndex[teNoiseChanIndex] = EditorGUILayout.Popup(teNoiseChanTypeIndex[teNoiseChanIndex], teNoiseChannelTypes);
        if (teNoiseChanTypeIndex[teNoiseChanIndex] != tempInt) { genNoise(teNoiseChanIndex); }
        GUILayout.EndHorizontal();
        if (teNoiseChanTypeIndex[teNoiseChanIndex] == 2)
        {
            GUILayout.BeginVertical(EditorStyles.textField);
            GUILayout.Label("Function Settings", EditorStyles.miniBoldLabel);
            GUILayout.BeginHorizontal();
            GUILayout.Label("Type", GUILayout.Width(80));
            tempInt = teFunctionTypeIndex[teNoiseChanIndex];
            teFunctionTypeIndex[teNoiseChanIndex] = EditorGUILayout.Popup(teFunctionTypeIndex[teNoiseChanIndex], teFunctionTypes);
            if (tempInt != teFunctionTypeIndex[teNoiseChanIndex]) { previewRedrawTime = (int)EditorApplication.timeSinceStartup + 1; }
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();
            GUILayout.Label("Source A", GUILayout.Width(80));
            tempInt = srcChannel1Id[teNoiseChanIndex];
            srcChannel1Id[teNoiseChanIndex] = EditorGUILayout.Popup(srcChannel1Id[teNoiseChanIndex], teNoiseChannels);
            if (tempInt != srcChannel1Id[teNoiseChanIndex]) { previewRedrawTime = (int)EditorApplication.timeSinceStartup + 1; }
            GUILayout.EndHorizontal();
            if (teFunctionTypeIndex[teNoiseChanIndex] < 6)
            {
                GUILayout.BeginHorizontal();
                GUILayout.Label("Source B", GUILayout.Width(80));
                tempInt = srcChannel2Id[teNoiseChanIndex];
                srcChannel2Id[teNoiseChanIndex] = EditorGUILayout.Popup(srcChannel2Id[teNoiseChanIndex], teNoiseChannels);
                if (tempInt != srcChannel2Id[teNoiseChanIndex]) { previewRedrawTime = (int)EditorApplication.timeSinceStartup + 1; }
                GUILayout.EndHorizontal();
            }
			
            if (teFunctionTypeIndex[teNoiseChanIndex] == 5||teFunctionTypeIndex[teNoiseChanIndex] == 15)
            {
                GUILayout.BeginHorizontal();
                GUILayout.Label("Controller", GUILayout.Width(80));
                tempInt = srcChannel3Id[teNoiseChanIndex];
                srcChannel3Id[teNoiseChanIndex] = EditorGUILayout.Popup(srcChannel3Id[teNoiseChanIndex], teNoiseChannels);
                if (tempInt != srcChannel3Id[teNoiseChanIndex]) { previewRedrawTime = (int)EditorApplication.timeSinceStartup + 1; }
                GUILayout.EndHorizontal();
            }
			if(teFunctionTypeIndex[teNoiseChanIndex]==11){
				tempDouble = exponent[teNoiseChanIndex];
				exponent[teNoiseChanIndex] = (double)teUI.floatSlider("Exponent:",(float)exponent[teNoiseChanIndex],0.0f,1.0f);
				if(tempDouble!=noiseFuncMin[teNoiseChanIndex]){previewRedrawTime = (int) EditorApplication.timeSinceStartup+1;}			
			}
			if(teFunctionTypeIndex[teNoiseChanIndex]==13){ // ScaleBias
				tempDouble = scale[teNoiseChanIndex];
				scale[teNoiseChanIndex] = (double)teUI.floatSlider("Scale:",(float)scale[teNoiseChanIndex],0.0f,1.0f);
				if(tempDouble!=noiseFuncMin[teNoiseChanIndex]){previewRedrawTime = (int) EditorApplication.timeSinceStartup+1;}			
				tempDouble = bias[teNoiseChanIndex];
				bias[teNoiseChanIndex] = (double)teUI.floatSlider("Bias:",(float)bias[teNoiseChanIndex],0.0f,1.0f);
				if(tempDouble!=noiseFuncMin[teNoiseChanIndex]){previewRedrawTime = (int) EditorApplication.timeSinceStartup+1;}			
			}
			if(teFunctionTypeIndex[teNoiseChanIndex]==14){
				tempDouble = power[teNoiseChanIndex];
				power[teNoiseChanIndex] = (double)teUI.floatSlider("Power:",(float)power[teNoiseChanIndex],0.0f,1.0f);
				if(tempDouble!=noiseFuncMin[teNoiseChanIndex]){previewRedrawTime = (int) EditorApplication.timeSinceStartup+1;}			
			}
			if(teFunctionTypeIndex[teNoiseChanIndex]==15){
				tempDouble = falloff[teNoiseChanIndex];
				falloff[teNoiseChanIndex] = (double)teUI.floatSlider("Falloff:",(float)falloff[teNoiseChanIndex],0.0f,1.0f);
				if(tempDouble!=noiseFuncMin[teNoiseChanIndex]){previewRedrawTime = (int) EditorApplication.timeSinceStartup+1;}			
			}
            if(teFunctionTypeIndex[teNoiseChanIndex]==6||teFunctionTypeIndex[teNoiseChanIndex] == 15)
			{
				GUILayout.BeginHorizontal();
				GUILayout.Label ("Limits", GUILayout.Width(80));
				GUILayout.Label (noiseFuncMin[teNoiseChanIndex].ToString("N2"), GUILayout.Width(40));
				tempFloat = noiseFuncMin[teNoiseChanIndex];
				tempFloat2 = noiseFuncMax[teNoiseChanIndex];
				EditorGUILayout.MinMaxSlider(ref noiseFuncMin[teNoiseChanIndex],ref noiseFuncMax[teNoiseChanIndex],0.0f,1.0f,GUILayout.MinWidth(40));
				GUILayout.Label (noiseFuncMax[teNoiseChanIndex].ToString("N2"), GUILayout.Width(40));
				if(tempFloat!=noiseFuncMin[teNoiseChanIndex]||tempFloat2!=noiseFuncMax[teNoiseChanIndex]){previewRedrawTime = (int) EditorApplication.timeSinceStartup+1;}			
				GUILayout.EndHorizontal();
			}
			if(teFunctionTypeIndex[teNoiseChanIndex]>7&&teFunctionTypeIndex[teNoiseChanIndex]<10)
			{
				GUILayout.BeginHorizontal();
				GUILayout.Label ("Control Points", GUILayout.Width(80));
				tempInt = controlpointcount[teNoiseChanIndex];
				controlpointcount[teNoiseChanIndex] = (int)GUILayout.HorizontalSlider(controlpointcount[teNoiseChanIndex],4.0f,10.0f);
				if(tempInt!=controlpointcount[teNoiseChanIndex]){previewRedrawTime = (int) EditorApplication.timeSinceStartup+1;}					
				GUILayout.Label (controlpointcount[teNoiseChanIndex].ToString("N0"));
				GUILayout.EndHorizontal();
				GUILayout.BeginHorizontal(GUILayout.MaxHeight(80));
					GUILayout.Label ("\nCurve\nControl\nPoints", GUILayout.Width(80),GUILayout.Height(70));
					for(int i=0;i<controlpointcount[teNoiseChanIndex];i++){
						GUILayout.BeginVertical(GUILayout.Width(16));
						tempFloat = cpval[teNoiseChanIndex,i];
						cpval[teNoiseChanIndex,i] = GUILayout.VerticalSlider(cpval[teNoiseChanIndex,i],-1.0f,1.0f);
						if(tempFloat!=cpval[teNoiseChanIndex,i]){previewRedrawTime = (int) EditorApplication.timeSinceStartup+1;}			
						GUI.enabled = true;
						GUILayout.EndVertical();
					}
				GUILayout.EndHorizontal();
				if(teFunctionTypeIndex[teNoiseChanIndex]==9)
				{
					tempBool = invertTerrace[teNoiseChanIndex];
					invertTerrace[teNoiseChanIndex] = GUILayout.Toggle(invertTerrace[teNoiseChanIndex],"Invert Terraces");	
					if(tempBool!=invertTerrace[teNoiseChanIndex]){previewRedrawTime = (int) EditorApplication.timeSinceStartup+1;}	
				}
				EditorGUILayout.Space();
			}
            GUILayout.EndVertical();
        }
        if (teNoiseChanTypeIndex[teNoiseChanIndex] == 1)
        {
            GUILayout.BeginVertical(EditorStyles.textField);
            GUILayout.Label("Noise Settings", EditorStyles.miniBoldLabel);
            GUILayout.BeginHorizontal();
            GUILayout.Label("Type", GUILayout.Width(80));
            tempInt = teNoiseTypeIndex[teNoiseChanIndex];
            teNoiseTypeIndex[teNoiseChanIndex] = EditorGUILayout.Popup(teNoiseTypeIndex[teNoiseChanIndex], teNoiseTypes);
            if (tempInt != teNoiseTypeIndex[teNoiseChanIndex]) { previewRedrawTime = (int)EditorApplication.timeSinceStartup + 1; }
            GUILayout.EndHorizontal();
            		
            tempInt = seed[teNoiseChanIndex];
			seed[teNoiseChanIndex] = teUI.intSlider("Seed:", seed[teNoiseChanIndex], 0, 65535);
            if (tempInt != seed[teNoiseChanIndex]) { previewRedrawTime = (int)EditorApplication.timeSinceStartup + 1; }
			
			tempDouble = frequency[teNoiseChanIndex];
			frequency[teNoiseChanIndex] = (double)teUI.floatSlider("Frequency:", (float)frequency[teNoiseChanIndex], 0.01f, 4f);
			if (tempDouble != frequency[teNoiseChanIndex]) { previewRedrawTime = (int)EditorApplication.timeSinceStartup + 1; }
			
			if (teNoiseTypeIndex[teNoiseChanIndex] == 5||teNoiseTypeIndex[teNoiseChanIndex] == 6){
				tempDouble = offset[teNoiseChanIndex];
				offset[teNoiseChanIndex] = (double)teUI.floatSlider("Offset:", (float)offset[teNoiseChanIndex], 0.01f, 1f);
				if (tempDouble != offset[teNoiseChanIndex]) { previewRedrawTime = (int)EditorApplication.timeSinceStartup + 1; }
			}
			
			if (teNoiseTypeIndex[teNoiseChanIndex] == 6) {
				tempDouble = gain[teNoiseChanIndex];
				gain[teNoiseChanIndex] = (double)teUI.floatSlider("Gain:", (float)gain[teNoiseChanIndex], 0.0f, 1f);
				if (tempDouble != gain[teNoiseChanIndex]) { previewRedrawTime = (int)EditorApplication.timeSinceStartup + 1; }
			}
			
            if (teNoiseTypeIndex[teNoiseChanIndex] != 3){ 
                tempDouble = lacunarity[teNoiseChanIndex];
                lacunarity[teNoiseChanIndex] = (double)teUI.floatSlider("Lacunarity:",(float)lacunarity[teNoiseChanIndex], 0.1f, 4.0f);
                if (tempDouble != lacunarity[teNoiseChanIndex]) { previewRedrawTime = (int)EditorApplication.timeSinceStartup + 1; }
				if (teNoiseTypeIndex[teNoiseChanIndex] != 2 && teNoiseTypeIndex[teNoiseChanIndex] < 4 ){
					tempDouble = persistance[teNoiseChanIndex];
	                persistance[teNoiseChanIndex] = (double)teUI.floatSlider("Persistance:",(float)persistance[teNoiseChanIndex], 0.1f, 1.0f);
	                if (tempDouble != persistance[teNoiseChanIndex]) { previewRedrawTime = (int)EditorApplication.timeSinceStartup + 1; }
				}
                tempInt = octaves[teNoiseChanIndex];
                octaves[teNoiseChanIndex] = teUI.intSlider("Octaves:",octaves[teNoiseChanIndex], 1, 10);
                if (tempInt != octaves[teNoiseChanIndex]) { previewRedrawTime = (int)EditorApplication.timeSinceStartup + 1; }
            } else {     
                tempDouble = displacement[teNoiseChanIndex];
                displacement[teNoiseChanIndex] = (double)teUI.floatSlider("Displacement:",(float)displacement[teNoiseChanIndex], 0.01f, 1.0f);
                if (tempDouble != displacement[teNoiseChanIndex]) { previewRedrawTime = (int)EditorApplication.timeSinceStartup + 2; }
                
                tempBool = distance[teNoiseChanIndex];
                distance[teNoiseChanIndex] = teUI.CheckBox("Distance",distance[teNoiseChanIndex],"Apply Distance");
                if (tempBool != distance[teNoiseChanIndex]) { previewRedrawTime = (int)EditorApplication.timeSinceStartup + 2; }       
            }
            GUILayout.EndVertical();
        }
        if (teNoiseChanTypeIndex[teNoiseChanIndex] > 0)
        {
            GUILayout.BeginHorizontal();
            GUILayout.BeginVertical();
            GUILayout.Label("Preview", EditorStyles.miniBoldLabel);
            if (previewRedrawTime > 0)
            {
                previewRedrawTime = 0;
                genNoise(teNoiseChanIndex);
            }
            GUILayout.Box(m_textures[teNoiseChanIndex], GUILayout.Width(64), GUILayout.Height(64));
            GUILayout.EndVertical();
            GUILayout.BeginVertical();
            GUILayout.Label("Apply Heightmaps", EditorStyles.miniBoldLabel);
            GUILayout.BeginHorizontal();
            GUILayout.Label("Height", GUILayout.Width(45));
            noiseAmp = GUILayout.HorizontalSlider(noiseAmp, 0.1f, 1.0f);
            GUILayout.Label(noiseAmp.ToString("N3"), EditorStyles.label, GUILayout.Width(55));
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();
            GUILayout.Label("Alpha", GUILayout.Width(45));
            alphaStrength = GUILayout.HorizontalSlider(alphaStrength, 0.1f, 1.0f);
            if (alphaStrength == 1.0f)
            {
                GUILayout.Label("Replace", EditorStyles.label, GUILayout.Width(55));
            }
            else
            {
                GUILayout.Label(alphaStrength.ToString("N3"), EditorStyles.label, GUILayout.Width(55));
            }
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();
            if (TerrainEdge.isObjectTerrain == true)
            {
                if (GUILayout.Button("Selected"))
                {
					Undo.RegisterUndo(TerrainEdge.selectedObject.GetComponent<Terrain>().terrainData,"te:Generate All Heightmaps"); 
                    Generate(TerrainEdge.selectedObject);
                }
            }
            else
            {
                GUI.enabled = false;
                GUILayout.Button("Selected");
                GUI.enabled = true;
            }
            if (GUILayout.Button("Full Scene"))
            {
                if (EditorUtility.DisplayDialog("Generate Full Scene", "Replace ALL terrain heightmaps in your scene?", "Ok"))
                {
					Undo.RegisterUndo(FindObjectsOfType(typeof(Terrain)) as Terrain[],"te:Generate All Heightmaps"); 
                    Terrain[] terrs = FindObjectsOfType(typeof(Terrain)) as Terrain[];
                    int terrIndex = 0;
                    foreach (Terrain terr in terrs)
                    {
                        EditorUtility.DisplayProgressBar("Generating Terrain", "Generating " + terr.gameObject.name + " (" + (terrIndex + 1) + " of " + terrs.Length + ")....", (float)(terrIndex * (1.0f / terrs.Length)));
                        Generate(terr.gameObject);
                        EditorUtility.ClearProgressBar();
                        terrIndex++;
                    }
                }
            }
            GUILayout.EndHorizontal();
            GUILayout.EndVertical();
            GUILayout.EndHorizontal();
			GUI.backgroundColor = new Color(0.9f,0.9f,1f,1f);
			GUILayout.BeginVertical(EditorStyles.textField);
			teNoisePaint = GUILayout.Toggle(teNoisePaint,"Enable Noise Painting");
			if(teNoisePaint==true){
				teNoisePaintMode = teUI.DropDown("Paint Type:",teNoisePaintMode, teNoisePaintModes);
				int maxbrushsize = 32;
				if(teFunc.isTerrain(Selection.activeGameObject)){maxbrushsize=Selection.activeGameObject.GetComponent<Terrain>().terrainData.heightmapResolution;}
				teNoiseBrushSize = teUI.intSlider("Brush Size:",teNoiseBrushSize,1,maxbrushsize);
				if(teNoiseBrushSize>maxbrushsize){teNoiseBrushSize=maxbrushsize;}
				GUILayout.Label("Use right-mouse button to click where you would like the noise paint applied.",EditorStyles.wordWrappedLabel,GUILayout.Height(60));
			}
			GUILayout.EndVertical();
			GUI.backgroundColor = Color.white;
        }
        GUILayout.EndVertical();
    }

	public static void genIt(GameObject terrObject){teFunc.generateHeightmap(terrObject,moduleBase[teNoiseChanIndex],alphaStrength,noiseAmp);}
    public override void Generate(GameObject terrObject){genIt(terrObject);}
		
    public static void genNoise(int channelId)
    {
        moduleBase[channelId] = new Perlin();
        if (teNoiseChanTypeIndex[channelId] == 1)
        {
            int tIdx = teNoiseTypeIndex[channelId];
            if (tIdx == 0) { moduleBase[channelId] = new Perlin(frequency[channelId], lacunarity[channelId], persistance[channelId], octaves[channelId], seed[channelId], QualityMode.High); }
            if (tIdx == 1) { moduleBase[channelId] = new Billow(frequency[channelId], lacunarity[channelId], persistance[channelId], octaves[channelId], seed[channelId], QualityMode.High); }
            if (tIdx == 2) { moduleBase[channelId] = new RiggedMultifractal(frequency[channelId], lacunarity[channelId], octaves[channelId], seed[channelId], QualityMode.High); }
            if (tIdx == 3) { moduleBase[channelId] = new Voronoi(frequency[channelId], displacement[channelId], seed[channelId], distance[channelId]); }
            if (tIdx == 4) { moduleBase[channelId] = new BrownianMotion(frequency[channelId], lacunarity[channelId], octaves[channelId], seed[channelId], QualityMode.High); }
            if (tIdx == 5) { moduleBase[channelId] = new HeterogeneousMultiFractal(frequency[channelId], lacunarity[channelId], octaves[channelId], persistance[channelId], seed[channelId], offset[channelId], QualityMode.High); }
            if (tIdx == 6) { moduleBase[channelId] = new HybridMulti(frequency[channelId], lacunarity[channelId], octaves[channelId], persistance[channelId], seed[channelId], offset[channelId], gain[channelId], QualityMode.High); }
        }
        if (teNoiseChanTypeIndex[channelId] == 2)
        {
            int fIdx = teFunctionTypeIndex[channelId];
            if (fIdx == 0) { moduleBase[channelId] = new Add(moduleBase[srcChannel1Id[channelId]], moduleBase[srcChannel2Id[channelId]]); }
            if (fIdx == 1) { moduleBase[channelId] = new Subtract(moduleBase[srcChannel1Id[channelId]], moduleBase[srcChannel2Id[channelId]]); }
            if (fIdx == 2) { moduleBase[channelId] = new Multiply(moduleBase[srcChannel1Id[channelId]], moduleBase[srcChannel2Id[channelId]]); }
            if (fIdx == 3) { moduleBase[channelId] = new Min(moduleBase[srcChannel1Id[channelId]], moduleBase[srcChannel2Id[channelId]]); }
            if (fIdx == 4) { moduleBase[channelId] = new Max(moduleBase[srcChannel1Id[channelId]], moduleBase[srcChannel2Id[channelId]]); }
            if (fIdx == 5) { moduleBase[channelId] = new Blend(moduleBase[srcChannel1Id[channelId]], moduleBase[srcChannel2Id[channelId]], moduleBase[srcChannel3Id[channelId]]); }
            if (fIdx == 6) { moduleBase[channelId] = new Clamp((double)noiseFuncMin[channelId], (double)noiseFuncMax[channelId], moduleBase[srcChannel1Id[channelId]]); }
			if (fIdx == 7) { moduleBase[channelId] = new Power(moduleBase[srcChannel1Id[channelId]],moduleBase[srcChannel2Id[channelId]]);}
			if (fIdx == 8) { Curve tmpCurve = new Curve(moduleBase[srcChannel1Id[channelId]]);
				double adjust = double.Parse((controlpointcount[channelId]-1).ToString())*0.5;
				for(int i=0;i<controlpointcount[channelId];i++){
					tmpCurve.Add(double.Parse(i.ToString())-adjust,(double)cpval[channelId,i]);
					moduleBase[channelId] = tmpCurve;
				}
			}
			if(fIdx==9){Terrace tmpTerrace = new Terrace(invertTerrace[channelId],moduleBase[srcChannel1Id[channelId]]);
				for(int i=0;i<controlpointcount[channelId];i++){
					tmpTerrace.Add((double)cpval[channelId,i]-0.5);
					moduleBase[channelId] = tmpTerrace;
				}
			}
            if (fIdx == 17) { moduleBase[channelId] = new WindexWarp(moduleBase[srcChannel1Id[channelId]]); }
			if (fIdx == 16) { moduleBase[channelId] = new TEWarp(moduleBase[srcChannel1Id[channelId]]); }
			if (fIdx == 15) { moduleBase[channelId] = new Select((double)noiseFuncMin[channelId], (double)noiseFuncMax[channelId], falloff[channelId],moduleBase[srcChannel1Id[channelId]],moduleBase[srcChannel3Id[channelId]]); }
			if (fIdx == 14) { moduleBase[channelId] = new Turbulence(power[channelId],moduleBase[srcChannel1Id[channelId]]); }
			if (fIdx == 13) { moduleBase[channelId] = new ScaleBias(scale[channelId],bias[channelId],moduleBase[srcChannel1Id[channelId]]); }
			if (fIdx == 12) { moduleBase[channelId] = new Invert(moduleBase[srcChannel1Id[channelId]]);}
			if (fIdx == 11) { moduleBase[channelId] = new Exponent(exponent[channelId],moduleBase[srcChannel1Id[channelId]]); }
			if (fIdx == 10) { moduleBase[channelId] = new Abs(moduleBase[srcChannel1Id[channelId]]);}
		}
        int resolution = 64;
        int xoffset = 0; int yoffset = 0;
        m_noiseMap[channelId] = new Noise2D(resolution, resolution, moduleBase[channelId]);
        float x1 = xoffset * zoom[channelId];
        float x2 = (xoffset * zoom[channelId]) + ((zoom[channelId] / resolution) * (resolution + 1));
        float y1 = -yoffset * zoom[channelId];
        float y2 = (-yoffset * zoom[channelId]) + ((zoom[channelId] / resolution) * (resolution + 1));
        m_noiseMap[channelId].GeneratePlanar(x1, x2, y1, y2);
        m_textures[channelId] = m_noiseMap[channelId].GetTexture();
        m_textures[channelId].Apply();
    }
}

// TE:Textures ===================================================================================

public class TETextures : TEGroup
{
    public static string[] teSplatTextures = new string[4] { "Texture 1", "Texture 2", "Texture 3", "Texture 4" };
    public static float[] teSplatElevationHeight = new float[12] { 0.01f, 0.1f, 0.2f, 0.3f, 0.4f, 0.5f, 0.6f, 0.7f, 0.8f, 0.9f, 0.99f,1f };
    public static int[] teSplatElevationTexId = new int[12];
    public static float[] teSplatSlopeSteepness = new float[5] { 60.0f, 70f, 80f, 90f, 90f };
    public static int[] teSplatSlopeTexId = new int[5];
	public static bool[] guiFold = new bool[8] {true,false,false,false,false,false,false,false};
	public static bool[] shaderSplatFold = new bool[8];
	public static Texture2D[] Bumps = new Texture2D[8];
	public static Texture2D[] SpecMaps = new Texture2D[8];
	public static float[] Specs = new float[8];
	public static float[] TileUVs = new float[8] {-0.25f,-0.25f,-0.25f,-0.25f,-0.25f,-0.25f,-0.25f,-0.25f};
	public static bool[] multiUV = new bool[8];
	public TETextures() : base("TE:Textures", "Apply textures to terrain using a combination of elevation/slope rules and manage replacement terrain shaders.") { }
	public static int flowTexId = 0;
	public static bool useFlowTex = false;
	public static float[] flowTexParams = new float[5]{8f,0.2f,0.18f,1.0f,45f};
	public static string[] shaderOpts = new string[4]{"None","Bump Mapped","Tri-Planar","Allegorithmic Clone"};
	public static string[] shaderOptsAdd = new string[3]{"None","Bump Mapped","Tri-Planar"};
	public static int[] shaderIndex = new int[2]{0,0}; 
	public static float mixScale = 0f;
	protected override void OnInit(){}
	public override void sceneEvents(SceneView sceneview){}
		
    public override void OnGUI()
    {
		guiFold[1] = EditorGUILayout.Foldout(guiFold[1],"Splatmap Generator");
		if(guiFold[1]){
	       GUILayout.BeginVertical(EditorStyles.textField);
	        if (TerrainEdge.isObjectTerrain == true)
	        {
	            SplatPrototype[] splatdata = TerrainEdge.selectedObject.GetComponent<Terrain>().terrainData.splatPrototypes;
	            teSplatTextures = new string[splatdata.Length+1];
	            if (splatdata.Length > 0)
	            {
	                if (splatdata.Length != teSplatTextures.Length)
	                {
	                    teSplatTextures = new string[splatdata.Length+1];
	                    for (int i = 0; i < splatdata.Length; i++)
	                    {
	                        teSplatTextures[i] = splatdata[i].texture.name;
	                    }
	                }
	                GUILayout.Label("Elevation:", EditorStyles.miniBoldLabel);
	                
					
					GUILayout.BeginHorizontal();
	                teSplatElevationTexId[0] = EditorGUILayout.Popup(teSplatElevationTexId[0], teSplatTextures, GUILayout.Width(120));
	                teSplatElevationHeight[0] = GUILayout.HorizontalSlider(teSplatElevationHeight[0], 0.0f, 1.0f);
	                GUILayout.Label(teSplatElevationHeight[0].ToString("N2"), EditorStyles.label, GUILayout.Width(40));
	                GUILayout.EndHorizontal();
					
	                GUILayout.BeginHorizontal();
	                teSplatElevationTexId[1] = EditorGUILayout.Popup(teSplatElevationTexId[1], teSplatTextures, GUILayout.Width(120));
	                teSplatElevationHeight[1] = GUILayout.HorizontalSlider(teSplatElevationHeight[1], 0.01f, 0.99f);
	                GUILayout.Label(teSplatElevationHeight[1].ToString("N2"), EditorStyles.label, GUILayout.Width(40));
	                GUILayout.EndHorizontal();
	                
					for(int hIndex=2;hIndex<10;hIndex++){
						GUILayout.BeginHorizontal();
		                if (teSplatElevationHeight[hIndex] < teSplatElevationHeight[hIndex-1]) { teSplatElevationHeight[hIndex] = teSplatElevationHeight[hIndex-1]; }
		                teSplatElevationTexId[hIndex] = EditorGUILayout.Popup(teSplatElevationTexId[hIndex], teSplatTextures, GUILayout.Width(120));
		                teSplatElevationHeight[hIndex] = GUILayout.HorizontalSlider(teSplatElevationHeight[hIndex], 0.01f, 0.99f);
		                GUILayout.Label(teSplatElevationHeight[hIndex].ToString("N2"), EditorStyles.label, GUILayout.Width(40));
		                GUILayout.EndHorizontal();
					}
					
					teSplatElevationTexId[10]=teSplatElevationTexId[9];
	                GUILayout.Label("Slope:", EditorStyles.miniBoldLabel);
	                GUILayout.BeginHorizontal();
	                GUILayout.Label("(Elevation Texture)", GUILayout.Width(120));
	                teSplatSlopeSteepness[0] = GUILayout.HorizontalSlider(teSplatSlopeSteepness[0], 0.0f, 90.0f);
	                GUILayout.Label(teSplatSlopeSteepness[0].ToString("N2"), EditorStyles.label, GUILayout.Width(40));
	                GUILayout.EndHorizontal();
	                GUILayout.BeginHorizontal();
	                if (teSplatSlopeSteepness[1] < teSplatSlopeSteepness[0]) { teSplatSlopeSteepness[1] = teSplatSlopeSteepness[0]; }
	                teSplatSlopeTexId[1] = EditorGUILayout.Popup(teSplatSlopeTexId[1], teSplatTextures, GUILayout.Width(120));
	                teSplatSlopeSteepness[1] = GUILayout.HorizontalSlider(teSplatSlopeSteepness[1], 0.0f, 90.0f);
	                GUILayout.Label(teSplatSlopeSteepness[1].ToString("N2"), EditorStyles.label, GUILayout.Width(40));
	                GUILayout.EndHorizontal();
	                GUILayout.BeginHorizontal();
	                if (teSplatSlopeSteepness[2] < teSplatSlopeSteepness[1]) { teSplatSlopeSteepness[2] = teSplatSlopeSteepness[1]; }
	                teSplatSlopeTexId[2] = EditorGUILayout.Popup(teSplatSlopeTexId[2], teSplatTextures, GUILayout.Width(120));
	                teSplatSlopeSteepness[2] = GUILayout.HorizontalSlider(teSplatSlopeSteepness[2], 0.0f, 90.0f);
	                GUILayout.Label(teSplatSlopeSteepness[2].ToString("N2"), EditorStyles.label, GUILayout.Width(40));
	                GUILayout.EndHorizontal();
	                GUILayout.BeginHorizontal();
	                if (teSplatSlopeSteepness[3] < teSplatSlopeSteepness[2]) { teSplatSlopeSteepness[3] = teSplatSlopeSteepness[2]; }
	                teSplatSlopeTexId[3] = EditorGUILayout.Popup(teSplatSlopeTexId[3], teSplatTextures, GUILayout.Width(120));
	                teSplatSlopeTexId[4] = teSplatSlopeTexId[3];  
					teSplatSlopeSteepness[3] = GUILayout.HorizontalSlider(teSplatSlopeSteepness[3], 0.0f, 90.0f);
	                GUILayout.Label(teSplatSlopeSteepness[3].ToString("N2"), EditorStyles.label, GUILayout.Width(40));
	                GUILayout.EndHorizontal();
					GUI.backgroundColor = new Color(0.9f,0.9f,1f,1f);
					GUILayout.BeginVertical(EditorStyles.textField);
					useFlowTex = GUILayout.Toggle(useFlowTex,"Flow-Map Texturing");
					if(useFlowTex==true){
		                flowTexId = EditorGUILayout.Popup(flowTexId, teSplatTextures, GUILayout.Width(120));
		                flowTexParams[0] = TerrainEdge.teFloatSlider("Iterations",flowTexParams[0],1f,100f);
						flowTexParams[3] = TerrainEdge.teFloatSlider("Initial",flowTexParams[3],0.0f,5.0f);
						flowTexParams[1] = TerrainEdge.teFloatSlider("Push Down",flowTexParams[1],0.05f,1.0f);
						flowTexParams[2] = TerrainEdge.teFloatSlider("Pull Down",flowTexParams[2],0.05f,1.0f);
						flowTexParams[4] = TerrainEdge.teFloatSlider("Min Slope",flowTexParams[4],0.05f,90.0f);
					}
					GUILayout.EndVertical();
					GUI.backgroundColor = Color.white;
						
	                GUILayout.BeginHorizontal();
	                if (GUILayout.Button("Selected"))
	                {
						Undo.RegisterUndo(Selection.activeGameObject.GetComponent<Terrain>().terrainData,"te:Generate Splatmap For Selected Terrain");
	                    Generate(Selection.activeGameObject);
	                }
	                if (GUILayout.Button("Full Scene"))
	                {
	                    Terrain[] terrs = FindObjectsOfType(typeof(Terrain)) as Terrain[];
	                    int terrIndex = 0;
	                    foreach (Terrain terr in terrs)
	                    {
	                        EditorUtility.DisplayProgressBar("Generating Splatmaps", "Generating " + terr.gameObject.name + " (" + (terrIndex + 1) + " of " + terrs.Length + ")....", (float)(terrIndex * (1.0f / terrs.Length)));
	                        Generate(terr.gameObject);
	                        terrIndex++;
	                    }
						EditorUtility.ClearProgressBar();
					}
	                GUILayout.EndHorizontal();
	            }
	            else
	            {
	                teUI.LabelBold("Selected terrain has no textures assigned.");
	            }
	        }
	        else
	        {
	            teUI.LabelBold("Select a terrain object.");
	        }
	        GUILayout.EndVertical();
		}
		EditorGUILayout.Space();
		
		
		guiFold[2] = EditorGUILayout.Foldout(guiFold[2],"Terrain Shaders");
		if(guiFold[2]){
			GUILayout.BeginVertical(EditorStyles.textField);
			if(TerrainEdge.isObjectTerrain){
				
				GUILayout.BeginHorizontal();
				GUILayout.Label("First Pass Shader:",GUILayout.Width(150));
				int tmpIndex=shaderIndex[0];
				shaderIndex[0] = EditorGUILayout.Popup(shaderIndex[0],shaderOpts);
				if(shaderIndex[0]!=tmpIndex){
					EditorUtility.DisplayProgressBar("Shaders","Rebuilding First Pass Shader...",0.5f);
					switch(shaderIndex[0]){
					case 0: teShaders.setShaders(0);break;
					case 1: teShaders.setShaders(1);break;
					case 2: teShaders.setShaders(3);break;
					case 3: teShaders.setShaders(2);break;
					}
					EditorUtility.ClearProgressBar();
				}
				GUILayout.EndHorizontal();
				GUILayout.BeginHorizontal();
				GUILayout.Label("Add Pass Shader:",GUILayout.Width(150));
				tmpIndex=shaderIndex[1];
				shaderIndex[1] = EditorGUILayout.Popup(shaderIndex[1],shaderOptsAdd);
				if(shaderIndex[1]!=tmpIndex){
					EditorUtility.DisplayProgressBar("Shaders","Rebuilding Add Pass Shader...",0.5f);
					switch(shaderIndex[1]){
					case 0: teShaders.setShaders(4);break;
					case 1: teShaders.setShaders(5);break;
					case 2: teShaders.setShaders(7);break;
					}
					EditorUtility.ClearProgressBar();
				}				
				GUILayout.EndHorizontal();
				if(shaderIndex[0]==3){
					teUI.floatSlider("Mix Scale", mixScale, 0f, 1f);	
				}
				TerrainData terdat = TerrainEdge.selectedObject.GetComponent<Terrain>().terrainData;
				int splats = terdat.splatPrototypes.Length;
				int passIndex = 0;
				if(splats>8){splats=8;}
				for(int i=0;i<splats;i++){
					if(i==4){passIndex=1;}
					shaderSplatFold[i] = EditorGUILayout.Foldout(shaderSplatFold[i],"Layer "+i+": "+terdat.splatPrototypes[i].texture.name);
					if(shaderSplatFold[i]){
						GUILayout.BeginHorizontal();
						GUILayout.Label("Diffuse",GUILayout.Width(64));
						GUI.enabled = shaderIndex[passIndex]==1||shaderIndex[passIndex]==2; //Both tri-planar and bump shader sets need bumpmaps
						GUILayout.Label("Bump",GUILayout.Width(64));
						GUI.enabled = shaderIndex[passIndex]==2; //Only tri-planar has spec maps
						GUILayout.Label("Spec",GUILayout.Width(64));
						GUI.enabled = true;
						GUILayout.EndHorizontal();
						GUILayout.BeginHorizontal();
						GUILayout.Box(terdat.splatPrototypes[i].texture,GUILayout.Width(64),GUILayout.Height(64));
						GUI.enabled = shaderIndex[passIndex]==1||shaderIndex[passIndex]==2; //Both tri-planar and bump shader sets need bumpmaps
						Bumps[i] = EditorGUILayout.ObjectField(Bumps[i],typeof(Texture2D),false,GUILayout.Height(64),GUILayout.Width(64)) as Texture2D;
						GUI.enabled = shaderIndex[passIndex]==2; //Only tri-planar has spec maps
						SpecMaps[i] = EditorGUILayout.ObjectField(SpecMaps[i],typeof(Texture2D),false,GUILayout.Height(64),GUILayout.Width(64)) as Texture2D;
						GUILayout.EndHorizontal();
						GUILayout.BeginHorizontal();
						GUI.enabled = shaderIndex[passIndex]==1; //Only bump needs spec slider
						GUILayout.Label ("Specularity", GUILayout.Width(80));
						Specs[i]=GUILayout.HorizontalSlider(Specs[i],0f,1,GUILayout.MinWidth(40));
						GUILayout.Label (Specs[i].ToString("N3"), GUILayout.Width(40));
						GUI.enabled=true;
						GUILayout.EndHorizontal();
						GUILayout.BeginHorizontal();
						multiUV[i] = GUILayout.Toggle(multiUV[i], "Multi-UV", GUILayout.Width(80));
						GUI.enabled = multiUV[i];
						TileUVs[i] = GUILayout.HorizontalSlider(TileUVs[i],-4f,4f,GUILayout.MinWidth(40));
						GUILayout.Label (TileUVs[i].ToString("N3"), GUILayout.Width(40));
						GUILayout.EndHorizontal();
						GUI.enabled = true;
						
					}
				}	
				GUI.backgroundColor=Color.white;
					
				if(GUILayout.Button("Apply Settings")){
					float[] tileUVsOut = new float[8];
					int indx = 0;
					foreach(bool mUV in multiUV){
						if(mUV==false){tileUVsOut[indx++]=1.0f;}else{tileUVsOut[indx]=TileUVs[indx++];}
					}
					teShaders.setTerrainShaderParams(Selection.activeGameObject,Bumps,SpecMaps,Specs,tileUVsOut,mixScale);
				}
			} else {
				GUILayout.Label("Select terrain object.");	
			}
			GUILayout.EndVertical();
		}
		EditorGUILayout.Space();
    }

	public static void genIt(GameObject go){
		TerrainData terdata = go.GetComponent<Terrain>().terrainData;
		float[,] heights = terdata.GetHeights(0,0,terdata.heightmapResolution,terdata.heightmapResolution);
		int alphares = terdata.alphamapResolution;
		float[,,] alphadata = terdata.GetAlphamaps(0,0,alphares,alphares);
		int splatSlot = 0;
		int splatSlotSteep = 0;
		float mult = 1.0f;
		float slopemult = 1.0f;
		float tmpSlope = 0.0f;
		for(int y=0;y<alphares;y++){
			for(int x=0;x<alphares;x++){
				float tmpHeight = (terdata.GetHeight(y,x)/terdata.size.y);
				tmpSlope = terdata.GetSteepness(((1.0f/alphares)*y)+(0.5f/alphares),((1.0f/alphares)*x)+(0.5f/alphares));
				splatSlotSteep = 0;
				if(tmpHeight<teSplatElevationHeight[0]){
					splatSlot = 0;	
				}
				for(int i=0;i<11;i++){
					if(tmpHeight>=teSplatElevationHeight[i]){
						splatSlot = i;	
					}
					if(i<4){
						if(tmpSlope>=teSplatSlopeSteepness[i]){
							splatSlotSteep = i;	
						}
					}	
				}
				if(tmpHeight>teSplatElevationHeight[10]){
					splatSlot = 10;	
				}
				if(tmpSlope>teSplatSlopeSteepness[3]){
					splatSlotSteep = 3;	
				}
				for(int i=0;i<terdata.splatPrototypes.Length;i++){
					alphadata[x,y,i]=0.0f;
				}
				mult=1.0f / (teSplatElevationHeight[splatSlot+1]-teSplatElevationHeight[splatSlot]);
				slopemult=1.0f / (teSplatSlopeSteepness[splatSlotSteep+1]-teSplatSlopeSteepness[splatSlotSteep]);
				int texFrom = teSplatElevationTexId[splatSlot];
				int texTo = teSplatElevationTexId[splatSlot+1];
				int slopeTexFrom = teSplatSlopeTexId[splatSlotSteep];
				int slopeTexTo = teSplatSlopeTexId[splatSlotSteep+1];
				if(splatSlotSteep>0){
					slopemult=1.0f / (teSplatSlopeSteepness[splatSlotSteep+1]-teSplatSlopeSteepness[splatSlotSteep]);
					float texFromAmount = (slopemult*(tmpSlope-teSplatSlopeSteepness[splatSlotSteep]));
					float texToAmount = 1.0f-texFromAmount;
					if(slopeTexFrom==slopeTexTo){texToAmount=1.0f;}
					alphadata[x,y,slopeTexTo] = texFromAmount;
					alphadata[x,y,slopeTexFrom] = texToAmount;
				} else {
					
					if(tmpSlope<teSplatSlopeSteepness[0]){
						// skip slope splatting
						if(texFrom==texTo){
							alphadata[x,y,texFrom] = 1.0f;
						} else {
							alphadata[x,y,texFrom] = 1.0f-(mult*(tmpHeight-teSplatElevationHeight[splatSlot]));
							alphadata[x,y,texTo] = mult*(tmpHeight-teSplatElevationHeight[splatSlot]);
						}
					} else {
						int slopeTex = teSplatSlopeTexId[splatSlotSteep+1];
						float slopeTexAmount = slopemult*(tmpSlope-teSplatSlopeSteepness[splatSlotSteep]);
						float texFromAmount = 1.0f-slopeTexAmount;
						float texToAmount = 1.0f-slopeTexAmount;
						if(texFrom!=texTo){
							texToAmount = mult*(tmpHeight-teSplatElevationHeight[splatSlot]);
							texFromAmount = 1.0f-texToAmount;
							texToAmount = texToAmount * (1.0f-slopeTexAmount);
							texFromAmount = texFromAmount * (1.0f-slopeTexAmount);
							if(texTo==slopeTex&&texTo!=texFrom){
								texToAmount = slopeTexAmount+texToAmount;
								slopeTexAmount = texToAmount;
							}
							if(texFrom==slopeTex&&texTo!=texFrom){
								texFromAmount = slopeTexAmount+texFromAmount;
								slopeTexAmount = texFromAmount;
							}
							
						}
						if(texFrom==slopeTex&&texTo==texFrom){
							texFromAmount = 1.0f;
							slopeTexAmount = 1.0f;
							texToAmount = 1.0f;
						}
						if(terdata.splatPrototypes.Length>texFrom){
							alphadata[x,y,texFrom]=texFromAmount;
						} else {
							Debug.Log("Alpha/Splat layer "+texFrom+" on "+go.name+" was skipped because terrain does not have that many prototypes.");
						}
						if(terdata.splatPrototypes.Length>texFrom){
							alphadata[x,y,texFrom]=texFromAmount;
						} else {
							Debug.Log("Alpha/Splat layer "+texFrom+" on "+go.name+" was skipped because terrain does not have that many prototypes.");
						}
						if(terdata.splatPrototypes.Length>texFrom){
							alphadata[x,y,texFrom]=texFromAmount;
						} else {
							Debug.Log("Alpha/Splat layer "+texFrom+" on "+go.name+" was skipped because terrain does not have that many prototypes.");
						}
						alphadata[x,y,texTo]=texToAmount;
						alphadata[x,y,slopeTex]=slopeTexAmount;
					}
				}
			}
		}
		
		if(useFlowTex){
			int heightres = TerrainEdge.selectedObject.GetComponent<Terrain>().terrainData.heightmapResolution;
			float[,] flows = new float[terdata.heightmapResolution,terdata.heightmapResolution];
			// Flow-based splatmapping :)
			for(int hY=1;hY<terdata.heightmapResolution-1;hY++){
			    for(int hX=1;hX<terdata.heightmapResolution-1;hX++){
					if(terdata.GetSteepness(hX*(1f/terdata.heightmapResolution),hY*(1f/terdata.heightmapResolution))>flowTexParams[4]){
						flows[hX,hY]=flowTexParams[3];
					}
				}
			}
			
			for(int i = 0;i<(int)flowTexParams[0];i++){ //Iterations
				EditorUtility.DisplayProgressBar("Flowmap","Flowmap generation "+i+" / "+TETextures.flowTexParams[0].ToString("N0"),(float)i*(1f/TETextures.flowTexParams[0]));
			    for(int hY=1;hY<terdata.heightmapResolution-1;hY++){
			    	for(int hX=1;hX<terdata.heightmapResolution-1;hX++){
						if(terdata.GetSteepness(hX*(1f/terdata.heightmapResolution),hY*(1f/terdata.heightmapResolution))>flowTexParams[4]){
							float cumulativedrop = 0f; 
							float nextHighest = 0f;
							float overflow = 0f;
							if(flows[hX,hY]>1f){
								overflow = (flows[hX,hY]-1f)/100f;	
							}
							float thisCellHeight = heights[hX,hY] + overflow;
							
							for(int nY = hY-1; nY < hY+2; nY++){
								for(int nX = hX-1; nX < hX+2; nX++){
									if(nY>0&&nX>0&&nX<heightres&&nY<heightres){
										if(heights[nX,nY]<thisCellHeight){
											cumulativedrop += thisCellHeight-heights[nX,nY];
										}
										if(heights[nX,nY]>thisCellHeight&&heights[nX,nY]<nextHighest){
											nextHighest = heights[nX,nY];
										}
									}
								}	
							}
							if(cumulativedrop>0f){
								for(int nY = hY-1; nY < hY+2; nY++){
									for(int nX = hX-1; nX < hX+2; nX++){
										if(nY>0&&nX>0&&nX<heightres&&nY<heightres){
											if(heights[nX,nY]<thisCellHeight){
												flows[nX,nY] += (thisCellHeight-heights[nX,nY]) * (flows[hX,hY]*(flowTexParams[1] / cumulativedrop));
												flows[hX,hY] -= (thisCellHeight-heights[nX,nY]) * (flows[hX,hY]*(flowTexParams[2] / cumulativedrop));
											}
										}
									}	
								}
							}
						}
					}
				}
			}			
			
			EditorUtility.ClearProgressBar();
			    
			for(int hY=0;hY<terdata.alphamapResolution;hY++){
			    for(int hX=0;hX<terdata.alphamapResolution;hX++){
					for(int splat=0;splat<terdata.splatPrototypes.Length; splat++){
						
						int convX = (int)(((float)hX/(float)terdata.alphamapResolution) * (float)terdata.heightmapResolution);
						int convY = (int)(((float)hY/(float)terdata.alphamapResolution) * (float)terdata.heightmapResolution);
						
						float addAmount = flows[convY,convX];
						addAmount = TerEdge.teFunc.clampVal(addAmount);
						float remainder = 1.0f-addAmount;
						// Now we tally up the total value shared by other splat channels...
						float cumulativeAmount = 0f;
						for(int i2=0;i2<terdata.splatPrototypes.Length;i2++){
							if(i2!=flowTexId){
								cumulativeAmount += alphadata[hY,hX,i2];
							}
						}
						if(cumulativeAmount>0.0f){
							float fixLayerMult = remainder / cumulativeAmount; // we multiple the other layer's splat values by this
							// Now we re-apply the splat values..
							for(int i2=0;i2<terdata.splatPrototypes.Length;i2++){
								if(i2!=flowTexId){
									alphadata[hY,hX,i2] = fixLayerMult*alphadata[hY,hX,i2];
								} else {
									alphadata[hY,hX,i2] = addAmount;
								}
							}
						} else {
							alphadata[hY,hX,flowTexId] = 1.0f;
						}
					}
				}
			}
		}
		terdata.SetAlphamaps(0,0,alphadata);
		go.GetComponent<Terrain>().Flush();		
	}
	
    public override void Generate(GameObject go){genIt(go);}
}

// TE:Tools ===========================================================================

public class TETools : TEGroup
{
	public Vector2 slopeStart;
	public static int teRampWidth = 8;
	public static int teRampFalloff = 3;
	public static int[] teToolsResSettings = new int[3] {8,8,8};
	public static int teErodeThermIter;
	public static float teErodeThermMinSlope;
	public static float teErodeThermFalloff;
	public bool[] guiFold = new bool[20];
	public static bool[] syncOptions = new bool[4] { true, true, true, true };
	private static float[,] heights;	
    public static int blendDist = 5;
    public static int blendIterations = 10;
    public static int blendSmooth = 2;
    public static bool onlyBrokenEdges = false;
    private float[,] heightsNW;
    private float[,] heightsNE;
    private float[,] heightsN;
    private float[,] heightsSW;
    private float[,] heightsSE;
    private float[,] heightsS;
    private float[,] heightsE;
    private float[,] heightsW;
	public static Vector3[,] cameraPositions = new Vector3[20,3];
	public static string[] cameraNames = new string[20];
	public static string[] cameraOptions = new string[20];
	public static int cameraIndex = 0;
	public static string curCameraName = "";
	
	public TETools() : base("TE:Tools", "Any features that didn't seem to fit into other categories! :)") { }

    public override void Generate(GameObject go){}
	protected override void OnInit(){
		for(int i=0;i<20;i++){cameraNames[i] = "(Unassigned)";}
		rebuildCameraOptions();
	}
	
	public void rebuildCameraOptions(){
		for(int i=0;i<20;i++){cameraOptions[i] = "Cam "+i+" - "+ cameraNames[i];}
	}
	
	public override void sceneEvents(SceneView sceneview){
		if(guiFold[1]==false){return;}
		if(guiFold[1]==true){
			if(Event.current.type==EventType.MouseDown){
				Ray ray = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition);
				RaycastHit hit = new RaycastHit();
				if(Physics.Raycast(ray, out hit, 7000.0f)){
					slopeStart = new Vector2(hit.point.x - (int)hit.collider.gameObject.transform.position.x,hit.point.z - (int)hit.collider.gameObject.transform.position.z);
					Event.current.Use();
				}
			}
			if(Event.current.type==EventType.MouseUp){
				Ray ray = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition);
				RaycastHit hit = new RaycastHit();
				if(Physics.Raycast(ray, out hit, 7000.0f)){
					Event.current.Use();
					doSlope(TerrainEdge.selectedObject,slopeStart,new Vector2(hit.point.x - (int)hit.collider.gameObject.transform.position.x,hit.point.z - (int)hit.collider.gameObject.transform.position.z));
				}
			}
		}
	}
	
	public override void OnGUI()
    {
		guiFold[2] = EditorGUILayout.Foldout(guiFold[2],"Thermal Erosion");
		if(guiFold[2]){
			GUILayout.BeginVertical(EditorStyles.textField);
			if(TerrainEdge.isObjectTerrain){
				GUILayout.Label ("Heightmap Erosion:",EditorStyles.miniBoldLabel);		
				teErodeThermIter = (int)TerrainEdge.teFloatSlider("Iterations",(float)teErodeThermIter,0f,50f);
				teErodeThermMinSlope = TerrainEdge.teFloatSlider("Min Slope",teErodeThermMinSlope,0f,1f);
				teErodeThermFalloff = TerrainEdge.teFloatSlider("Falloff",teErodeThermFalloff,0f,1f);		
				if(GUILayout.Button("Apply Erosion")){thermalErosion(TerrainEdge.selectedObject);}
				GUILayout.EndVertical();
			} else {
				GUILayout.Label("Select a terrain...");	
			}
			GUILayout.EndVertical();
		}
		EditorGUILayout.Space();
		guiFold[3] = EditorGUILayout.Foldout(guiFold[3],"Heightmap Render Quality");
		if(guiFold[3]){
			GUILayout.BeginVertical(EditorStyles.textField);
			GUILayout.Label ("Heightmap Render Quality:",EditorStyles.miniBoldLabel);		
			GUILayout.BeginHorizontal();
			float hmQual = 0.0f;
			if(GUILayout.Button("Low")){hmQual = 15.0f;}
			if(GUILayout.Button("Medium")){hmQual = 5.0f;}
			if(GUILayout.Button("High")){hmQual = 1.0f;}
			if(hmQual>0.0f){
				Terrain[] terrs = FindObjectsOfType(typeof(Terrain)) as Terrain[];
				foreach (Terrain terr in terrs) {
					terr.heightmapPixelError = hmQual;
				}	
			}
			GUILayout.EndHorizontal();
			GUILayout.EndVertical();
		}
		EditorGUILayout.Space();
		guiFold[4] = EditorGUILayout.Foldout(guiFold[4],"Ramps / Slopes");
		if(guiFold[4]){
			GUILayout.BeginVertical(EditorStyles.textField);
			teUI.LabelBold ("Ramps:");		
			teRampWidth = (int)TerrainEdge.teFloatSlider("Width",(float)teRampWidth,1f,60f);
			teRampFalloff = (int)TerrainEdge.teFloatSlider("Falloff",(float)teRampFalloff,0f,20f);	
			guiFold[1] = GUILayout.Toggle(guiFold[1],"Enable Ramp Tool");
			if(guiFold[1]==true){GUILayout.Label("Use right-mouse button to drag between the start and end points of slopes.  When finished, uncheck the box above to restore normal right-click functions.",EditorStyles.wordWrappedLabel,GUILayout.Height(60));}
			GUILayout.EndVertical();
		}
		EditorGUILayout.Space();
		guiFold[5] = EditorGUILayout.Foldout(guiFold[5],"Resolution");
		if(guiFold[5]){
			GUILayout.BeginVertical(EditorStyles.textField);
			if(TerrainEdge.isObjectTerrain){
				GUILayout.BeginHorizontal();
				GUILayout.BeginVertical(GUILayout.Width(60));	
				TerrainData terdat = TerrainEdge.selectedObject.GetComponent<Terrain>().terrainData;
				teUI.LabelBold ("Map:");				
				GUILayout.Label ("Detail", GUILayout.Width(50));
				GUILayout.Label ("Height", GUILayout.Width(50));
				GUILayout.Label ("Splat", GUILayout.Width(50));
				GUILayout.EndVertical();
				GUILayout.BeginVertical(GUILayout.Width(40));
				teUI.LabelBold ("Old:");
				GUILayout.Label (terdat.detailResolution.ToString());
				GUILayout.Label (terdat.heightmapResolution.ToString());
				GUILayout.Label (terdat.alphamapResolution.ToString());
				GUILayout.EndVertical();
				GUILayout.BeginVertical();
				GUILayout.Label ("",EditorStyles.miniBoldLabel);				
				for(int i=0;i<3;i++){
					teToolsResSettings[i] = (int)GUILayout.HorizontalSlider(teToolsResSettings[i],5.0f,10.0f,GUILayout.MinWidth(40));
				}
				GUILayout.EndVertical();
				GUILayout.BeginVertical(GUILayout.Width(40));
				teUI.LabelBold ("New:");
				for(int i=0;i<3;i++){
					double tmpResOut = Mathf.Pow(2,teToolsResSettings[i]);
					if(i==1){ tmpResOut=tmpResOut+1; }
					GUILayout.Label (tmpResOut.ToString(), GUILayout.Width(40));
				}
				GUILayout.EndVertical();	
				GUILayout.EndHorizontal();
				GUI.enabled = TerrainEdge.isObjectTerrain;
				if(GUILayout.Button("Selected")){
					string confirmMsg = "";
					if(terdat.detailResolution!=teToolsResSettings[0]){confirmMsg = confirmMsg + "\n- Detail";}
					if(terdat.heightmapResolution!=teToolsResSettings[1]){confirmMsg = confirmMsg + "\n- Height";}
					if(terdat.alphamapResolution!=teToolsResSettings[2]){confirmMsg = confirmMsg + "\n- Splat";}
					if(confirmMsg==""){
						EditorUtility.DisplayDialog("Resolutions already match","The resolutions you have specified are already identical to the current terrain and so no action has been taken.","Okay");
					} else {
						Undo.RegisterUndo(terdat,"te: Adjust Terrain Resolution");
						teFunc.reRes(TerrainEdge.selectedObject,(int)Mathf.Pow(2.0f,(float)teToolsResSettings[0]),1+(int)Mathf.Pow(2.0f,(float)teToolsResSettings[1]),(int)Mathf.Pow(2.0f,(float)teToolsResSettings[2]));
					}
				}
				GUI.enabled = true;
			} else {
				GUILayout.Label("Select a terrain...");	
			}
			GUILayout.EndVertical();
		}
		EditorGUILayout.Space();
		guiFold[6] = EditorGUILayout.Foldout(guiFold[6],"Synchronize Terrain");
		if(guiFold[6]){
			GUILayout.BeginVertical(EditorStyles.textField);
			if (TerrainEdge.isObjectTerrain == true)
	        {
	            teUI.LabelBold("Terrain Synchronization");
	            GUILayout.BeginHorizontal();
	            GUILayout.BeginVertical(GUILayout.Width(70));
	            syncOptions[0] = GUILayout.Toggle(syncOptions[0], "Settings");
	            syncOptions[1] = GUILayout.Toggle(syncOptions[1], "Textures");
	            GUILayout.EndVertical();
	            GUILayout.BeginVertical(GUILayout.Width(60));
	            syncOptions[2] = GUILayout.Toggle(syncOptions[2], "Trees");
	            syncOptions[3] = GUILayout.Toggle(syncOptions[3], "Detail");
	            GUILayout.EndVertical();
	            if (GUILayout.Button("Sync", GUILayout.Height(34))){teFunc.doSync(syncOptions[0],syncOptions[1],syncOptions[2],syncOptions[3]);}
	            GUILayout.EndHorizontal();
	        }
	        else
	        {
	            GUILayout.Label("Select a terrain...");
	        }
			GUILayout.EndVertical();
		}
		EditorGUILayout.Space();
		guiFold[7] = EditorGUILayout.Foldout(guiFold[7],"Fix Edges/Seams");
		if(guiFold[7]){
			GUILayout.BeginVertical(EditorStyles.textField);
			GUILayout.Label("Fix / Join / Smooth", EditorStyles.miniBoldLabel);
	        GUILayout.BeginHorizontal();
	        GUILayout.Label("Distance", GUILayout.Width(100));
	        blendDist = (int)GUILayout.HorizontalSlider((float)blendDist, 1.0f, (float)32.0f);
	        GUILayout.Label(blendDist.ToString() + "  ".Substring(0, 2), EditorStyles.label, GUILayout.Width(40));
	        GUILayout.EndHorizontal();
	        GUILayout.BeginHorizontal();
	        GUILayout.Label("Iterations", GUILayout.Width(100));
	        blendIterations = (int)GUILayout.HorizontalSlider((float)blendIterations, 1.0f, 20.0f);
	        GUILayout.Label(blendIterations.ToString() + "  ".Substring(0, 2), EditorStyles.label, GUILayout.Width(40));
	        GUILayout.EndHorizontal();
	        GUILayout.BeginHorizontal();
	        GUILayout.Label("Smoothing", GUILayout.Width(100));
	        blendSmooth = (int)GUILayout.HorizontalSlider((float)blendSmooth, 0f, 3.0f);
	        GUILayout.Label(blendSmooth.ToString() + "  ".Substring(0, 2), EditorStyles.label, GUILayout.Width(40));
	        GUILayout.EndHorizontal();
	
	        GUILayout.BeginHorizontal();
	        GUILayout.Label("", GUILayout.Width(100));
	        onlyBrokenEdges = GUILayout.Toggle(onlyBrokenEdges, "Broken edges only");
	        GUILayout.EndHorizontal();
	        GUILayout.BeginHorizontal();
	        if (TerrainEdge.isObjectTerrain == true)
	        {
	            if (GUILayout.Button("Selected"))
	            {
					Undo.RegisterUndo(FindObjectsOfType(typeof(Terrain)) as Terrain[],"te:Join Selected To Neighbors"); 
	                EditorUtility.DisplayProgressBar("Blending Terrain", "Blending " + TerrainEdge.selectedObject.name + "..", 0.5f);
	                fixSeams(TerrainEdge.selectedObject);
	                EditorUtility.ClearProgressBar();
	            }
	        }
	        else
	        {
	            GUI.enabled = false;
	            GUILayout.Button("Selected");
	            GUI.enabled = true;
	        }
	        if (GUILayout.Button("Full Scene"))
	        {
	   			Undo.RegisterUndo(FindObjectsOfType(typeof(Terrain)) as Terrain[],"te:Generate All Terrain Trees"); 
	            Terrain[] terrs = FindObjectsOfType(typeof(Terrain)) as Terrain[];
	            int terrIndex = 0;
	            foreach (Terrain terr in terrs)
	            {
	                EditorUtility.DisplayProgressBar("Repairing Terrain", "Checking/fixing " + terr.gameObject.name + " (" + (terrIndex + 1) + " of " + terrs.Length + ")....", (float)(terrIndex * (1.0f / terrs.Length)));
	                fixSeams(terr.gameObject);
	                EditorUtility.ClearProgressBar();
	                terrIndex++;
	            }
	        }
	        GUILayout.EndHorizontal();
		}
		EditorGUILayout.Space();
		guiFold[8] = EditorGUILayout.Foldout(guiFold[8],"Combine/Split Terrains");
		if(guiFold[8]){
			GUILayout.BeginVertical(EditorStyles.textField);
			if(Selection.gameObjects.Length==1){
				if(GUILayout.Button("Split Selected")){teFunc.splitSelected();}
			}
			if(Selection.gameObjects.Length==4){
				if(GUILayout.Button("Combine Selected")){teFunc.combineSelected();}	
			}
			if(Selection.gameObjects.Length!=4&&Selection.gameObjects.Length!=1){
				GUILayout.Label("Select either one or four terrains.");
			}
			GUILayout.EndVertical();
		}
		EditorGUILayout.Space();
		guiFold[9] = EditorGUILayout.Foldout(guiFold[9],"Editor Camera Assistant");
		if(guiFold[9]){
			GUILayout.BeginVertical(EditorStyles.textField);
			
			cameraIndex = EditorGUILayout.Popup(cameraIndex, cameraOptions);
			GUILayout.BeginHorizontal();
			GUILayout.Label("Name:",GUILayout.Width(40));
			curCameraName = GUILayout.TextField(curCameraName);
			if(GUILayout.Button("Save",GUILayout.Width(60))){
				cameraPositions[cameraIndex,1]=SceneView.lastActiveSceneView.rotation.eulerAngles;
				cameraPositions[cameraIndex,0]=SceneView.lastActiveSceneView.pivot;
				cameraNames[cameraIndex]=curCameraName;
				rebuildCameraOptions();	
			}
			if(GUILayout.Button("Load",GUILayout.Width(60))){
				SceneView.lastActiveSceneView.rotation = Quaternion.Euler(cameraPositions[cameraIndex,1]);	
				SceneView.lastActiveSceneView.pivot = cameraPositions[cameraIndex,0];
				curCameraName = cameraNames[cameraIndex];
				SceneView.RepaintAll();
			}
			GUILayout.EndHorizontal();
			GUILayout.EndVertical();
		}
		//if(GUILayout.Button("Create A Plane Mesh")){ --- teMesh.cs no released yet!! :)
			//teMesh.Generate("testPlane",200f,200f,32,32,true);	
		//}
    }

    void fixSeams(GameObject go)
    {
        TerrainEdge.tileW = null; TerrainEdge.tileE = null; TerrainEdge.tileS = null; TerrainEdge.tileN = null;
        TerrainEdge.tileNW = null; TerrainEdge.tileNE = null; TerrainEdge.tileSW = null; TerrainEdge.tileSE = null;
        int hwidth = go.GetComponent<Terrain>().terrainData.heightmapResolution;
		TerrainEdge.getNeighbors(go);
        heights = go.GetComponent<Terrain>().terrainData.GetHeights(0, 0, hwidth, hwidth);
        if (TerrainEdge.tileNW != null) { heightsNW = TerrainEdge.tileNW.GetComponent<Terrain>().terrainData.GetHeights(0, 0, hwidth, hwidth); }
        if (TerrainEdge.tileN != null) { heightsN = TerrainEdge.tileN.GetComponent<Terrain>().terrainData.GetHeights(0, 0, hwidth, hwidth); }
        if (TerrainEdge.tileNE != null) { heightsNE = TerrainEdge.tileNE.GetComponent<Terrain>().terrainData.GetHeights(0, 0, hwidth, hwidth); }
        if (TerrainEdge.tileW != null) { heightsW = TerrainEdge.tileW.GetComponent<Terrain>().terrainData.GetHeights(0, 0, hwidth, hwidth); }
        if (TerrainEdge.tileE != null) { heightsE = TerrainEdge.tileE.GetComponent<Terrain>().terrainData.GetHeights(0, 0, hwidth, hwidth); }
        if (TerrainEdge.tileSW != null) { heightsSW = TerrainEdge.tileSW.GetComponent<Terrain>().terrainData.GetHeights(0, 0, hwidth, hwidth); }
        if (TerrainEdge.tileS != null) { heightsS = TerrainEdge.tileS.GetComponent<Terrain>().terrainData.GetHeights(0, 0, hwidth, hwidth); }
        if (TerrainEdge.tileSE != null) { heightsSE = TerrainEdge.tileSE.GetComponent<Terrain>().terrainData.GetHeights(0, 0, hwidth, hwidth); }
        float[,] hmapraw = new float[1 + ((hwidth - 1) * 3), 1 + ((hwidth - 1) * 3)];
        if (TerrainEdge.tileNW != null) { for (int hY = 0; hY < hwidth; hY++) { for (int hX = 0; hX < hwidth; hX++) { hmapraw[hX, hY] = heightsNW[hX, (hwidth - 1) - hY]; } } }
        if (TerrainEdge.tileN != null) { for (int hY = 0; hY < hwidth; hY++) { for (int hX = 0; hX < hwidth; hX++) { hmapraw[hX + (hwidth - 1), hY] = heightsN[hX, (hwidth - 1) - hY]; } } }
        if (TerrainEdge.tileNE != null) { for (int hY = 0; hY < hwidth; hY++) { for (int hX = 0; hX < hwidth; hX++) { hmapraw[((hwidth - 1) * 2) + hX, hY] = heightsNE[hX, (hwidth - 1) - hY]; } } }
        if (TerrainEdge.tileW != null) { for (int hY = 0; hY < hwidth; hY++) { for (int hX = 0; hX < hwidth; hX++) { hmapraw[hX, (hwidth - 1) + hY] = heightsW[hX, (hwidth - 1) - hY]; } } }
        if (TerrainEdge.tileE != null) { for (int hY = 0; hY < hwidth; hY++) { for (int hX = 0; hX < hwidth; hX++) { hmapraw[((hwidth - 1) * 2) + hX, (hwidth - 1) + hY] = heightsE[hX, (hwidth - 1) - hY]; } } }
        if (TerrainEdge.tileSW != null) { for (int hY = 0; hY < hwidth; hY++) { for (int hX = 0; hX < hwidth; hX++) { hmapraw[hX, hY + ((hwidth - 1) * 2)] = heightsSW[hX, (hwidth - 1) - hY]; } } }
        if (TerrainEdge.tileS != null) { for (int hY = 0; hY < hwidth; hY++) { for (int hX = 0; hX < hwidth; hX++) { hmapraw[(hwidth - 1) + hX, hY + ((hwidth - 1) * 2)] = heightsS[hX, (hwidth - 1) - hY]; } } }
        if (TerrainEdge.tileSE != null) { for (int hY = 0; hY < hwidth; hY++) { for (int hX = 0; hX < hwidth; hX++) { hmapraw[hX + (hwidth - 1), hY] = heightsSE[hX, (hwidth - 1) - hY]; } } }
        for (int hY = 0; hY < hwidth; hY++) { for (int hX = 0; hX < hwidth; hX++) { hmapraw[hX + (hwidth - 1), hY + (hwidth - 1)] = heights[hX, (hwidth - 1) - hY]; } };
        string stateS = "connected"; string stateN = "connected"; string stateW = "connected"; string stateE = "connected";
        if (onlyBrokenEdges == true)
        {
            for (int hX = 0; hX < hwidth; hX++)
            {
                if (TerrainEdge.tileS != null) { if (heights[hX, 0] != heightsS[hX, hwidth - 1]) { stateS = "broken"; } }
                if (TerrainEdge.tileN != null) { if (heights[hX, hwidth - 1] != heightsN[hX, 0]) { stateN = "broken"; } }
                if (TerrainEdge.tileW != null) { if (heights[0, hX] != heightsW[hwidth - 1, hX]) { stateW = "broken"; } }
                if (TerrainEdge.tileE != null) { if (heights[hwidth - 1, hX] != heightsE[0, hX]) { stateE = "broken"; } }
            }
        }
        else
        {
            stateS = "broken"; stateN = "broken"; stateE = "broken"; stateW = "broken";
        }
        for (float iters = 0.0f; iters < blendIterations; iters++)
        {
            for (int hY = (hwidth - 1); hY < ((hwidth - 1) * 2); hY++)
            {
                for (int hX = -blendDist; hX < blendDist; hX++)
                {
                    float tally = 0; float tally2 = 0;
                    float tally3 = 0; float tally4 = 0;
                    float avg = 0; float avg2 = 0;
                    float avg3 = 0; float avg4 = 0;
                    for (int sX = -blendSmooth; sX < blendSmooth + 1; sX++)
                    {
                        if (stateW == "broken") { tally = tally + hmapraw[(hwidth - 1) - (hX + sX), hY]; }
                        if (stateE == "broken") { tally2 = tally2 + hmapraw[((hwidth - 1) * 2) - (hX + sX), hY]; }
                        if (stateN == "broken") { tally3 = tally3 + hmapraw[hY, (hwidth - 1) - (hX + sX)]; }
                        if (stateS == "broken") { tally4 = tally4 + hmapraw[hY, ((hwidth - 1) * 2) - (hX + sX)]; }
                    }
                    avg = tally / ((blendSmooth * 2) + 1);
                    avg2 = tally2 / ((blendSmooth * 2) + 1);
                    avg3 = tally3 / ((blendSmooth * 2) + 1);
                    avg4 = tally4 / ((blendSmooth * 2) + 1);
                    if (stateW == "broken") { hmapraw[(hwidth - 1) - hX, hY] = avg; } //(influence*avg)      + ((1.0f-influence)*hmapraw[(hwidth-1)-hX,hY]);
                    if (stateE == "broken") { hmapraw[((hwidth - 1) * 2) - hX, hY] = avg2; } //(influence*avg2) + ((1.0f-influence)*hmapraw[((hwidth*2)-1)-hX,hY]);
                    if (stateN == "broken") { hmapraw[hY, (hwidth - 1) - hX] = avg3; } //(influence*avg3) + ((1.0f-influence)*hmapraw[hY,(hwidth-1)-hX]);
                    if (stateS == "broken") { hmapraw[hY, ((hwidth - 1) * 2) - hX] = avg4; } //(influence*avg4) + ((1.0f-influence)*hmapraw[hY,((hwidth*2)-1)-hX]);
                }
            }
        }
        if (onlyBrokenEdges == true && (stateW != "broken" && stateE != "broken" && stateN != "broken" && stateS != "broken"))
        {
            // No broken edges to fix.
        }
        else
        {
            if (TerrainEdge.tileNW != null) { for (int hY = 0; hY < hwidth; hY++) { for (int hX = 0; hX < hwidth; hX++) { heightsNW[hX, (hwidth - 1) - hY] = hmapraw[hX, hY]; } } TerrainEdge.tileNW.GetComponent<Terrain>().terrainData.SetHeights(0, 0, heightsNW); }
            if (TerrainEdge.tileN != null) { for (int hY = 0; hY < hwidth; hY++) { for (int hX = 0; hX < hwidth; hX++) { heightsN[hX, (hwidth - 1) - hY] = hmapraw[hX + (hwidth - 1), hY]; } } TerrainEdge.tileN.GetComponent<Terrain>().terrainData.SetHeights(0, 0, heightsN); }
            if (TerrainEdge.tileNE != null) { for (int hY = 0; hY < hwidth; hY++) { for (int hX = 0; hX < hwidth; hX++) { heightsNE[hX, (hwidth - 1) - hY] = hmapraw[((hwidth - 1) * 2) + hX, hY]; } } TerrainEdge.tileNE.GetComponent<Terrain>().terrainData.SetHeights(0, 0, heightsNE); }
            if (TerrainEdge.tileW != null) { for (int hY = 0; hY < hwidth; hY++) { for (int hX = 0; hX < hwidth; hX++) { heightsW[hX, (hwidth - 1) - hY] = hmapraw[hX, (hwidth - 1) + hY]; } } TerrainEdge.tileW.GetComponent<Terrain>().terrainData.SetHeights(0, 0, heightsW); }
            if (TerrainEdge.tileE != null) { for (int hY = 0; hY < hwidth; hY++) { for (int hX = 0; hX < hwidth; hX++) { heightsE[hX, (hwidth - 1) - hY] = hmapraw[((hwidth - 1) * 2) + hX, (hwidth - 1) + hY]; } } TerrainEdge.tileE.GetComponent<Terrain>().terrainData.SetHeights(0, 0, heightsE); }
            if (TerrainEdge.tileSW != null) { for (int hY = 0; hY < hwidth; hY++) { for (int hX = 0; hX < hwidth; hX++) { heightsSW[hX, (hwidth - 1) - hY] = hmapraw[hX, hY + ((hwidth - 1) * 2)]; } } TerrainEdge.tileSW.GetComponent<Terrain>().terrainData.SetHeights(0, 0, heightsSW); }
            if (TerrainEdge.tileS != null) { for (int hY = 0; hY < hwidth; hY++) { for (int hX = 0; hX < hwidth; hX++) { heightsS[hX, (hwidth - 1) - hY] = hmapraw[(hwidth - 1) + hX, hY + ((hwidth - 1) * 2)]; } } TerrainEdge.tileS.GetComponent<Terrain>().terrainData.SetHeights(0, 0, heightsS); }
            if (TerrainEdge.tileSE != null) { for (int hY = 0; hY < hwidth; hY++) { for (int hX = 0; hX < hwidth; hX++) { heightsSE[hX, (hwidth - 1) - hY] = hmapraw[hX + (hwidth - 1), hY]; } } TerrainEdge.tileSE.GetComponent<Terrain>().terrainData.SetHeights(0, 0, heightsSE); }
            for (int hY = 0; hY < hwidth; hY++) { for (int hX = 0; hX < hwidth; hX++) { heights[hX, (hwidth - 1) - hY] = hmapraw[hX + (hwidth - 1), hY + (hwidth - 1)]; } } go.GetComponent<Terrain>().terrainData.SetHeights(0, 0, heights);
        }
    }	
	
	
	void doSlope(GameObject go,Vector2 slopeStart, Vector2 slopeEnd){
		TerrainData tdat = go.GetComponent<Terrain>().terrainData;
		Undo.RegisterUndo(tdat,"Create Slope");
		int res = tdat.heightmapResolution;
		heights = tdat.GetHeights(0,0,res,res);
		float mult = (1.0f / tdat.size.x) * (float)res;
		int y1 = (int)(mult * (float)slopeStart.x);
		int y2 = (int)(mult * (float)slopeEnd.x);
		int x1 = (int)(mult * (float)slopeStart.y);
		int x2 = (int)(mult * (float)slopeEnd.y);
		float xdiff = 0; float ydiff=0;
		float h1 = heights[x1,y1];
		float h2 = heights[x2,y2];
		if(x1>x2){xdiff=x1-x2;}else{xdiff=x2-x1;}
		if(y1>y2){ydiff=y1-y2;}else{ydiff=y2-y1;}
		int steps = (int)(Mathf.Sqrt((xdiff*xdiff) + (ydiff*ydiff)));
		steps = steps * 5;
		if(steps<1){steps=1;}
		float xstep = (float)(x2-x1)/(float)steps;
		float ystep = (float)(y2-y1)/(float)steps;
		float hstep = (float)(h2-h1)/(float)steps;
		int posx = 0;
		int posy = 0;
		float foX = 0f;
		float foY = 0f;
		// iterate through each step
		for(int i=0 ;i<steps;i++){
			int tmpX = (int)(x1 + (xstep*(float)i));
			int tmpY = (int)(y1 + (ystep*(float)i));
			float tmpH = h1 + (hstep*(float)i);
			for(int y=-(teRampWidth+teRampFalloff);y<teRampWidth+teRampFalloff;y++){
				for(int x=-(teRampWidth+teRampFalloff);x<teRampWidth+teRampFalloff;x++){
					if(x<0){posx = 0-x;}else{posx=x;}
					if(y<0){posy = 0-y;}else{posy=y;}
					posx = posx - teRampWidth; posy = posy - teRampWidth;
					foX = 0f;
					foY = 0f;
					if(teRampFalloff>0){
						foX = (1f/(float)teRampFalloff) * (float)posx;
						foY = (1f/(float)teRampFalloff) * (float)posx;
					}
					float falloff = foX+foY;
					falloff = falloff * 0.5f;
					if(falloff>0.0f&&falloff<1.0f){
						heights[tmpX+x,tmpY+y]=(falloff * heights[tmpX+x,tmpY+y]) + (tmpH*(1.0f-falloff));
					}
					if(falloff<=0){heights[tmpX+x,tmpY+y]=tmpH;}
				}
			}
		}
		tdat.SetHeights(0,0,heights);
	}
	
	void thermalErosion(GameObject go) {
		// Have used Terrain Toolkit's source as reference for the math/algorithms in this function.
		// As such, Sandor Moldan of 6x0 really deserves the credit for it.
		TerrainData terdata = go.GetComponent<Terrain>().terrainData;
		Undo.RegisterUndo(terdata,"Apply Thermal Erosion");				
		int res = terdata.heightmapResolution;
		int terWidth = terdata.heightmapWidth; 
		int terHeight = terdata.heightmapHeight;
		float[,] heightmap = terdata.GetHeights(0,0,res,res);
		float[,] diffdata = new float[terdata.heightmapWidth,terdata.heightmapHeight];
		float slopeMin = ((terdata.size.x / terWidth) * Mathf.Tan(teErodeThermMinSlope * Mathf.Deg2Rad)) / terdata.size.y;
		if (slopeMin > 1.0f) {slopeMin = 1.0f;}
		if (teErodeThermFalloff == 1.0f) {teErodeThermFalloff = 0.999f;}
		float slopeMax = slopeMin + ((90 - slopeMin) * teErodeThermFalloff);
		if (slopeMax > 1.0f) {slopeMax = 1.0f;}
		for (int iter = 0; iter < teErodeThermIter; iter++) {
			for (int y = 0; y < terHeight; y++) {
				int yI = 1; int yN=2; int yOff=-1;
				if(y==0){yOff=0; yI=0;}else if(y!=terHeight-1){yN=3; yOff=-1;}
				for (int x = 0; x < terWidth; x++) {
					int xN=2; int xOff=-1; int xI=1;
					if (x==0) {xOff = 0; xI = 0;}else if(x!=terWidth-1){xN=3;}
					float indexHeight = heightmap[x + xI + xOff, y + yI + yOff];
					float hMin = 1f; float hMax = 0f; float hTotal = 0f; int totalNeighbors = 0;
					for (int Ny = 0; Ny < yN; Ny++) {
						for (int Nx = 0; Nx < xN; Nx++) {
							if ((Nx != xI || Ny != yI)&&(Nx == xI || Ny == yI)) {
								float hdiff = indexHeight - heightmap[x + Nx + xOff, y + Ny + yOff];
								if (hdiff > 0) {
									hTotal += hdiff;
									if (hdiff < hMin) {hMin = hdiff;}
									if (hdiff > hMax) {hMax = hdiff;}
								}
								totalNeighbors++;
							}
						}
					}
					float avgdiff = hTotal / totalNeighbors;
					if (avgdiff >= slopeMin) {
						float blendAmount = 1f;
						if (avgdiff <= slopeMax) {blendAmount = (avgdiff - slopeMin) / (slopeMax - slopeMin);}
						diffdata[x + xI + xOff, y + yI + yOff] = diffdata[x + xI + xOff, y + yI + yOff] - (hMin / 2 * blendAmount);
						for (int Ny = 0; Ny < yN; Ny++) {
							for (int Nx = 0; Nx < xN; Nx++) {
								if ((Nx != xI || Ny != yI)&&(Nx == xI || Ny == yI)) {
									if ((heightmap[x + xI + xOff, y + yI + yOff] - heightmap[x + Nx + xOff, y + Ny + yOff]) > 0) {diffdata[x + Nx + xOff, y + Ny + yOff] = (diffdata[x + Nx + xOff, y + Ny + yOff]) + ((hMin / 2 * blendAmount) * (heightmap[x + xI + xOff, y + yI + yOff] - heightmap[x + Nx + xOff, y + Ny + yOff]) / hTotal);}
								}
							}
						}
					}
				}
			}
			for (int y=0;y<terHeight;y++) {
				for (int x=0; x<terWidth; x++) {
					heightmap[x,y] = TerEdge.teFunc.clampVal(heightmap[x,y] + diffdata[x,y]);
				}
			}
		}
		terdata.SetHeights(0,0,heightmap);
	}	
	
	
	
}

// TE:Trees ==================================================================================

 public class TETrees : TEGroup
{
	private static List<TreeInstance> TreeInstances;
    public static int[] teTreeRuleProtoId = new int[10];
    public static float[,] teTreeRuleParams = new float[10, 8];
    public static string[] teTreeRules = new string[10] {"Tree Rule 1","Tree Rule 2","Tree Rule 3","Tree Rule 4","Tree Rule 5","Tree Rule 6","Tree Rule 7","Tree Rule 8","Tree Rule 9","Tree Rule 10"};
    public static int teTreeRuleIndex = 0;
	public static Color[] teTreeColor = new Color[10] {Color.white,Color.white,Color.white,Color.white,Color.white,Color.white,Color.white,Color.white,Color.white,Color.white};
	public static Color[] teTreeLightmapColor = new Color[10] {Color.white,Color.white,Color.white,Color.white,Color.white,Color.white,Color.white,Color.white,Color.white,Color.white};
	public static float[] teTreeHeight = new float[10] {1f,1f,1f,1f,1f,1f,1f,1f,1f,1f};
	public static float[] teTreeWidth = new float[10] {1f,1f,1f,1f,1f,1f,1f,1f,1f,1f};	
	public static float[] teTreeHeightVariation = new float[10];
	public static float[] teTreeWidthVariation = new float[10];
	public static string[,,] tfRuleParams = new string[10,10,12];
	public static string[] tfRules = new string[10] {"Rule 0","Rule 1","Rule 2","Rule 3","Rule 4","Rule 5","Rule 6","Rule 7","Rule 8","Rule 9"};
	public static int tfRuleIndex = 0;
	public static string[] tfRuleTypes = new string[4] {"None","Detail","Splat","Tree"};

    public TETrees() : base("TE:Trees", "Generate trees for your terrains with a stack of 10 height/slope rules.") { }

	public override void sceneEvents(SceneView sceneview){}	
	protected override void OnInit()
    {
        for (int i = 0; i < 10; i++){
            teTreeRuleParams[i, 1] = 1.0f;
            teTreeRuleParams[i, 3] = 90.0f;
            teTreeRuleParams[i, 6] = 0.5f;
			for ( int ii = 0; ii < 10; ii++ ) {
				tfRuleParams[i,ii,2] = "0.0";
				tfRuleParams[i,ii,3] = "0.0";
				tfRuleParams[i,ii,4] = Color.white.ToString();
				tfRuleParams[i,ii,5] = Color.white.ToString();
				tfRuleParams[i,ii,6] = "0.0";
				tfRuleParams[i,ii,7] = "0.0";
				tfRuleParams[i,ii,8] = "0.0";
				tfRuleParams[i,ii,9] = "0.0";
				tfRuleParams[i,ii,10] = "0.0";
			}
		}
    }
	
    public override void Generate(GameObject go)
    {
			genIt(go);
    }
	
	public override void OnGUI()
    {
        GUILayout.BeginVertical(EditorStyles.textField);

        if (TerrainEdge.isObjectTerrain == true)
        {
			GUILayout.BeginHorizontal();
			int prevTreeRuleIndex = teTreeRuleIndex;
			teTreeRuleIndex = EditorGUILayout.Popup(teTreeRuleIndex,teTreeRules);
			if(prevTreeRuleIndex!=teTreeRuleIndex){
				tfRuleIndex = 0;	
			}
			TreePrototype[] treedata = TerrainEdge.selectedObject.GetComponent<Terrain>().terrainData.treePrototypes;
			string[] teTreeOptions = new string[treedata.Length+1];
			teTreeOptions[0]="(none)";
			if(treedata.Length>0){
				for(int i=0;i<treedata.Length;i++){
					teTreeOptions[i+1] = treedata[i].prefab.name;
				}
			}
			teTreeRuleProtoId[teTreeRuleIndex] = EditorGUILayout.Popup(teTreeRuleProtoId[teTreeRuleIndex],teTreeOptions,GUILayout.Width(120));
			GUILayout.EndHorizontal();
			if(teTreeRuleProtoId[teTreeRuleIndex]>0){
				GUILayout.BeginHorizontal();
					GUILayout.Label ("Height:");
					GUILayout.Label (teTreeRuleParams[teTreeRuleIndex,0].ToString("N2"), GUILayout.Width(40));
					EditorGUILayout.MinMaxSlider(ref teTreeRuleParams[teTreeRuleIndex,0],ref teTreeRuleParams[teTreeRuleIndex,1],0.0f,1.0f,GUILayout.MinWidth(40));
					GUILayout.Label (teTreeRuleParams[teTreeRuleIndex,1].ToString("N2"), GUILayout.Width(40));
				GUILayout.EndHorizontal();
				GUILayout.BeginHorizontal();
					GUILayout.Label ("Slope:");
					GUILayout.Label (teTreeRuleParams[teTreeRuleIndex,2].ToString("N2"), GUILayout.Width(40));
					EditorGUILayout.MinMaxSlider(ref teTreeRuleParams[teTreeRuleIndex,2],ref teTreeRuleParams[teTreeRuleIndex,3],0.0f,90.0f,GUILayout.MinWidth(40));
					GUILayout.Label (teTreeRuleParams[teTreeRuleIndex,3].ToString("N2"), GUILayout.Width(40));
				GUILayout.EndHorizontal();
				teTreeRuleParams[teTreeRuleIndex,6] = TerrainEdge.teFloatSlider("Trees", teTreeRuleParams[teTreeRuleIndex,6], 1.0f, 2000f);
				teTreeColor[teTreeRuleIndex] = teFunc.colorParse(teUI.colorPicker("Color:",teTreeColor[teTreeRuleIndex].ToString()));
				teTreeLightmapColor[teTreeRuleIndex] = teFunc.colorParse(teUI.colorPicker("Lightmap:",teTreeLightmapColor[teTreeRuleIndex].ToString()));
				teTreeWidth[teTreeRuleIndex] = TerrainEdge.teFloatSlider("Width", teTreeWidth[teTreeRuleIndex], 0.25f, 4.0f);
				teTreeWidthVariation[teTreeRuleIndex] = TerrainEdge.teFloatSlider("Variation", teTreeWidthVariation[teTreeRuleIndex], 0.0f, 0.9f);
				teTreeHeight[teTreeRuleIndex] = TerrainEdge.teFloatSlider("Height", teTreeHeight[teTreeRuleIndex], 0.25f, 4.0f);
				teTreeHeightVariation[teTreeRuleIndex] = TerrainEdge.teFloatSlider("Variation", teTreeHeightVariation[teTreeRuleIndex], 0.0f, 0.9f);
		
				GUI.backgroundColor = new Color(0.9f,0.9f,1f,1f);
				GUILayout.BeginVertical(EditorStyles.textField);
				GUILayout.Label("Tree Surround Foliage");
				GUILayout.BeginHorizontal();
				tfRuleIndex = EditorGUILayout.Popup(tfRuleIndex,tfRules);
				tfRuleParams[teTreeRuleIndex,tfRuleIndex,0] = EditorGUILayout.Popup(int.Parse("0"+tfRuleParams[teTreeRuleIndex,tfRuleIndex,0]),tfRuleTypes).ToString();
				GUILayout.EndHorizontal();
				
				if(int.Parse("0"+tfRuleParams[teTreeRuleIndex,tfRuleIndex,0])>0){
					string[] tfProtos = teFunc.getProtoList(tfRuleParams[teTreeRuleIndex,tfRuleIndex,0],TerrainEdge.selectedObject.GetComponent<Terrain>().terrainData);		
					if(tfProtos.Length==0){
						tfProtos = new string[1]{"Terrain has no "+tfRuleTypes[int.Parse(tfRuleParams[teTreeRuleIndex,tfRuleIndex,0])]+" prototypes"};
						GUI.enabled=false;
						tfRuleParams[teTreeRuleIndex,tfRuleIndex,1] = EditorGUILayout.Popup(int.Parse(tfRuleParams[teTreeRuleIndex,tfRuleIndex,1]),tfProtos).ToString();
						GUI.enabled=true;
					} else {
						tfRuleParams[teTreeRuleIndex,tfRuleIndex,1] = EditorGUILayout.Popup(int.Parse(tfRuleParams[teTreeRuleIndex,tfRuleIndex,1]),tfProtos).ToString();
						string folTypeId = tfRuleParams[teTreeRuleIndex,tfRuleIndex,0];	
						if(folTypeId!="3"){
					    	tfRuleParams[teTreeRuleIndex,tfRuleIndex,2] = teUI.floatSlider("Strength:",float.Parse(tfRuleParams[teTreeRuleIndex,tfRuleIndex,2]),0f,1f).ToString();	
						}
						tfRuleParams[teTreeRuleIndex,tfRuleIndex,3] = teUI.floatSlider("Density:",float.Parse(tfRuleParams[teTreeRuleIndex,tfRuleIndex,3]),0f,1f).ToString();				
						tfRuleParams[teTreeRuleIndex,tfRuleIndex,10] = teUI.floatSlider("Radius:",float.Parse(tfRuleParams[teTreeRuleIndex,tfRuleIndex,10]),0f,10f).ToString();
						if(folTypeId=="3"){
							tfRuleParams[teTreeRuleIndex,tfRuleIndex,4] = teUI.colorPicker("Color:",tfRuleParams[teTreeRuleIndex,tfRuleIndex,4]);
							tfRuleParams[teTreeRuleIndex,tfRuleIndex,5] = teUI.colorPicker("Light:",tfRuleParams[teTreeRuleIndex,tfRuleIndex,5]);
							tfRuleParams[teTreeRuleIndex,tfRuleIndex,6] = teUI.floatSlider("Width:",float.Parse(tfRuleParams[teTreeRuleIndex,tfRuleIndex,6]),1f,4f).ToString();
							tfRuleParams[teTreeRuleIndex,tfRuleIndex,7] = teUI.floatSlider("Variation:",float.Parse(tfRuleParams[teTreeRuleIndex,tfRuleIndex,7]),0.0f,0.9f).ToString();
							tfRuleParams[teTreeRuleIndex,tfRuleIndex,8] = teUI.floatSlider("Height:",float.Parse(tfRuleParams[teTreeRuleIndex,tfRuleIndex,8]),1f,4f).ToString();
							tfRuleParams[teTreeRuleIndex,tfRuleIndex,9] = teUI.floatSlider("Variation:",float.Parse(tfRuleParams[teTreeRuleIndex,tfRuleIndex,9]),0.0f,0.9f).ToString();		
						}	
					}
				} else {
					string[] tfProtos = new string[1]{"No type selected"};
					GUI.enabled=false;
					tfRuleParams[teTreeRuleIndex,tfRuleIndex,1] = EditorGUILayout.Popup(int.Parse("0"+tfRuleParams[teTreeRuleIndex,tfRuleIndex,1]),tfProtos).ToString();
					GUI.enabled=true;
				}
				GUILayout.EndVertical();
				GUI.backgroundColor = new Color(1f,1f,1f,1f);			
			}
			
			GUILayout.BeginHorizontal();
			if(GUILayout.Button("Selected")){
				Undo.RegisterUndo(TerrainEdge.selectedObject.GetComponent<Terrain>().terrainData,"te:Generate Trees");
				Generate(TerrainEdge.selectedObject);
			}
			if(GUILayout.Button("Full Scene")){
				Undo.RegisterUndo(FindObjectsOfType(typeof(Terrain)) as Terrain[],"te:Generate All Terrain Trees"); 
				Terrain[] terrs = FindObjectsOfType(typeof(Terrain)) as Terrain[];
				int terrIndex = 0;
				foreach (Terrain terr in terrs) {
					//EditorUtility.DisplayProgressBar("Generating Trees", "Generating "+terr.gameObject.name+" ("+(terrIndex+1)+" of "+terrs.Length+")....",(float)(terrIndex*(1.0f/terrs.Length)));
					Generate(terr.gameObject);
					EditorUtility.ClearProgressBar();
					terrIndex++;
				}
			}
			GUILayout.EndHorizontal();
        }
        else
        {
            GUILayout.Label("Select a terrain object.", EditorStyles.miniBoldLabel);
        }

        GUILayout.EndVertical();
    }
		
	public static void genIt(GameObject go){
		TerrainData terdata = go.GetComponent<Terrain>().terrainData;
		TreeInstance[] newtrees = new TreeInstance[0];
		int res = terdata.heightmapResolution;
		float tmpHeight = 0.0f;
		float tmpSlope = 0.0f;
		bool[,] posTaken = new bool[res+1,res+1];
		terdata.treeInstances = newtrees;
		for(int ruleId=0;ruleId<10;ruleId++){
			if(teTreeRuleProtoId[ruleId]>0){
				int treecount = 0;
				int attempts = 0;
				while(treecount<(int)teTreeRuleParams[ruleId,6]&&attempts<2000){
					EditorUtility.DisplayProgressBar("Generate Trees","Terrain GameObject:"+go.name+"\nTree Rule: "+ruleId.ToString()+"\nGenerating: "+treecount+" of "+(int)teTreeRuleParams[ruleId,6]+"...",(float)treecount*(1f/(float)teTreeRuleParams[ruleId,6]));
				
					float strengthmult = 1f;
					int x=(int)((float)res*UnityEngine.Random.value);
					int y=(int)((float)res*UnityEngine.Random.value);
					tmpHeight = (terdata.GetHeight(y,x)/(terdata.size.y));		
					tmpSlope = terdata.GetSteepness(((1.0f/res)*y)+(0.2f/res),((1.0f/res)*x)+(0.2f/res));
					if(tmpHeight<teTreeRuleParams[ruleId,0]){strengthmult = 0f;}
					if(tmpHeight>teTreeRuleParams[ruleId,1]){strengthmult = 0f;}
					if(tmpSlope<teTreeRuleParams[ruleId,2]){strengthmult = 0f;}
					if(tmpSlope>teTreeRuleParams[ruleId,3]){strengthmult = 0f;}		
					if(posTaken[x,y]!=true&&strengthmult>0f){
						treecount++;
						attempts = 0;
						posTaken[x,y]=true;
						TreeInstance newTree = new TreeInstance();
						newTree.prototypeIndex = teTreeRuleProtoId[ruleId]-1;
						float treeX = (float)x / (float)res;
						float treeY = (float)y / (float)res;
						newTree.position = new Vector3(treeY,0,treeX);
						newTree.widthScale = teTreeWidth[ruleId] + (teTreeWidthVariation[ruleId]*(UnityEngine.Random.value-0.75f));
						newTree.heightScale = teTreeHeight[ruleId] + (teTreeHeightVariation[ruleId]*(UnityEngine.Random.value-0.75f));
						newTree.color = teTreeColor[ruleId];
						newTree.lightmapColor = teTreeLightmapColor[ruleId];
						go.GetComponent<Terrain>().AddTreeInstance(newTree);
						treeSurroundFoliage(ruleId,treeX,treeY);
					}
					attempts++;
				}
				EditorUtility.ClearProgressBar();
			}
		}
		go.GetComponent<Terrain>().Flush();
	}
	
	public static void treeSurroundFoliage(int treeRuleId,float inY, float inX){
		GameObject go = TerrainEdge.selectedObject;
		TerrainData td = go.GetComponent<Terrain>().terrainData;
		int x = (int)(inX*td.size.x);
		int z = (int)(inY*td.size.z);
		int detailres = td.detailResolution;
		int alphares = td.alphamapResolution;
		float[,,] alphadata = td.GetAlphamaps(0,0,alphares,alphares);
		float detailMult = (1.0f / td.size.x)*(float)detailres;
		float alphaMult = (1.0f / td.size.x)*(float)alphares;
		int detX = (int)(detailMult*(float)x); //x:z coords for other map resolutions
		int detZ = (int)(detailMult*(float)z);
		int alphaX = (int)(alphaMult*(float)x);
		int alphaZ = (int)(alphaMult*(float)z); 
		int[,] detaildata = new int[detailres,detailres]; 

		for(int tfRuleId=0;tfRuleId<10;tfRuleId++){	
			int pasteregionoffset = 0;
			float strengthmult =1.0f;
			bool[,] posTaken = new bool[(int)td.size.x,(int)td.size.x];
			switch(tfRuleParams[treeRuleId,tfRuleId,0]){
			case "1":
					//Debug.Log("TF-Detail @"+x+":"+z);
					// Detail -------------------------------------------
					detaildata = td.GetDetailLayer(0,0,detailres,detailres,int.Parse(tfRuleParams[treeRuleId,tfRuleId,1]));
					pasteregionoffset = (int)float.Parse("0"+tfRuleParams[treeRuleId,tfRuleId,10]);
					for(int tZ=(detZ)-pasteregionoffset;tZ<(detZ)+(pasteregionoffset+1);tZ++){
						for(int tX=(detX)-pasteregionoffset;tX<(detX)+(pasteregionoffset+1);tX++){
							strengthmult = (float.Parse(tfRuleParams[treeRuleId,tfRuleId,2])*15f)*teFunc.falloffMult(tX,detX,tZ,detZ,pasteregionoffset);
							if(tZ>=0&&tZ<=detailres-1&&tX>=0&&tX<=detailres-1){
								if(0f<float.Parse(tfRuleParams[treeRuleId,tfRuleId,3])){;
									int tmpval = detaildata[tZ,tX]+(int)strengthmult;
									if(tmpval>15){tmpval=15;}
									detaildata[tZ,tX]=tmpval;
								}
							}
						}
					}
					td.SetDetailLayer(0,0,int.Parse(tfRuleParams[treeRuleId,tfRuleId,1]),detaildata);
					break;
			case "2":
					//Debug.Log("TF-Splat @"+x+":"+z);
					// Splat/Texture  -------------------------------------------
					pasteregionoffset = (int)float.Parse("0"+tfRuleParams[treeRuleId,tfRuleId,10]);
					for(int tZ=(alphaZ)-pasteregionoffset;tZ<(alphaZ)+(pasteregionoffset+1);tZ++){
						for(int tX=(alphaX)-pasteregionoffset;tX<(alphaX)+(pasteregionoffset+1);tX++){
							strengthmult = (int)float.Parse(tfRuleParams[treeRuleId,tfRuleId,3])*teFunc.falloffMult(tX,alphaX,tZ,alphaZ,pasteregionoffset);
							if(tZ>=0&&tZ<=alphares-1&&tX>=0&&tX<=alphares-1){
								if(UnityEngine.Random.value<float.Parse(tfRuleParams[treeRuleId,tfRuleId,2])){
									float addAmount = strengthmult;
									float remainder = 1.0f-addAmount;
									// Now we tally up the total value shared by other splat channels...
									float cumulativeAmount = 0.0f;
									for(int i2=0;i2<td.splatPrototypes.Length;i2++){
										if(i2!=int.Parse(tfRuleParams[treeRuleId,tfRuleId,1])){
											cumulativeAmount=cumulativeAmount+alphadata[tZ,tX,i2];
										}
									}
									if(cumulativeAmount>0.0f){
										float fixLayerMult = remainder / cumulativeAmount; // we multiple the other layer's splat values by this
										// Now we re-apply the splat values..
										for(int i2=0;i2<td.splatPrototypes.Length;i2++){
											if(i2!=int.Parse(tfRuleParams[treeRuleId,tfRuleId,1])){
												alphadata[tZ,tX,i2] = fixLayerMult*alphadata[tZ,tX,i2];
											} else {
												alphadata[tZ,tX,i2] = addAmount;
											}
										}
									} else {
										alphadata[tZ,tX,int.Parse(tfRuleParams[treeRuleId,tfRuleId,1])] = 1.0f;
									}
								}
							}
						}
					}
					break;
			case "3":
					// Trees -------------------------------------------------------
					strengthmult = float.Parse("0"+tfRuleParams[treeRuleId,tfRuleId,2]);
					pasteregionoffset = (int)float.Parse("0"+tfRuleParams[treeRuleId,tfRuleId,10]);
					for(int tZ=z-pasteregionoffset;tZ<z+(pasteregionoffset+1);tZ++){
						for(int tX=x-pasteregionoffset;tX<x+(pasteregionoffset+1);tX++){
							if(tZ>=0&&tZ<=td.size.z-1&&tX>=0&&tX<=td.size.x-1){
								
								if(posTaken[tX,tZ]!=true&&UnityEngine.Random.value<(0.01f*strengthmult*teFunc.falloffMult(tX,x,tZ,z,10))){
									posTaken[tX,tZ]=true;
									TreeInstance newTree = new TreeInstance();
									newTree.prototypeIndex = int.Parse(tfRuleParams[treeRuleId,tfRuleId,1]);
									float treeX = (float)tX/td.size.x;
									float treeZ = (float)tZ/td.size.z;
									newTree.position = new Vector3(treeX,0,treeZ);
									newTree.widthScale = float.Parse(tfRuleParams[treeRuleId,tfRuleId,6]) + (float.Parse(tfRuleParams[treeRuleId,tfRuleId,7])*(UnityEngine.Random.value-0.75f));
									newTree.heightScale = float.Parse(tfRuleParams[treeRuleId,tfRuleId,8]) + (float.Parse(tfRuleParams[treeRuleId,tfRuleId,9])*(UnityEngine.Random.value-0.75f));
									newTree.color = TerEdge.teFunc.colorParse(tfRuleParams[treeRuleId,tfRuleId,4]);
									newTree.lightmapColor = TerEdge.teFunc.colorParse(tfRuleParams[treeRuleId,tfRuleId,5]);
									go.GetComponent<Terrain>().AddTreeInstance(newTree);
								}
							}
						}
					}
					// Get rid of tree colliders..
					float[,] tmpheights = td.GetHeights(0, 0, 0, 0);
        			td.SetHeights(0, 0, tmpheights);
					break;
			}
		td.SetAlphamaps(0,0,alphadata);			
		}	
	}
}

// TE:File Functions ===========================================================================

public class TEFileFunctions : TEGroup
{
    public TEFileFunctions() : base("File Functions", "Load/save your current TE3 settings to file.") { }
    public override void Generate(GameObject go){}
	public override void sceneEvents(SceneView sceneview){}
    protected override void OnInit(){}
    public override void OnGUI()
    {
        GUILayout.BeginHorizontal(EditorStyles.textField);
        if (GUILayout.Button("Load")){ TerrainEdge.loadConfig(EditorUtility.OpenFilePanel("Load TE Configuration", "", "te3"), false);}
        if (GUILayout.Button("Save")){ TerrainEdge.saveConfig(EditorUtility.SaveFilePanel("Save TE Configuration", "", "untitled", "te3"),false);}
        GUILayout.EndVertical();
    }
}

// TerrainEdge Class ===========================================================================

public class TerrainEdge : EditorWindow {

#region members
	public static string vers = "2.67";
	public static GameObject selectedObject = null;
	public static bool isObjectTerrain = false;
	public static GameObject tileW;
	public static GameObject tileE;
	public static GameObject tileN;
	public static GameObject tileS;
	public static GameObject tileNW;
	public static GameObject tileNE;
	public static GameObject tileSW;
	public static GameObject tileSE;
    public static string teConfigData = "";
	private string[] teMainMenu = new string[0];
	private int teMainMenuSelected = 0;
#endregion

   [MenuItem ("Window/TerrainEdge")]
	static void Init() {
		TerrainEdge window = (TerrainEdge)EditorWindow.GetWindow (typeof (TerrainEdge));
		window.Show();
		Debug.Log("TE:Init");	
	}

   List<TEGroup> Groups = new List<TEGroup>();

   public TerrainEdge()
   {
       Groups.Add(new TEAbout());
       Groups.Add(null);
       Groups.Add(new TEDetail());
       Groups.Add(new TEExpand());
       Groups.Add(new TEFoliageGroup());
	   Groups.Add(new TEMapInspector());
       Groups.Add(new TENoiseLab());
       Groups.Add(new TETextures());
       Groups.Add(new TETools());
       Groups.Add(new TETrees());
       Groups.Add(null);
       Groups.Add(new TEFileFunctions());
   }
	
	void OnDestroy(){
		//Debug.Log("TE:Unload");	
	}
	
	void OnInstantiate(){
		//Debug.Log("TE:Instantiate");	
	}
	
	void OnEnable(){
		Debug.Log("TE:TerrainEdge has been re-activated (Settings automatically loaded from 'autosave.te3')");
		if(Application.isPlaying==false){
			SceneView.onSceneGUIDelegate = sceneEvents;
			if(File.Exists(Application.dataPath+"/autosave.te3")){
				loadConfig(Application.dataPath+"/autosave.te3",true);
			}
		}
	}	
	
	void OnDisable(){
		Debug.Log("TE:TerrainEdge has been disabled (Settings saved as 'autosave.te3')");
		TerrainEdge.saveConfig(Application.dataPath+"/autosave.te3",true);
	}


	
	void sceneEvents(SceneView sceneview){
		if(teMainMenuSelected>0){
			Groups[teMainMenuSelected].sceneEvents(sceneview);
		}
	}
	
	void OnGUI () {
        // initialization/update stuff, this allows others to dynamically change the list!
        if (teMainMenu.Length != Groups.Count)
        {
            teMainMenu = new string[Groups.Count];
            int i = 0;
            foreach(TEGroup group in Groups)
            {
                if (group != null)
                {
                    teMainMenu[i++] = group.Name;
                    group.Init(); // this only gets called once.
                }
                else
                {
                    teMainMenu[i++] = "";
                }
            }
        }
		
        // display the gui
		EditorGUI.DropShadowLabel (new Rect(0, 0, 500, 20),"",EditorStyles.toolbarButton);
		EditorGUI.DropShadowLabel (new Rect(5, -1, 150, 20),"TerrainEdge "+vers,EditorStyles.miniLabel);
		EditorGUILayout.Space();EditorGUILayout.Space();EditorGUILayout.Space();
		
		// update selected object, set flag for whether it is a terrain or not and set neighborhood objects
		if(selectedObject!=Selection.activeGameObject){
			selectedObject = Selection.activeGameObject;
			isObjectTerrain = false;
			tileW = null; tileE = null; tileS = null; tileN = null;
	        tileNW = null; tileNE = null; tileSW = null; tileSE = null;
			if(selectedObject){
				if(selectedObject.GetComponent<Terrain>()!=null){
					isObjectTerrain=true;
			        getNeighbors(Selection.activeGameObject);	
				}
			}
		}
		GUILayout.BeginVertical(EditorStyles.textField);
		teMainMenuSelected = EditorGUILayout.Popup(teMainMenuSelected,teMainMenu);
        TEGroup selected = teMainMenuSelected < Groups.Count ? Groups[teMainMenuSelected] : null;
        if (selected == null)
        {
            GUILayout.EndVertical();
            return; 
        }
        GUILayout.Label(selected.Description, EditorStyles.wordWrappedLabel);
		GUILayout.EndVertical();
        selected.OnGUI();
    }

	// General Global Functions ==================================================================================
	
	public static float teFloatSlider(string label, float floatVar, float floatMin, float floatMax)
	{
		GUILayout.BeginHorizontal();
		GUILayout.Label (label, GUILayout.Width(80));
		floatVar = GUILayout.HorizontalSlider(floatVar,floatMin,floatMax,GUILayout.MinWidth(40));
		if(floatMax>500f){
			GUILayout.Label (floatVar.ToString("N0"), GUILayout.Width(40));
		}else{
			GUILayout.Label (floatVar.ToString("N3"), GUILayout.Width(40));
		}
		GUILayout.EndHorizontal();	
		return floatVar;
	}
	
	public static float[,] getNeighborhoodHeights(GameObject go){ // Cross-seam painting and prevent filters from breaking at boundries...
		getNeighbors(go);
		int hres = go.GetComponent<Terrain>().terrainData.heightmapResolution;					
		float[,] tmpArray = new float[(hres*3)-1,(hres*3)-1];
		float[,] tmpArray2 = new float[(hres*3)-1,(hres*3)-1];
		if(teFunc.isTerrain(tileSW)){tmpArray = tmpArray2; tmpArray2 = inject2Dinto2D(tmpArray,tileSW.GetComponent<Terrain>().terrainData.GetHeights(0,0,hres,hres),0,0);}
		if(teFunc.isTerrain(tileS)){tmpArray = tmpArray2; tmpArray2 = inject2Dinto2D(tmpArray,tileS.GetComponent<Terrain>().terrainData.GetHeights(0,0,hres,hres),hres-1,0);}
		if(teFunc.isTerrain(tileSE)){tmpArray = tmpArray2; tmpArray2 = inject2Dinto2D(tmpArray,tileSE.GetComponent<Terrain>().terrainData.GetHeights(0,0,hres,hres),(hres*2)-2,0);}
		if(teFunc.isTerrain(tileW)){tmpArray = tmpArray2; tmpArray2 = inject2Dinto2D(tmpArray,tileW.GetComponent<Terrain>().terrainData.GetHeights(0,0,hres,hres),0,hres-1);}
		if(teFunc.isTerrain(go)){tmpArray = tmpArray2; tmpArray2 = inject2Dinto2D(tmpArray,go.GetComponent<Terrain>().terrainData.GetHeights(0,0,hres,hres),hres-1,hres-1);}
		if(teFunc.isTerrain(tileE)){tmpArray = tmpArray2; tmpArray2 = inject2Dinto2D(tmpArray,tileE.GetComponent<Terrain>().terrainData.GetHeights(0,0,hres,hres),(hres*2)-2,hres-1);}
		if(teFunc.isTerrain(tileNW)){tmpArray = tmpArray2; tmpArray2 = inject2Dinto2D(tmpArray,tileNW.GetComponent<Terrain>().terrainData.GetHeights(0,0,hres,hres),0,(hres*2)-2);}
		if(teFunc.isTerrain(tileN)){tmpArray = tmpArray2; tmpArray2 = inject2Dinto2D(tmpArray,tileN.GetComponent<Terrain>().terrainData.GetHeights(0,0,hres,hres),hres-1,(hres*2)-2);}
		if(teFunc.isTerrain(tileNE)){tmpArray = tmpArray2; tmpArray2 = inject2Dinto2D(tmpArray,tileNE.GetComponent<Terrain>().terrainData.GetHeights(0,0,hres,hres),(hres*2)-2,(hres*2)-2);}
		return tmpArray2;
	}

	public static void setNeighborhoodHeights(GameObject go,float[,] bigHeightMap){
		getNeighbors(go);
		int hres = go.GetComponent<Terrain>().terrainData.heightmapResolution;	
		if(teFunc.isTerrain(tileSW)){tileSW.GetComponent<Terrain>().terrainData.SetHeights(0,0,extract2Dfrom2D(bigHeightMap,0,0,hres,hres));}
		if(teFunc.isTerrain(tileS)){tileS.GetComponent<Terrain>().terrainData.SetHeights(0,0,extract2Dfrom2D(bigHeightMap,hres-1,0,hres,hres));}
		if(teFunc.isTerrain(tileSE)){tileSE.GetComponent<Terrain>().terrainData.SetHeights(0,0,extract2Dfrom2D(bigHeightMap,(hres*2)-2,0,hres,hres));}
		if(teFunc.isTerrain(tileW)){tileW.GetComponent<Terrain>().terrainData.SetHeights(0,0,extract2Dfrom2D(bigHeightMap,0,hres-1,hres,hres));}
		if(teFunc.isTerrain(go)){go.GetComponent<Terrain>().terrainData.SetHeights(0,0,extract2Dfrom2D(bigHeightMap,hres-1,hres-1,hres,hres));}
		if(teFunc.isTerrain(tileE)){tileE.GetComponent<Terrain>().terrainData.SetHeights(0,0,extract2Dfrom2D(bigHeightMap,(hres*2)-2,hres-1,hres,hres));}
		if(teFunc.isTerrain(tileNW)){tileNW.GetComponent<Terrain>().terrainData.SetHeights(0,0,extract2Dfrom2D(bigHeightMap,0,(hres*2)-2,hres,hres));}
		if(teFunc.isTerrain(tileN)){tileN.GetComponent<Terrain>().terrainData.SetHeights(0,0,extract2Dfrom2D(bigHeightMap,hres-1,(hres*2)-2,hres,hres));}
		if(teFunc.isTerrain(tileNE)){tileNE.GetComponent<Terrain>().terrainData.SetHeights(0,0,extract2Dfrom2D(bigHeightMap,(hres*2)-2,(hres*2)-2,hres,hres));}
	}
			
	public static float[,] extract2Dfrom2D(float[,] bigArray,int offX, int offY, int sizeX, int sizeY){
		float[,] tmpArray = new float[sizeX,sizeY];
		for(int y=0;y<sizeY;y++){
			for(int x=0;x<sizeX;x++){
				tmpArray[x,y]=bigArray[offX+x,offY+y];
			}
		}
		return tmpArray;
	}
					
	public static float[,] inject2Dinto2D(float[,] bigArray,float[,] smallArray,int offX,int offY){
		for(int y = offY;y<(int)Mathf.Sqrt(smallArray.Length)+offY;y++){
			for(int x = offX;x<(int)Mathf.Sqrt(smallArray.Length)+offX;x++){
				bigArray[x,y] = smallArray[x-offX,y-offY];
			}
		}
		return bigArray;
	}
					
	public static void getNeighbors(GameObject go)
		{
			tileS = null; tileN=null; tileW=null; tileE=null;
			tileSE = null; tileNW=null; tileSW=null; tileNE=null;
			Terrain[] terrains = FindObjectsOfType(typeof(Terrain)) as Terrain[];
	        Vector3 gopos = go.transform.position;
	        float cwidth = go.GetComponent<Terrain>().terrainData.size.x;
	        foreach (Terrain terrain in terrains)
	        {
	            Vector3 tpos = terrain.transform.position;
	            if (tpos.x == gopos.x - cwidth && tpos.z == gopos.z) { tileS = terrain.gameObject; }
	            if (tpos.x == gopos.x + cwidth && tpos.z == gopos.z) { tileN = terrain.gameObject; }
	            if (tpos.z == gopos.z - cwidth && tpos.x == gopos.x) { tileW = terrain.gameObject; }
	            if (tpos.z == gopos.z + cwidth && tpos.x == gopos.x) { tileE = terrain.gameObject; }
	            if (tpos.x == gopos.x - cwidth && tpos.z == gopos.z - cwidth) { tileSW = terrain.gameObject; }
	            if (tpos.x == gopos.x - cwidth && tpos.z == gopos.z + cwidth) { tileSE = terrain.gameObject; }
	            if (tpos.x == gopos.x + cwidth && tpos.z == gopos.z - cwidth) { tileNW = terrain.gameObject; }
	            if (tpos.x == gopos.x + cwidth && tpos.z == gopos.z + cwidth) { tileNE = terrain.gameObject; }
	        }	
		}
	
	// === Configuration Functions ===============================================================	
	
    public static void addCKey(string configKey, string configValueType, string configValue)
    {
        teConfigData = teConfigData + configKey + "=(" + configValueType + ")" + configValue + "\n";
    }

    public static string getCKey(string configKey)
    {
        //string[] teConfigLines = teConfigData.Split(char.Parse("\n"));
		int keyinfostart = teConfigData.IndexOf(configKey+"=");
		if(keyinfostart>-1){
			int keyinfoend = teConfigData.IndexOf("\n",keyinfostart);
			string keyline = teConfigData.Substring(keyinfostart,keyinfoend-keyinfostart);
			string configValue = keyline.Replace(configKey + "=", "");
            return configValue.Substring(3, configValue.Length - 3);
        }
		Debug.Log(configKey+" (no data saved)");
        return "";
    }			

	// === Configuration Functions (To Split & Move Into Groups) ==============================

	/* NOTE: These need to be changed to iterate through each TEGroup and call each class's instances of the saveconfig routines.
	 * And then all the "addCKey", etc. calls need to be moved to the relevant group in their own save/load functions.  */

	public static void saveConfig(string fpath, bool silent)
    {
		if(fpath.Length < 2){return;}
        teConfigData = "";
		addCKey("onlyBrokenEdges","b",TETools.onlyBrokenEdges.ToString());
		addCKey("blendDist","i",TETools.blendDist.ToString());
		addCKey("blendIterations","i", TETools.blendIterations.ToString());
		addCKey("blendSmooth","i",TETools.blendSmooth.ToString());
		addCKey("noiseAmp","f",TENoiseLab.noiseAmp.ToString());
		addCKey("alphaStrength","f",TENoiseLab.alphaStrength.ToString());
		addCKey("teNoiseChanIndex","i",TENoiseLab.teNoiseChanIndex.ToString());
		addCKey("teErodeThermIter","i",TETools.teErodeThermIter.ToString());
		addCKey("teErodeThermMinSlope","f",TETools.teErodeThermMinSlope.ToString());
		addCKey("teErodeThermFalloff","f",TETools.teErodeThermFalloff.ToString());
		addCKey("teRampWidth","i",TETools.teRampWidth.ToString());
		addCKey("teRampFalloff","i",TETools.teRampFalloff.ToString());
		addCKey("teTerObjGenPresetIndex","f",TEExpand.teTerObjGenPresetIndex.ToString());
		addCKey("teTerObjGenGridX","f",TEExpand.teTerObjGenGridX.ToString());
		addCKey("teTerObjGenGridY","f",TEExpand.teTerObjGenGridY.ToString());
		for(int i=0;i<20;i++){
			addCKey("invertTerrace"+i,"b",TENoiseLab.invertTerrace[i].ToString());
			addCKey("controlpointcount"+i,"f",TENoiseLab.controlpointcount[i].ToString());
			addCKey("noiseFuncMin"+i,"f",TENoiseLab.noiseFuncMin[i].ToString());
			addCKey("noiseFuncMax"+i,"f",TENoiseLab.noiseFuncMax[i].ToString());
			addCKey("srcChannel1Id"+i,"i",TENoiseLab.srcChannel1Id[i].ToString());
			addCKey("srcChannel2Id"+i,"i",TENoiseLab.srcChannel2Id[i].ToString());
			addCKey("srcChannel3Id"+i,"i",TENoiseLab.srcChannel3Id[i].ToString());
			addCKey("teFunctionTypeIndex"+i,"i",TENoiseLab.teFunctionTypeIndex[i].ToString());
			addCKey("teNoiseChanTypeIndex"+i,"i",TENoiseLab.teNoiseChanTypeIndex[i].ToString());
			addCKey("teNoiseTypeIndex"+i,"i",TENoiseLab.teNoiseTypeIndex[i].ToString());
			addCKey("lacunarity"+i,"d",TENoiseLab.lacunarity[i].ToString());
			addCKey("persistance"+i,"d",TENoiseLab.persistance[i].ToString());
			addCKey("displacement"+i,"d",TENoiseLab.displacement[i].ToString());
			addCKey("frequency"+i,"d",TENoiseLab.frequency[i].ToString());
			addCKey("distance"+i,"b",TENoiseLab.distance[i].ToString());
			addCKey("seed"+i,"i",TENoiseLab.seed[i].ToString());
			addCKey("octaves"+i,"i",TENoiseLab.octaves[i].ToString());
			addCKey("zoom"+i,"f",TENoiseLab.zoom[i].ToString());
			addCKey("falloff"+i,"f",TENoiseLab.falloff[i].ToString());
			addCKey("offset"+i,"f",TENoiseLab.offset[i].ToString());
			addCKey("scale"+i,"f",TENoiseLab.scale[i].ToString());
			addCKey("bias"+i,"f",TENoiseLab.bias[i].ToString());
			addCKey("power"+i,"f",TENoiseLab.power[i].ToString());
			addCKey("exponent"+i,"f",TENoiseLab.exponent[i].ToString());
			addCKey("gain"+i,"f",TENoiseLab.gain[i].ToString());
			
			TENoiseLab.falloff[i] = double.Parse(getCKey("falloff" + i));
			TENoiseLab.offset[i] = double.Parse(getCKey("offset" + i));
			TENoiseLab.scale[i] = double.Parse(getCKey("scale" + i));
			TENoiseLab.bias[i] = double.Parse(getCKey("bias" + i));
			TENoiseLab.power[i] = double.Parse(getCKey("power" + i));
			TENoiseLab.exponent[i] = double.Parse(getCKey("exponent" + i));
			TENoiseLab.gain[i] = double.Parse(getCKey("gain" + i));

			for(int i2=0;i2<10;i2++){
				addCKey("cpval"+i+"_"+i2,"f",TENoiseLab.cpval[i,i2].ToString());
			}
			if(i<10){
				for (int i2 = 0; i2 < 10; i2++){
					addCKey("teFoliageParams"+i+"_"+i2,"s",TEFoliageGroup.teFoliageParams[i,i2]);
					for (int i3 = 0; i3 < 12; i3++){
						addCKey("tfRuleParams"+i+"_"+i2+"_"+i3,"s",TETrees.tfRuleParams[i,i2,i3]);	
					}
				}
				addCKey("teTreeLightmapColor"+i,"i",TETrees.teTreeLightmapColor[i].ToString());
				addCKey("teTreeColor"+i,"i",TETrees.teTreeColor[i].ToString());
				addCKey("teTreeHeight"+i,"i",TETrees.teTreeHeight[i].ToString());
				addCKey("teTreeWidth"+i,"i",TETrees.teTreeWidth[i].ToString());
				addCKey("teTreeHeightVariation"+i,"i",TETrees.teTreeHeightVariation[i].ToString());
				addCKey("teTreeWidthVariation"+i,"i",TETrees.teTreeWidthVariation[i].ToString());
				addCKey("teDetailSplatPrototypeEnable"+i,"i",TEDetail.teDetailSplatPrototypeEnable[i].ToString());
				addCKey("teDetailSplatPrototypeMatch"+i,"i",TEDetail.teDetailSplatPrototypeMatch[i].ToString());
				addCKey("teDetailSplatPrototypeAmount"+i,"i",TEDetail.teDetailSplatPrototypeAmount[i].ToString());
				addCKey("teDetailRuleProtoId"+i,"i",TEDetail.teDetailRuleProtoId[i].ToString());
				addCKey("teTreeRuleProtoId"+i,"i",TETrees.teTreeRuleProtoId[i].ToString());
				for(int i2=0;i2<8;i2++){
					addCKey("teDetailRuleParams"+i+"_"+i2,"f",TEDetail.teDetailRuleParams[i,i2].ToString());
					addCKey("teTreeRuleParams"+i+"_"+i2,"f",TETrees.teTreeRuleParams[i,i2].ToString());	
				}
			}
			if(i<5){
				addCKey("teSplatSlopeSteepness"+i,"f",TETextures.teSplatSlopeSteepness[i].ToString());
				addCKey("teSplatSlopeTexId"+i,"i",TETextures.teSplatSlopeTexId[i].ToString());				
			}
			if(i<12){
				addCKey("teSplatElevationHeight"+i,"f",TETextures.teSplatElevationHeight[i].ToString());
				addCKey("teSplatElevationTexId"+i,"i",TETextures.teSplatElevationTexId[i].ToString());
			}
			if(i<4){
				addCKey("teExpandOpts"+i,"b",TEExpand.teExpandOpts[i].ToString());	
				addCKey("syncOptions"+i,"b",TETools.syncOptions[i].ToString());	
			}
		}
        File.WriteAllText(fpath, teConfigData);
		if(!silent){
        	EditorUtility.DisplayDialog("Save Configuration", "Configuration has been saved.", "Ok");
		}
    }

    public static void loadConfig(string fpath, bool silent)
    {
		if(fpath.Length < 2){return;}
		teConfigData = File.ReadAllText(fpath);
        TETools.onlyBrokenEdges = bool.Parse(getCKey("onlyBrokenEdges"));
        TETools.blendDist = int.Parse(getCKey("blendDist"));
        TETools.blendIterations = int.Parse(getCKey("blendIterations"));
        TETools.blendSmooth = int.Parse(getCKey("blendSmooth"));
        TENoiseLab.noiseAmp = float.Parse(getCKey("noiseAmp"));
        TENoiseLab.alphaStrength = float.Parse(getCKey("alphaStrength"));
        TENoiseLab.teNoiseChanIndex = int.Parse(getCKey("teNoiseChanIndex"));
		TETools.teErodeThermIter = int.Parse(getCKey("teErodeThermIter"));
		TETools.teErodeThermMinSlope = float.Parse(getCKey("teErodeThermMinSlope"));
		TETools.teErodeThermFalloff = float.Parse(getCKey("teErodeThermFalloff"));
		TETools.teRampWidth = int.Parse(getCKey("teRampWidth"));
		TETools.teRampFalloff = int.Parse(getCKey("teRampFalloff"));
		TEExpand.teTerObjGenPresetIndex = int.Parse(getCKey("teTerObjGenPresetIndex"));
		TEExpand.teTerObjGenGridX = int.Parse(getCKey("teTerObjGenGridX"));
		TEExpand.teTerObjGenGridY = int.Parse(getCKey("teTerObjGenGridY"));
        for (int i = 0; i < 20; i++)
        {
			TENoiseLab.controlpointcount[i] = int.Parse(getCKey("controlpointcount"+i));
			TENoiseLab.invertTerrace[i] = bool.Parse(getCKey("invertTerrace"+i));
			TENoiseLab.noiseFuncMin[i] = float.Parse(getCKey("noiseFuncMin" + i));
            TENoiseLab.noiseFuncMax[i] = float.Parse(getCKey("noiseFuncMax" + i));
            TENoiseLab.srcChannel1Id[i] = int.Parse(getCKey("srcChannel1Id" + i));
            TENoiseLab.srcChannel2Id[i] = int.Parse(getCKey("srcChannel2Id" + i));
            TENoiseLab.srcChannel3Id[i] = int.Parse(getCKey("srcChannel3Id" + i));
            TENoiseLab.teFunctionTypeIndex[i] = int.Parse(getCKey("teFunctionTypeIndex" + i));
            TENoiseLab.teNoiseChanTypeIndex[i] = int.Parse(getCKey("teNoiseChanTypeIndex" + i));
            TENoiseLab.teNoiseTypeIndex[i] = int.Parse(getCKey("teNoiseTypeIndex" + i));
            TENoiseLab.lacunarity[i] = double.Parse(getCKey("lacunarity" + i));
            TENoiseLab.persistance[i] = double.Parse(getCKey("persistance" + i));
            TENoiseLab.displacement[i] = double.Parse(getCKey("displacement" + i));
            TENoiseLab.frequency[i] = double.Parse(getCKey("frequency" + i));
            TENoiseLab.distance[i] = bool.Parse(getCKey("distance" + i));
            TENoiseLab.seed[i] = int.Parse(getCKey("seed" + i));
            TENoiseLab.octaves[i] = int.Parse(getCKey("octaves" + i));
            TENoiseLab.zoom[i] = int.Parse(getCKey("zoom" + i));
			TENoiseLab.falloff[i] = double.Parse(getCKey("falloff" + i));
			TENoiseLab.offset[i] = double.Parse(getCKey("offset" + i));
			TENoiseLab.scale[i] = double.Parse(getCKey("scale" + i));
			TENoiseLab.bias[i] = double.Parse(getCKey("bias" + i));
			TENoiseLab.power[i] = double.Parse(getCKey("power" + i));
			TENoiseLab.exponent[i] = double.Parse(getCKey("exponent" + i));
			TENoiseLab.gain[i] = double.Parse(getCKey("gain" + i));
			for(int i2=0;i2<10;i2++){
				TENoiseLab.cpval[i,i2] = float.Parse(getCKey("cpval"+i+"_"+i2));	
			}
            if (i < 10)
            {
				for (int i2 = 0; i2 < 10; i2++){
					TEFoliageGroup.teFoliageParams[i,i2] = getCKey("teFoliageParams"+i+"_"+i2);
					for (int i3 = 0; i3 < 12; i3++){
						TETrees.tfRuleParams[i,i2,i3]=getCKey("tfRuleParams"+i+"_"+i2+"_"+i3);	
					}	
				}
				TETrees.teTreeLightmapColor[i] = TerEdge.teFunc.colorParse(getCKey("teTreeLightmapColor"+i));
				TETrees.teTreeColor[i] = TerEdge.teFunc.colorParse(getCKey("teTreeColor"+i));
				TETrees.teTreeHeight[i] = float.Parse(getCKey("teTreeHeight"+i));
				TETrees.teTreeWidth[i] = float.Parse(getCKey("teTreeWidth"+i));
				TETrees.teTreeHeightVariation[i] = float.Parse(getCKey("teTreeHeightVariation"+i));
				TETrees.teTreeWidthVariation[i] = float.Parse(getCKey("teTreeWidthVariation"+i));
                TETrees.teTreeRuleProtoId[i] = int.Parse(getCKey("teTreeRuleProtoId" + i));
				TEDetail.teDetailSplatPrototypeEnable[i] = bool.Parse(getCKey("teDetailSplatPrototypeEnable"+i));
				TEDetail.teDetailSplatPrototypeMatch[i] = int.Parse(getCKey("teDetailSplatPrototypeMatch"+i));
				TEDetail.teDetailSplatPrototypeAmount[i] = float.Parse(getCKey("teDetailSplatPrototypeAmount"+i));
                TEDetail.teDetailRuleProtoId[i] = int.Parse(getCKey("teDetailRuleProtoId" + i));
                for (int i2 = 0; i2 < 8; i2++)
                {
                    TEDetail.teDetailRuleParams[i, i2] = float.Parse(getCKey("teDetailRuleParams" + i + "_" + i2));
                    TETrees.teTreeRuleParams[i, i2] = float.Parse(getCKey("teTreeRuleParams" + i + "_" + i2));
                }
            }
            if (i < 12)
            {
                TETextures.teSplatElevationHeight[i] = float.Parse(getCKey("teSplatElevationHeight" + i));
                TETextures.teSplatElevationTexId[i] = int.Parse(getCKey("teSplatElevationTexId" + i));
			}
			if (i < 5)
            {
				TETextures.teSplatSlopeSteepness[i] = float.Parse(getCKey("teSplatSlopeSteepness" + i));
                TETextures.teSplatSlopeTexId[i] = int.Parse(getCKey("teSplatSlopeTexId" + i));
            }
            if (i < 4)
            {
                TEExpand.teExpandOpts[i] = bool.Parse(getCKey("teExpandOpts" + i));
                TETools.syncOptions[i] = bool.Parse(getCKey("syncOptions" + i));
            }
        }
        for (int i = 0; i < 20; i++)
        {
			TENoiseLab.genNoise(i);
        }
        if(!silent){EditorUtility.DisplayDialog("Load Configuration", "Configuration has been loaded.", "Ok");}
    }

}
