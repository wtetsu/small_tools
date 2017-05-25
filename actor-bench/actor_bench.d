import std.stdio;
import std.datetime;

void main() {
  auto t1 = Clock.currTime;
  go();
  auto t2 = Clock.currTime;
  writeln( t2 - t1 );
}

void go() {
  const int LEN = 10000;
  const int TIMES = 30000;
  
  Actor[] list;
  list.length = LEN;
  for (int i = 0; i < LEN; i++) {
    Actor a = new Actor();
    a.vx = 0.000001 * i;
    a.vy = 0.000002 * i;
    list[i] = a;
  }
  
  for (int j = 0; j < TIMES; j++) {
    for (int i = 0; i < LEN; i++) {
      Actor a = list[i];
      a.update();
    }
  }
  
  writeln(list[5000].x);
  writeln(list[5000].y);
}


class Actor {
  public double x = 0;
  public double y = 0;
  public double vx = 0;
  public double vy = 0;
  
  public void update() {
    this.x += this.vx;
    this.y += this.vy;
  }
}
