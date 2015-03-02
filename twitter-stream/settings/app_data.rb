require 'json'
require 'twitter_oauth'

class AppData
  FILENAME = './settings/settings.json'

  def self.write(oauth_token)
    File.open(FILENAME,'w') do |f|
      settings = {
          'twitter_token' => oauth_token.token,
          'twitter_secret' => oauth_token.secret
      }
      f.write(settings.to_json)
    end
  end

  def token
    @token
  end

  def data
    @data
  end

  def read
    if (!File.exists? (FILENAME))
      return nil
    end

    file = File.read(FILENAME)
    @data = JSON.parse(file)
    @token = OAuth::Token.new(@data['twitter_token'], @data['twitter_secret'])
    @data
  end
end