require 'minitest/autorun'
require 'minitest/reporters'
MiniTest::Reporters.use!

# http://visibletrap.blogspot.com/2013/05/rubymine-cant-find-testhelper.html
require_relative '../common/text_utility'

class TextUtilityTest < Minitest::Test
  def test_strip_ending_punctuation
    assert_equal('it', TextUtility::remove_end_punctuation('it!'))
    assert_equal('Yup', TextUtility::remove_end_punctuation('Yup.'))
    assert_equal('Yes', TextUtility::remove_end_punctuation('Yes?'))
    assert_equal('it was great! Yes', TextUtility::remove_end_punctuation('it was great! Yes!'))
    assert_equal('and so it goes. so they say', TextUtility::remove_end_punctuation('and so it goes. so they say.'))
  end

  def test_remove_emoji
    assert_equal('Good morning from Washington, D.C. ',
                 TextUtility::remove_emoji('Good morning from Washington, D.C. 🐶☕️'))

    assert_equal(' O just a few of my faces during #TotalDivas green screens  Make sure to tune in tonight for an… https://instagram.com/p/zsMi5hmzR6/',
                 TextUtility::remove_emoji('😂😂 O just a few of my faces during #TotalDivas green screens 😁 Make sure to tune in tonight for an… https://instagram.com/p/zsMi5hmzR6/'))

    assert_equal('no emojis here', TextUtility::remove_emoji('no emojis here'))
  end

  def test_strip_url
    assert_equal('', TextUtility::strip_url('https://instagram.com/p/zsMi5hmzR6/'))
    assert_equal('This is a  url then text',
                 TextUtility::strip_url('This is a https://instagram.com/p/zsMi5hmzR6/ url then text'))
    assert_equal("This is a test with no no urls here: /partial/",
                 TextUtility::strip_url("This is a test with no no urls here: /partial/"))
  end
end