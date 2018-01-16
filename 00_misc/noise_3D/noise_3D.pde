float noiseCoeff = 0.2;
float spacing = 4;

void setup() {
  size(400, 400, P3D);
}

void draw() {
  background(255);
  strokeWeight(2);
  translate(width*0.5, height*0.5, 0);
  rotateX(PI/8);
  rotateY(PI/8);
  for (int x = 0; x < 15; x++) {
    for (int y = 0; y < 15; y++) {
      for (int z = 0; z < 8; z++) {
        stroke(noise(x*noiseCoeff, y*noiseCoeff, z*noiseCoeff)*255);
        pushMatrix();
        translate(x*spacing, y*spacing, z*spacing);
        sphere(2);
        popMatrix();
      }
    }
  }
  
}