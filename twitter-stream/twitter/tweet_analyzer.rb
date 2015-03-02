require_relative '../twitter/twitter_auth'
require_relative '../twitter/tweet_parser'

# tweetstream feels a little heavy - lot of functionality but might consider doing more raw http/json type work ourself
require 'tweetstream'

require 'colorize'

class TweetAnalyzer

  def initialize(consumerKey, consumerSecret, oauthToken = nil)
    # class instance variables
    @consumerKey = consumerKey
    @consumerSecret = consumerSecret
    @isConnected = false
    @oauthToken = oauthToken
  end

  def connect
    authenticate
    @isConnected = true
    @oauthToken
  end

  def analyze(waitMinutes = 5)
    if (!@isConnected)
      raise 'connect first'
    end

    tweetParser = TweetParser.new
    wordCounts = Hash.new
    stopPending = false
    startTime = Time.now

    TweetStream::Client.new.on_error do |message|
      puts "ERROR => #{message}"
      raise message
    end.sample(language: 'en') do |status,client|
      elapsed = Time.now - startTime
      remainingSeconds = (waitMinutes * 60 - elapsed.to_i).round

      if (remainingSeconds < 1)
        if (!stopPending)
          puts 'stopping'.colorize(:yellow)
          client.stop()
          stopPending = true
        end
      else
        if (remainingSeconds % 10 == 0)
          puts "#{remainingSeconds}s left : #{status.text}".colorize(:cyan)
        else
          puts "#{status.text}"
        end

        words = tweetParser.get_clean_words(status)
        words.each do |word|
          if (!wordCounts.has_key? word)
            wordCounts[word] = 1
          else
            wordCounts[word] = wordCounts[word]+1
          end
        end
      end # remainingSeconds
    end # twitter sample stream

    # we could combine sort & take with [0..9]] in same line but for debug & output, helpful to have whole thing sorted
    @sortedWordCounts = Hash[wordCounts.sort_by { |k,v| -v }]
    @topWords = Hash[@sortedWordCounts.sort_by { |k,v| -v }[0..9]]
  end # analyze

  def word_counts
    @sortedWordCounts
  end

  def top_words
    @topWords
  end

  def to_s
    s = StringIO.new
    s << "Total words: #{@sortedWordCounts.length}\n\n"
    s << "Top Words:\n"

    @topWords.each_with_index do |(key,value),index|
      s << "#{index+1}) #{key}  #{value} times\n"
    end

    s.string
  end

  # ---- private methods ----
  private
  def authenticate
    # if we don't have a token from before, create one
    if (!@oauthToken)
      auth = TwitterAuth.new(@consumerKey, @consumerSecret)
      @oauthToken = auth.authorize()
    end

    TweetStream.configure do |cfg|
      cfg.auth_method = :oauth
      cfg.consumer_key = @consumerKey
      cfg.consumer_secret = @consumerSecret

      # we could be lazy and plug in our own access token from https://apps.twitter.com/app/{appId}/keys
      # but that's not a good/secure idea
      cfg.oauth_token = @oauthToken.token
      cfg.oauth_token_secret = @oauthToken.secret
    end
  end

end # class TweetAnalyzer