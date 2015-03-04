require 'stopwords'
require_relative '../common/text_file_reader'
require_relative '../common/text_utility'

class TweetParser
  def initialize
    # making absolute for different working directories (i.e. test vs run main script)
    dir = File.expand_path(File.dirname(__FILE__))
    @additional_remove_words = TextFileReader::get_lines("#{dir}/../settings/extra_ignore_words.txt")
  end

  def get_clean_words(tweet_object)
    cleaned_status_text = strip_tweet_retweet_mentions_urls(tweet_object)
    items = remove_stop_words(cleaned_status_text)
    items = clean_words(items)

    # finally remove any duplicates in array
    items = items.uniq
    items
  end

  def clean_words(words)
    cleaned_words = Array.new()
    words.each do |word|
      cleaned = clean_word(word)

      if (cleaned && cleaned.length > 0)
        cleaned_words.push(cleaned)
      end
    end
    cleaned_words
  end

  def clean_word(word)
    result = TextUtility::remove_end_punctuation(word)
    result = TextUtility::remove_emoji(result)
    result = TextUtility::strip_url(result)

    # stripped/changed word might now be a stopword / additional word to remove
    if !(include_word? result)
      result = nil
    end

    result
  end

  def remove_stop_words(source)
    words = source.kind_of?(Array) ? source : source.split

    # we could do our own dictionary of stop words but this gem has many common ones already
    filter = Stopwords::Snowball::Filter.new "en"
    items = filter.filter words

    items.delete_if do |item|
      item_lower = item.downcase
      # stopword-filter doesn't handle case so we lower and check here too
      should_delete = (@additional_remove_words.include? item_lower) || (filter.stopword? item_lower)
      should_delete
    end

    items
  end

  def include_word? (word)
    word_check = word.downcase
    filter = Stopwords::Snowball::Filter.new "en"
    skip = (filter.stopword? word_check) || (@additional_remove_words.include? word_check)
    !skip
  end

  def self.remove_retweet(source)
    target = source.dup

    # if tweet is in form "RT @username: blah blah" then remove "RT @username:"
    patterns = Array.new(2)
    patterns[0] = /RT @([a-z0-9_]+:)/i
    patterns[1] = /RT @([a-z0-9_]+ )/i

    patterns.each do |pattern|
      match = pattern.match(source)

      if (match)
        target = match.post_match.lstrip
      end
    end

    target
  end

  def strip_tweet_retweet_mentions_urls(tweet_object)
    text_parsed = TweetParser::remove_retweet(tweet_object.text.dup)

    if (tweet_object.user_mentions.length > 0)
      tweet_object.user_mentions.each do |mention|
        # remove any @username mention in tweet text (assumed behavior)
        replace = "@#{mention.screen_name}"
        text_parsed = text_parsed.gsub /#{replace}/i, ''
      end
    end

    if (tweet_object.urls.length > 0)
      tweet_object.urls.each do |url|
        # remove any hyperlink in tweet text (assumed behavior)
        text_parsed = text_parsed.gsub /#{url.url}/i, ''
      end
    end

    # assuming #hashtags should be included

    text_parsed
  end
end