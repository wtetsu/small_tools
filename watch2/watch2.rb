#! /usr/local/opt/ruby/bin/ruby

require "open3"
require "systemu"

if ARGV.length < 2
  puts "Usage: command executable watchfile"
  exit 1
end

def kill(pid)
  begin
    Process.kill("TERM", pid)
  rescue
  end
end


def get_mtimes(files)
  r = {}
  files.each{|file|
    r[file] = File.mtime(file)
  }
  return r.freeze
end

def main(command, watch_files)
  mtimes = {}

  while true
    new_mtimes = get_mtimes(watch_files)
    if mtimes != new_mtimes
      puts "[#{Time.new}]"

      status, stdout, stderr = systemu(command) {|pid|
        # puts "# run background #{pid}"
        while true
          if new_mtimes != get_mtimes(watch_files)
            kill(pid)
            break
          end
          sleep 0.25
        end
      }
      puts stdout if !stdout.empty?
      puts stderr if !stderr.empty?

      mtimes = new_mtimes
    end
    sleep 0.25
  end
end

if __FILE__ == $0
  command = ARGV[0]
  watchfile_patterns = ARGV[1..ARGV.length-1]
  watch_files = watchfile_patterns.map{|p|Dir[p]}.flatten.select{|f|File.file?(f)}
  puts watch_files

  main(command, watch_files)
end
