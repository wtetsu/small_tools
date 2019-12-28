class Actor
    attr_accessor :x
    attr_accessor :y
    attr_accessor :vx
    attr_accessor :vy

    def update
      @x += @vx
      @y += @vy
    end
end

def create_actors(num)
  instances = []
  num.times {|i|
    new_actor = Actor.new
    new_actor.x = i/10.0
    new_actor.y = i*2/10.0
    new_actor.vx = i/100.0
    new_actor.vy = i*2/100.0
    instances.push(new_actor)
  }
  return instances
end

def update_all(actors)
  actors.each {|actor|
    actor.update()
  }
end


actors = create_actors(10000)
10000.times {
  update_all(actors)
}

puts actors[5000].x
puts actors[5000].y
