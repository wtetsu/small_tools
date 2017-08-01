require 'csv'

class EcsvParser
  def exec
    sum_list, all_products = find_and_parse_csvs()

    all_products.each {|product|
      line = [product]
      
      sum_list.each {|sum|
        line.push(sum[product])
      }
      
      puts line.join("\t")
    }
  end
  
  def parse(csv_file)
    sum = Hash.new(0)
    csv_data = CSV.read(csv_file, headers: true)
    csv_data.each do |r|
      if r["RecordType"] == "LinkedLineItem" && r["ProductName"].length > 0
        cost = r["TotalCost"].to_f
        sum[r["ProductName"]] += cost
      end
    end
    return sum
  end

  def find_and_parse_csvs
    sum_list = []
    all_products = []
    Dir.glob("*.csv") {|csv_file|
      sum = parse(csv_file)
      sum_list.push(sum)
      all_products += sum.keys
    }
    all_products.sort!.uniq!
    return sum_list, all_products
  end
end


EcsvParser.new.exec
