package main

import (
	"fmt"
	"os"
	"strconv"
	"time"
)

func plus(a, b int) (int, int, int) {
	return a, b, a + b
}

type Actor struct {
	x  float32
	y  float32
	vx float32
	vy float32
}

func update(actor *Actor) {
	actor.x += actor.vx
	actor.y += actor.vy
}

func main() {
	if len(os.Args) <= 1 {
		return
	}

	num, err := strconv.Atoi(os.Args[1])

	if err != nil {
		return
	}

	start := time.Now()

	const LEN = 10000
	list := make([]*Actor, 10000)

	for i := 0; i < LEN; i++ {
		newActor := Actor{}
		newActor.x = float32(i) / 10.0
		newActor.y = (float32(i) * 2) / 10.0
		newActor.vx = float32(i) / 100.0
		newActor.vy = (float32(i) * 2) / 100.0

		list[i] = &newActor
	}

	for i := 0; i < num; i++ {
		for j := 0; j < LEN; j++ {
			update(list[j])
		}
	}

	fmt.Println(time.Since(start).Nanoseconds() / 10000)

	// fmt.Println(list[5000].x)
	// fmt.Println(list[5000].y)
}
