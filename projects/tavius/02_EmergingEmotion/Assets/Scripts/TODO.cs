

/*

Radial Menu:
- Fix teleport controls overlap.
  - Way to turn off teleport controls when menu is open?
- Materialize emotions at controller, if made via menu.
  - Separate function, one takes gameObject transform, the other the controller transform.
- Add script to 'unlock' emotions on the radial menu.


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