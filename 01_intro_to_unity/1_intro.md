### scene and game object basics
* create a new project in unity
* create a new scene by `Assets > Create... > Scene`
* overview of camera object
* overview of light object
* create a new game object
* * click `GameObject > Create Empty`
* rename it `Cube` by clicking on the name field in the right panel at the top, and typing Cube
* add a component by click `Add Component` and select `Mesh > Mesh Filter` or by typing `Mesh Filter` in the search bar
* give the cube a skeleton
* * select the mesh by clicking on the dotted circle icon in the `Mesh Filter` component next to the field that says `None (Mesh)`
* * select `Cube`.
* make the cube appear
* * add a component by clicking `Add Component`
* * select `Mesh > Mesh Renderer` or by typing `Mesh Renderer` in the search bar.
* give a skin to the mesh
* * click on the arrow next to `Materials`
* * click on the dotted circle
* * select `Default - Diffuse`
* manipulate the cube
* * make sure you are in the `Scene View` by click the tab `Scene` next to the `Play` button
* * translation: click the top left icon of orthogonal arrows to select the translation tool (or press W)
* * click and drag one of the arrows to move the cube along one axis
* * click and drag the center box to move the cube freely
* * input exact numbers in the `Position` fields of the transform component at the top of the `Inspector` panel
* * rotation: click the top left icon of rotating arrows to select the rotation tool (or press E)
* * click and drag one of the colored circles to rotate along one axis
* * input exact numbers in the rotation fields of the `Transform` component, below the `Position` fields
* * scale: click the top left icon with arrows surrounding a square (or press R)
* * click and drag one of the solid colored squares in the scene view to scale along one axis
* * click and drag the central grey square along all axes
* * again, you can input any value you want in the `Scale` fields

**break: everybody makes a sphere**

### scripts basics

* attach a script
* * click `Add Component`
* * click `New Script` at the very bottom of the list (you need to scroll down)
* * give it a meaningful name, e.g. `CubeBehavior` with the language `C Sharp`
* * double-click on the script in the inspector panel
* * MonoDevelop should open
* overview of Unity Scripts
* `Start()` and `Update()` are similar to `Setup()` and `Draw()` in Processing/p5
* in `Start()`, type `Debug.Log("hey there, fellas")` --or whatever variant of Hello, World you'd like.
* go back to the Unity Editor
* click the `Play` button or press `CMD+P`
* click on the `Console` tab, on the bottom panel
* you should see the log message!
* the `Clear` button gets rid of all messages.
* go back to MonoDevelop and, this time, put `Debug.Log("something else")` inside `Update()`
* click play / run it
* in the console tab, you should see *a lot* of debug messages
* click `Collapse`, and all the messages should collapse into one field, with the number of times it's being
* stop the application by clicking on the `Play` button again.

### accessing components

* in order to modify what we see in the scene, we need to modify a component
* let's modify the color of our cube
* in `Start()`, type `this.gameObject.GetComponent<MeshRenderer>().material.color = Color.black;`
* run it
* the cube should turn black
* each part of that statement corresponds to a part of the game GameObject:
* * * first we start from the script
* * * then we get to the game object this script is attached to
* * * then we get to the MeshRenderer component of that game object
* * * then we get to the material field of that component
* * * then we get to the color value of that field

**break: make the sphere turn a different color**
**bonus: write a short script so that, every frame, the sphere has a 50/50 chance to turn one color or another**

* let's modify the position of the cube
* * first, let's declare a new variable, of type Vector3
* * below `public class CubeBehavior : MonoBehaviour {` and above `void Start () {`, type `Vector3 newPos;`
* * in `Start()`, initialize the variable by typing `newPos = new Vector3(1, 0, 1);`
* * run it
* * the cube should have moved to its new position. if you can't see it, click on the `Scene` tab and drag while pressing `Alt` to look around.
* let's make our variable public!
* * change `Vector3 newPos` to `public Vector3 newPos`
* * back in the Editor, you should see a new field in your `CubeBehavior` component called `New Pos`
* * change the values there to something different than what you have in `Start()`
* * run it.
* * the cube should go back to its previous `newPos`
* * *anything that is written in `Start()` will override what is set in the Editor`*
* * delete the line `newPos = new Vector3(1, 0, 1);`
* * run it
* * the cube should move to its new position
* let's make the cube move smoothly instead of jumping to the new position
* **annoying fact: you can't directly modify the value of a transform Vector3**
* * we need to create a temporary variable to hold the Cube's current position.
* * in `Update()`, write the following:
```
Vector3 temp;
temp = transform.position;
temp.x += 0.1f;
transform.position = temp;
```
* * run it
* * the cube should smoothly make its way out. if you don't see it, you might increase its position by too much.

**break: make the speed a public variable**
**bonus: make the sphere go in the opposite direction of the cube**

### prefab basics
* if we like our cube, we might want to save it in order to reuse it.
* in the editor, let's drag the `Cube` object, from the `Hierarchy` panel on the left, into the `Project` panel on the bottom (you might need to click on the tab, if you're still looking at `Console`)
* in the `Hierarchy`, the Cube should have turned blue. which means that now the cube is saved!
* let's make a change to the prefab in the `Project` tab (let's say, i modify the y component of its position)
* now, if we click on the cube that is already in the `Hierarchy`, we can see that the `Position` field of `Transform` is now in bold. This means that the values between the prefab (the original) and the current object differ.
* to change it back to the prefab values, simply delete the cube in the `Hierarchy` (right click > delete), and drag and drop the prefab into the `Hierarchy` again.
* we now have a brand new cube with the updated values!
* on the other hand, we can modify the values in the object present in the `Hierarchy` (such as the scale), and by clicking the button `Revert`, we go back to the previous values of the prefab.
* by clicking `Apply`, we update the prefab with the new values. do not mistake the two!

**break: make the sphere into a prefab**

### prefab creation
* let's see how we can make a prefab appear in the scene from scratch.
* * delete the cube we have in the `Hierarchy`.
* * create a new GameObject `GameObject > Create Empty`
* * call it `SceneManager`
* * add a script component (`Add Component > New Script`), and call it `ProceduralGenerator`
* * open the script in MonoDevelop by double-clicking on it
* * declare a new public GameObject variable `public GameObject myPrefab;`
* * back in the editor, we should now have a field called `My Prefab` in the `Inspector` panel of the `Scene Manager`
* * drag our prefab from the `Project` panel into that empty field
* * in `Start()`, write `GameObject.Instantiate (myPrefab as Object, Vector3.zero, Quaternion.identity);`
* * run it
* * our cube is back! notice that its position and rotation have been overwritten by the arguments passed in `Instatiate()`

**break: generate 100 cubes in various locations**

### packages
* packages are essentially libraries made into .zip files for Unity.
* we need the Google VR package in order to be able to look around.
* import GVR by clicking `assets > import packages > custom package`
* locate the `GoogleVRForUnity_1.xxx.unitypackage` and open it.
* click `Import` in the pop-up window
* we now have a `GoogleVR` folder in our `Project` tab
* go into `GoogleVR > Prefabs` and drag `GvrEditorEmulator` into our `Hierarchy`
* drag the `Camera` object onto the `GvrEditorEmulator` object, and it should become a child (sub-object) of `GvrEditorEmulator`
* run it
* by pressing `Alt` and moving the mouse, you should be able to look around!
* by pressing `Shift` and moving the mouse, you should be able to tilt!
