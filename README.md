# Programming Challenges

## twitter-stream
Ruby shell script (ruby program.rb) that:
* Connects to [Twitter's Streaming Sample API](https://dev.twitter.com/streaming/reference/get/statuses/sample) via [PIN based OAuth](https://dev.twitter.com/oauth/pin-based)
  * On intital run a twitter auth url is given where a code will be generated and entered into app
* Reads sample tweets over the course of a few minutes.
* Strips urls, user mentions, emoticons, stop words and other common words and produces unique word list.
* Presents total word count and top 10 word 'trends' with occurence count for each.
* My first Ruby script beyond Hello World :)

## Mobile.Client

* Android Phone app built using Xamarin and C#
  ** If no software to run, screencast or Skype demo is available
* Partial security - frontend, simulated backend
* Allows taking a picture w/camera, uploading to cloud
* Currently fetches pictures/docs from cloud and displays count
  ** Doesn't yet display docs back to user
** Pictures are stored in Azure blob storage