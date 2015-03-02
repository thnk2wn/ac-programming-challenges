class TextUtility
  def self.strip_url(text)
    regex = /(?:f|ht)tps?:\/[^\s]+/
    result = text.sub regex, ''
  end

  def self.remove_end_punctuation(text)
    result = text.sub!(/[?.!,;]?$/, '')
    result
  end

  def self.remove_emoji(text)
    text = text.force_encoding('utf-8').encode
    clean_text = ""

    # emoticons  1F601 - 1F64F
    regex = /[\u{1f600}-\u{1f64f}]/
    clean_text = text.gsub regex, ''

    #dingbats 2702 - 27B0
    regex = /[\u{2702}-\u{27b0}]/
    clean_text = clean_text.gsub regex, ''

    # transport/map symbols
    regex = /[\u{1f680}-\u{1f6ff}]/
    clean_text = clean_text.gsub regex, ''

    # enclosed chars  24C2 - 1F251
    regex = /[\u{24C2}-\u{1F251}]/
    clean_text = clean_text.gsub regex, ''

    # symbols & pics
    regex = /[\u{1f300}-\u{1f5ff}]/
    clean_text = clean_text.gsub regex, ''

    clean_text

  end
end