class TextFileReader
  def self.get_lines(file_path)
    text = []
    File.read(file_path).each_line do |line|
      text << line.chop
    end
    text
  end
end