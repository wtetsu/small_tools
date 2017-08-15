require "Open3"

class Bench
  attr_accessor :name
  attr_accessor :build_command
  attr_accessor :execute_command
  attr_accessor :version_command

  def initialize(name, build_command, execute_command, version_command)
    @name = name
    @build_command = build_command
    @execute_command = execute_command
    @version_command = version_command
  end
end

class Runner
  def run(bench_list)
    build_all(bench_list)
    execute_all(bench_list)
  end

  def build_all(bench_list)
    bench_list.each {|bench|
      start_process(bench.build_command)
    }
  end

  def execute_all(bench_list)
    bench_list.each {|bench|
      begin_time = Time.new
      start_process(bench.execute_command)
      end_time = Time.new
      duration = ((end_time.to_f - begin_time.to_f) * 1000).round()

      puts "* #{bench.name}"
      puts "#{duration} ms"
      puts start_process(bench.version_command)
    }
  end

  def start_process(cmd)
    o, e, s = Open3.capture3(cmd)
    if s.exitstatus != 0
      raise e
    end
    return o
  end
end

if __FILE__ == $0
  bench_list = []
  bench_list.push(Bench.new(
    "D",
    "dmd -O actor_bench.d -ofactor_bench_d",
    "./actor_bench_d",
    "dmd --version",
  ))
  bench_list.push(Bench.new(
    "C#",
    "csc -o -out:actor_bench_cs.exe actor_bench.cs",
    "./actor_bench_cs",
    "msbuild /version",
  ))
  runner = Runner.new
  runner.run(bench_list)
end
