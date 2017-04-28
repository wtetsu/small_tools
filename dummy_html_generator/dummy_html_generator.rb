require "Find"
require "FileUtils"

template = File.read("template.html")

Find.find("in") {|path|
  next if !path.end_with?(".html")
  rpath = path.sub("in/", "")
  out_path = "out/#{rpath}"
  out_dir_path = File.dirname(out_path)
  FileUtils.mkdir_p(out_dir_path)
  
  new_link = "http://newsite.com/#{rpath}"
  new_html = template.sub("$LINK$", new_link)
  File.write(out_path, new_html)
  puts out_path
}
