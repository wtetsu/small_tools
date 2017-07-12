package main

import "fmt"
import "time"

func plus(a, b int)(int, int, int) {
  return a, b, a+b;
}

type Actor struct {
	x float32
	y float32
	vx float32
	vy float32
}

func update(actor *Actor) {
	actor.x += actor.vx;
	actor.y += actor.vy;
}

func main() {
	time0 := time.Now()

	const LEN = 10000
	const TIMES = 30000
	list := make([]*Actor, LEN)

	for i := 0; i < LEN; i++ {
		newActor := Actor{}
		newActor.x = 0
		newActor.y = 0
	    newActor.vx = 0.000001 * float32(i)
	    newActor.vy = 0.000002 * float32(i)
		list[i] = &newActor
	}

	for i := 0; i < TIMES; i++ {
		for j := 0; j < LEN; j++ {
			update(list[j])
		}
	}

	time1 := time.Now()
	dur := time1.Sub(time0)

	fmt.Println(dur)

	fmt.Println(list[5000].x)
	fmt.Println(list[5000].y)
}
