require './twitter/tweet_analyzer'
require './settings/app_config'
require './settings/app_data'
require 'colorize'

class Program
  def self.run
    app_data = AppData.new()
    app_data.read

    analyzer = TweetAnalyzer.new(
        AppConfig::TWITTER[:consumer_key],
        AppConfig::TWITTER[:consumer_secret],
        app_data.token)
    oauth_token = analyzer.connect
    AppData::write oauth_token

    analyzer.analyze(AppConfig::GENERAL[:wait_time_minutes])

    puts analyzer.to_s.colorize(:magenta)

    #TODO: Optional Part B: serialize total words with settings into app data file to pick up where we left off later
  end
end

Program::run
