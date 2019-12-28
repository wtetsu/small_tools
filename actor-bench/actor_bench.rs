use std::time::Instant;

const LEN: usize = 10000;
const TIMES: usize = 30000;

fn main() {
  let now = Instant::now();

  let mut list : Vec< Actor> = Vec::new();

  for i in 0..LEN {
    let f = i as f64;
    list.push( Actor{x:0.0, y:0.0, vx:0.000001*f, vy:0.000002*f});
  }

  for _i in 0..TIMES {
    for j in 0..LEN {
      let actor = &mut list[j];
      actor.update();
    }
  }

  let new_now = Instant::now();
    
  println!("{}", list[5000].x);
  println!("{}", list[5000].y);
  println!("{:?}", new_now.duration_since(now));  
}

struct Actor {
  pub x : f64,
  pub y : f64,
  pub vx : f64,
  pub vy : f64,
}

impl Actor {
  pub fn update(&mut self) {
    self.x += self.vx;
    self.y += self.vy;
  }
}
