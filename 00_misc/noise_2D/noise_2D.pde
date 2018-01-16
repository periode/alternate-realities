float noiseCoeffX = 0.01;
float noiseCoeffY = 0.0001;

float stepX = 0.01;

void setup() {
  size(800, 800);
  background(255);
  strokeWeight(2);

}

void draw() {
  
    for (int x = 0; x < width; x+=10) {
    for(int y = 0; y < height; y+=10){
      stroke(noise(x*noiseCoeffX, y*noiseCoeffX, stepX)*255);
      //stroke(random(255));
      ellipse(x, y, noise(x*noiseCoeffX, y*noiseCoeffX, stepX)*10, noise(x*noiseCoeffX, y*noiseCoeffX, stepX)*10);
    }
  }
  
  stepX += 0.1;
}