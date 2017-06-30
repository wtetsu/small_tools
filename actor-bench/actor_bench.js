var Actor = function() {
  this.x = 0;
  this.y = 0;
  this.vx = 0;
  this.vy = 0;
};
Actor.prototype.update = function() {
  this.x += this.vx;
  this.y += this.vy;
};

var time1 = new Date();

var LEN = 10000;
var TIMES = 30000;

var i, j;

list = [];
list.length = LEN;
for (i = 0; i < LEN; i++) {
  var a = new Actor();
  a.vx = 0.000001 * i;
  a.vy = 0.000002 * i;
  list[i] = a;
}

for (j = 0; j < TIMES; j++) {
  for (i = 0; i < LEN; i++) {
    var a = list[i];
    a.update();
  }
}

var time2 = new Date();

function puts(msg) {
  console.warn(msg);
}

puts(list[5000].x);
puts(list[5000].y);

//var time = (time2.getTime() - time1.getTime());
//var div = document.getElementById("area");
//div.innerText = time.toString();

/*
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
  public float x = 0;
  public float y = 0;
  public float vx = 0;
  public float vy = 0;
  
  public void update() {
    this.x += this.vx;
    this.y += this.vy;
  }
}
*/
