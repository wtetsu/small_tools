class Actor {
  constructor() {
    this.x = 0;
    this.y = 0;
    this.vx = 0;
    this.vy = 0;
  }

  update() {
    this.x += this.vx;
    this.y += this.vy;
  }
}

const createActors = num => {
  const actors = [];
  for (let i = 0; i < num; i++) {
    var newActor = new Actor();
    newActor.x = i / 10.0;
    newActor.y = (i * 2) / 10.0;
    newActor.vx = i / 100.0;
    newActor.vy = (i * 2) / 100.0;
    actors.push(newActor);
  }
  return actors;
};

const updateAll = actors => {
  for (i = 0; i < actors.length; i++) {
    const actor = actors[i];
    actor.update();
  }
};

const main = () => {
  if (process.argv.length <= 2) {
    return 1;
  }
  const start = new Date().getTime();

  const actors = createActors(10000);

  const num = parseInt(process.argv[2], 10);
  for (j = 0; j < num; j++) {
    updateAll(actors);
  }

  console.info(new Date().getTime() - start);

  console.info(actors[5000].x);
  console.info(actors[5000].y);
};

main();
