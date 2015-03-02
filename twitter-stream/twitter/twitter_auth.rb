# just in case old version of ruby (<= 1.8?)
require 'rubygems'

# using twitter_oauth ruby gem for twitter auth because whey reinvent the wheel
require 'twitter_oauth'

class TwitterAuth
  # Initializes twitter authorization class
  # Params:
  # +consumerKey+:: twitter app consumer key
  # +consumerSecret+:: twitter app consumer secret
  # +accessToken+:: access token, if any, from past successful authorization
  def initialize(consumerKey, consumerSecret, accessToken = nil)
    # class instance variables
    @accessToken = accessToken
    @client = TwitterOAuth::Client.new(
        :consumer_key => consumerKey,
        :consumer_secret => consumerSecret,
        :accessToken => accessToken
    )
  end

  def authorize
    if @client.authorized?
      # don't see access token property on client, just return last value (i.e. ctor)
      return @accessToken
    end

    # oob indicates pin based authentication (user goes to twitter grabs pin, returns and plugs in)
    # request an authentication token
    puts 'Authenticating with Twitter'
    request_token = @client.authentication_request_token(
        :oauth_callback => 'oob'
    )

    puts 'Visit the following web page and copy the access code: '
    puts request_token.authorize_url

    # prompt user for code from above url user went to and trim whitespace from their input
    print 'Enter the access code: '
    code = gets.strip

    # authorize using token and code
    @accessToken = @client.authorize(
        request_token.token,
        request_token.secret,
        :oauth_verifier => code
    )

    puts "Authenticated with #{@accessToken}"
    # return access token
    @accessToken
  end
end  