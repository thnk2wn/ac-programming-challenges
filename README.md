# Programming Challenges

## twitter-stream
Ruby shell script (ruby program.rb) that:
* Connects to [Twitter's Streaming Sample API](https://dev.twitter.com/streaming/reference/get/statuses/sample) via [PIN based OAuth](https://dev.twitter.com/oauth/pin-based)
  ** On intital run a twitter auth url is given where a code will be generated and entered into app
* Reads sample tweets over the course of a few minutes.
* Strips urls, user mentions, emoticons, stop words and other common words and produces unique word list.
* Presents total word count and top 10 word 'trends' with occurence count for each.
* My first Ruby script beyond Hello World :)

