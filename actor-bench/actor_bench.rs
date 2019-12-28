use std::time::Instant;
use std::env;
const LEN: usize = 10000;

fn main() {
  let args: Vec<String> = env::args().collect();
  if args.len() <= 1 {
    return;
  }

  let num = args[1].parse().unwrap();

  let now = Instant::now();

  let mut list : Vec< Actor> = Vec::new();

  for i in 0..LEN {
    let f = i as f64;
    list.push( Actor{x:f/10.0, y: f*2.0/10.0, vx:f/100.0, vy: f*2.0/100.0});
  }

  for _i in 0..num {
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
