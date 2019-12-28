import sys
import time


class Actor:
    def __init__(self):
        self.x = 0
        self.y = 0
        self.vx = 0
        self.vy = 0

    def update(self):
        self.x += self.vx
        self.y += self.vy


def create_actors(num):
    instances = []
    for i in range(0, num):
        new_actor = Actor()
        new_actor.x = i/10.0
        new_actor.y = i*2/10.0
        new_actor.vx = i/100.0
        new_actor.vy = i*2/100.0
        instances.append(new_actor)
    return instances


def update_all(actors):
    for actor in actors:
        actor.update()


if __name__ == '__main__':
    if len(sys.argv) <= 1:
        sys.exit(1)
    num = int(sys.argv[1])

    start_time = time.time()

    actors = create_actors(10000)
    for i in range(0, num):
        update_all(actors)

    print((time.time() - start_time)*1000)

    # print(actors[5000].x)
    # print(actors[5000].y)
