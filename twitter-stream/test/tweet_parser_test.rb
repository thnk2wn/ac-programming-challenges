require 'minitest/autorun'
require 'minitest/reporters'
MiniTest::Reporters.use!

# http://visibletrap.blogspot.com/2013/05/rubymine-cant-find-testhelper.html
require_relative '../twitter/tweet_parser'

class TweetParserTests < Minitest::Test
  def setup
    @parser = TweetParser.new
  end

  def test_stopwords_personal_pronouns
    words1 = ['I', 'i', 'You', 'YOU', 'you', 'u', 'we', 'We', 'WE', 'us', 'Us',
              'them', 'Them', 'They', 'they', 'He', 'he', 'She', 'she', 'Me', 'me',
              'Her', 'her', 'it', 'It' ]
    words2 = @parser.remove_stop_words(words1)
    assert_equal(0, words2.length)
  end

  def test_retweet_removal
    assert_equal('Learning some Ruby',
                 TweetParser::remove_retweet('RT @thnk2wn: Learning some Ruby'))
    assert_equal('Learning some Ruby',
                 TweetParser::remove_retweet('RT @thnk2wn Learning some Ruby'))
  end

  def test_clean_words
    items = ['Something!', 'Emoticon 😁', 'http://t.co/foo', 'Just your normal stuff. Ya know']
    items2 = @parser.clean_words(items)
    assert_equal('Emoticon ', items2[0])
    assert_equal(2, items2.length)
  end

end