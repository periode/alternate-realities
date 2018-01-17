### import a file into unity

### image files

* open a new project
* create a new Game Object (`GameObject > 3D Object > Cube`)
* a cube should appear in your scene view
* click `File > Save Scenes` in order to save the scene. A scene file should appear in your `Project` tab in your `Assets` folder.
* create two new folder in which we're going to store our external files. right-click in the `Project` tab and `Create > Folder`
* name one `Materials` and the other one `Textures`.
* find an image that you like (on your hard drive/the web)
* drag and drop it directly inside the `Textures` folder of your Unity Editor
* go to your `Materials` folder
* `Create > Material` and call it `cubemat`
* in the `Inspector` panel we can change the *shader* (how the material looks like)
* at the top, select `Mobile > Diffuse` in the dropdown menu
* now, in the top square on the right that says `None (Texture)` we need to select a Texture
* click select and select our existing texture
* now our material is skinned with our texture!
* click on the cube in the `Hierarchy`
* go to the `Mesh Renderer` component, then to the `Materials` section
* click on the dotted circle next to `Default-Material`
* select our `cubemap`
* our cube is now skinned with our texture!

### sound files

* create a folder named `Audio`
* find an audio file that you like and drag and drop it into that folder
* convert it to Mono in the `Inspector` panel and click `Apply`
* in order to play that file, we first need to attach it to a GameObject
* create a new GameObject `GameObject > Create Empty`
* rename it `AudioManager`
* in the `Inspector` panel, click `Add Component`
* select `Audio > Audio Source`
* in the `Audio Source` component, assign the clip by either clicking on the dotted circle or drag and dropping the file into the empty field that says `None (Audio Clip)`
* make sure that `Play On Awake` is checked
* run it
* the sound should be playing!
* let's uncheck `Play On Awake` for now

### physics collider

* let's create a new sphere `GameObject > 3D Object > Sphere`
* move the sphere a little along the X axis, so that it doesn't overlap with the cube
* by creating objects this ways, we see that a `Collider` component gets automatically added
* run it
* switch to the scene tab
* move the sphere into the cube using the red arrow
* the sphere should be going through the cube, without colliding
* what we need is a rigidbody on one of those objects
* on the sphere, click `Add Component > Physics > Rigidbody`
* run it
* the sphere falls infinitely!
* we need a plane to hold it: `GameObject > 3D Object > Plane`
* move it a little below our sphere and cube
* run it
* because of the collider on the plane, now the sphere doesn't go through anymore
* while still playing, switch to the `Scene` tab and move the sphere against the cube. it should resist a little, then eventually teleport through.
* now, reset the scene (stop and play again)
* in the `Scene` tab, move the cube against the sphere. if you go slow enough (i.e. continuous movement), you should be able to push the sphere off the edge!
* in a nutshell: colliders react to the physics engine, and rigidbodies simulate realistic physics (mass, gravity, etc.)


**break: create another game object that you will use to push the other two off the plane**
### keyboard input
* because it's annoying to move things around with the arrows, let's create a keyboard input manager
* on the sphere, create a new script `Add Component > New Script` and call it `MovementManager`
* open the script in MonoDevelop by double clicking on it
* in the `Update()`, add the following
```
  if(Input.GetKeyDown(KeyCode.UpArrow)){
  	transform.position = new Vector3 (transform.position.x, transform.position.y, transform.position.z + 0.1f);
  }

  if(Input.GetKeyDown(KeyCode.DownArrow)){
  	transform.position = new Vector3 (transform.position.x, transform.position.y, transform.position.z - 0.1f);
  }

  if(Input.GetKeyDown(KeyCode.LeftArrow)){
  	transform.position = new Vector3 (transform.position.x + 0.1f, transform.position.y, transform.position.z);
  }

  if(Input.GetKeyDown(KeyCode.RightArrow)){
  	transform.position = new Vector3 (transform.position.x - 0.1f, transform.position.y, transform.position.z);
  }
```
**break: now do the same with the sphere, but with different keys!**

### triggers

* with your new object (a cylinder), tick the `Is Trigger` check box in the `Collider` component
* add a new script called `ColliderManager.cs` on the cylinder
* below `Update()`, we write a function to check if another object enters the collider of the cylinder
* ```
void OnTriggerEnter(Collider col){
	Debug.Log ("something entered: "+col);
}
```
* run it
* again, we move the cube into the cylinder, and nothing happens in our console
* if we move the sphere into the cylinder, we get a message in our console!
* this is because *in order for a collision to be detected, the colliding object must have a rigidbody*
* in order to print the colliding object's name, we can write `Debug.Log("collided with "+col.gameObject.name)`
* now, let's play a sound whenever we cross that trigger
* in the script, let's write an if statement:
```
if(col.gameObject.name == "Sphere"){

}
```
* and inside, we need to do multiple things
* * find the audio manager: `GameObject.Find("AudioManager")`
* * get its component `GetComponent<AudioSource>()`
* * play it: `Play()`
* * so the total line of code is `GameObject.Find ("AudioManager").GetComponent<AudioSource> ().Play ();`
* run it
* if you move the sphere through the cylinder, it should work!
* if you move the cube through the cylinder, it should not work!


### tags

* we can replace the name by `Tags` so that a specific action can apply to multiple game objects
* select the object you want to tag -e.g. the sphere.
* in the `Inspector` panel, clikc on the `Tag` dropdown menu and select `Add Tag...`
* in the new panel, click on the `+` icon on the right
* call it `Colliding`
* select the sphere again
* in the `Tag` dropdown menu you can now select the `Colliding` tag
* do the same thing for the other object -e.g. the cube
* change the code in `ColliderManager` so that the if statement checks for the tag of the gameObject
* `if(col.gameObject.tag == "Colliding"){`
* add a `Rigidbody` component to the cube
* run it
* now, both game objects should be able to trigger the sound!
