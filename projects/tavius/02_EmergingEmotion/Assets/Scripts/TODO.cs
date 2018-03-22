

/*
KEY TASKS:
 1) Make Joy feel nice.
 * Add audio source to play laughter.
 * Add particle effects.
 * Special lighting/texture.
 
 2) Swap environments when emotions are put to face.
 * Make transition screen. (A plane/UI with that color attaches to Camera(Eye) for 5ish seconds)
 * New terrain is placed below user.
 * Old terrain is disabled.
 * New audio fades in.
 * New light fades in.
 * Blocking plane/UI fades away.
 
 3) Polish.
 * Make the rest of the emotions feel accurate.
 * Fix bugs.
 * Tweak environments.
 * Add script to 'unlock' emotions on the radial menu?
 * Secondary emotions affect the environment. (Balloons, surprise chests, etc.)




BUG:
- When same same base emotions combine (e.g. JOY+JOY, SAD+SAD, FEA+FEA) two copies are made, but only one has the new emotionType applied. Extra confusing when only one should be made in the first place.

 
 


// Name them accordingly:
		
//      JOY
//      SAD
//      FEA
//		ECS  J+J = Ecstasy    
//		DES  S+S = Despair    
//		SUR  J+F = Surprise   
//		TER  F+F = Terror     
//		ANX  F+S = Anxiety    
//		MEL  J+S = Melancholy 

// change Mesh Renderer shadow properties for some?

// Note the VRTK Chest Control Component

*/
