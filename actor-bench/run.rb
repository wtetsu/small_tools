require "Open3"

class Runner
  def run
  end
end

class Builder
  def run
    start_process("dmd -O actor_bench.d")
  end

  def start_process(cmd)
    o, e, s = Open3.capture3(cmd)
    if s.exitstatus != 0
      raise e
    end
  end
end

Builder.new.run
Runner.new.run

