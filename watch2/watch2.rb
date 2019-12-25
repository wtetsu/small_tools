#! /usr/local/opt/ruby/bin/ruby

require "open3"
require "systemu"

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

EXECUTABLES = {
  ".py" => "python",
  ".rb" => "ruby",
  ".js" => "node",
  ".sh" => "sh",
}

if __FILE__ == $0
  if ARGV.length < 1
    puts "Usage: command executable watchfile"
    exit 1
  end

  if ARGV.length == 0
    exit 1
  elsif ARGV.length == 1
    watchfile = ARGV[0]
    ext = File.extname(watchfile).downcase
    executable = EXECUTABLES[ext]
    if executable.nil?
      exit 1
    end
    command = "#{executable} #{watchfile}"
    watchfile_patterns = [watchfile]
  else
    command = ARGV[0]
    watchfile_patterns = ARGV[1..ARGV.length-1]
  end
  watch_files = watchfile_patterns.map{|p|Dir[p]}.flatten.select{|f|File.file?(f)}
  puts watch_files

  main(command, watch_files)
end
