float noiseCoeff = 0.02;

void setup() {
  size(400, 400);
  background(255);
  strokeWeight(2);
  for (int x = 0; x < width; x++) {
    for(int y = 0; y < height; y++){
      stroke(noise(x*noiseCoeff, y*noiseCoeff)*255);
      ellipse(x, y, 2, 2);
    }
  }
}

void draw() {
}