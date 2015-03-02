# Programming Challenges

## twitter-stream
Ruby script that:
* Connects to [Twitter's Streaming Sample API](https://dev.twitter.com/streaming/reference/get/statuses/sample) via PIN based OAuth
* Reads sample tweets over the course of a few minutes.
* Strips urls, user mentions, emoticons, stop words and other common words and produces unique word list.
* Presents total word count and top 10 word 'trends' with occurence count for each.

