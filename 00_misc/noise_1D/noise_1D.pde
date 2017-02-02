float step = 0.05;
float noiseVal = 0;
float noiseCoeff = 200;

void setup() {
  size(1000, 600);
  background(255);
  beginShape();
  for (float i = 0; i < width; i++) {
    vertex(i, noise(noiseVal)*noiseCoeff+(height-noiseCoeff)*0.5);
    noiseVal+=step;
  }
  endShape();
}

void draw() {
}