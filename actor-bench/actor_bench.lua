
LEN = 10000
TIMES = 30000

list = {}

function createActor(vx, vy)
  a = {}
  a.x = 0.0;
  a.y = 0.0;
  a.vx = vx;
  a.vy = vy;
  return a
end

function init()
  for i = 0, LEN do
    list[i] = createActor(0.000001 * i, 0.000002 * i)
  end
end

function update(actor)
  actor.x = actor.x + actor.vx
  actor.y = actor.y + actor.vy
end

function updateAll()
  for i = 0, LEN do
    update(list[i])
  end
end

init()
for i = 0, TIMES do
  updateAll()
end

print(list[5000].x)
print(list[5000].y)
