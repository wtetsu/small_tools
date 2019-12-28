import std.stdio;
import std.conv;
import std.datetime;

void main(string[] args) {
  if (args.length <= 1) {
    return;
  }
  int num = to!int(args[1]);

  auto t1 = Clock.currTime;

  Actor[] actors = createActors(10000);
  
  for (int j = 0; j < num; j++) {
    updateAll(actors);
  }
  
  // writefln("%.14f", actors[5000].x);
  // writefln("%.14f", actors[5000].y);

  auto t2 = Clock.currTime;

  writeln( (t2.stdTime - t1.stdTime)/10000 );
}

Actor[] createActors(int num) {
  Actor[] actors;
  actors.length = num;
  for (int i = 0; i < num; i++) {
    Actor newActor = new Actor();
    newActor.x = i / 10.0;
    newActor.y = (i * 2) / 10.0;
    newActor.vx = i / 100.0;
    newActor.vy = (i * 2) / 100.0;

    actors[i] = newActor;
  }
  return actors;
}

void updateAll(Actor[] actors) {
  for (int i = 0; i < actors.length; i++) {
    actors[i].update();
  }
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
