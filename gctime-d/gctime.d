import core.stdc.stdlib;
import core.memory : GC;
import std.stdio;
import std.datetime;

void main() {
  measure1(); // Actor without GC scan
  GC.collect;
  measure2(); // Normal Actor
}

void measure1() {
  const int LEN = 50000;
  Actor1[] list;
  list.length = LEN;
  for(int i = 0; i < LEN; i++) {
    list[i] = new Actor1();
  }
  auto tm1 = Clock.currTime;
  for(int i = 0; i < 10000; i++) {
    GC.collect;
  }
  auto tm2 = Clock.currTime;
  writeln(tm2 - tm1);
  list.length = 0;
}

void measure2() {
  const int LEN = 50000;
  Actor2[] list;
  list.length = LEN;
  for(int i = 0; i < LEN; i++) {
    list[i] = new Actor2();
  }
  auto tm1 = Clock.currTime;
  for(int i = 0; i < 10000; i++) {
    GC.collect;
  }
  auto tm2 = Clock.currTime;
  writeln(tm2 - tm1);
  list.length = 0;
}

// Actor without GC scan
// https://wiki.dlang.org/Memory_Management
// https://dlang.org/library/core/memory/gc.add_range.html
class Actor1 {
  public float X;
  public float Y;
  public float Vx;
  public float Vy;
  new(size_t sz) {
    void* p;
    p = malloc(sz);
    if (!p) {
       throw new Error("malloc error.");
    }
    //GC.addRange(p, sz);
    return p;
  }
  delete(void* p) {
    if (p) {
      //GC.removeRange(p);
      free(p);
    }
  }
}

// Normal Actor
class Actor2 {
  public float X;
  public float Y;
  public float Vx;
  public float Vy;
}


