using System;
using System.Collections.Generic;
using System.Linq;

namespace BestComments
{
    class Ranker
    {
        private readonly Topic _topic;
        private readonly int _takeCount;
        private readonly double _timeWeight;
        private readonly double _childCountWeight;

        public Ranker(Topic topic, int takeCount, double timeWeight, double childCountWeight)
        {
            _topic = topic;
            _takeCount = takeCount;
            _timeWeight = timeWeight;
            _childCountWeight = childCountWeight;
        }

        public IEnumerable<Comment> Rank()
        {
            if (!_topic.Comments.Any()) return _topic.Comments;

            // walk tree and calculate scores, root parent id, depth
            var items = WalkCommentsAndCalculate().OrderByDescending(c => c.Score).ToList();

            // remove items scored in bottom half 
            var median = items.Median(i => i.Score);
            if (items.Count > _takeCount) items.RemoveAll(i => i.Score < median);
            var totalCount = items.Count;

            // group by root / top level comment thread
            var groups = items.GroupBy(c => c.RootParentId)
                .Select(g => new {Root = g.Key, TotalScore = g.Sum(x=> x.Score), List = g.OrderByDescending(c=> c.Score).ToList()})
                .OrderByDescending(g=> g.TotalScore).ToList();

            // determine how many max to take in each group (MG) so there's diversity in threads. take MG per, add to hash
            var maxPerGroup = (int) Math.Ceiling((double)(totalCount > _takeCount ? _takeCount : totalCount) / groups.Count);
            var dict = new Dictionary<int, Comment>();
            groups.SelectMany(g=> g.List.Take(maxPerGroup))
                .ForEach(c =>
                {
                    if (!dict.ContainsKey(c.Id) && dict.Count + 1 <= _takeCount) dict.Add(c.Id, c);
                });

            var ranked = dict.Values.OrderByDescending(c=> c.Score).ToList();
            return ranked;
        }

        // More suited for larger tree, 1k total mentioned would prob be okay in recursive fashion stack and time wise but
        private IEnumerable<Comment> WalkCommentsAndCalculate()
        {
            var stack = new Stack<Comment>();
            _topic.Comments.ForEach(c =>
            {
                c.Depth = 1;
                stack.Push(c);
            });
            
            while (stack.Any())
            {
                var comment = stack.Pop();
                comment.RootParentId = GetRootParentId(comment);
                comment.Score = CalculateScore(comment);
                // will return all comments at end or partial if caller filters with where lambda etc. for early iterate term
                yield return comment;

                foreach (var childComment in comment.Children)
                {
                    childComment.Depth = comment.Depth + 1;
                    stack.Push(childComment);
                }
            }
        }

        private static int? GetRootParentId(Comment c)
        {
            int? rootParentId = null;
            var cp = c.Parent;
            while (cp != null)
            {
                rootParentId = cp.Id;
                cp = cp.Parent;
            }
            return rootParentId;
        }

        private double CalculateScore(Comment c)
        {
            // problem doesn't mention anything about age of comments but IRL much older comments would
            // likely be downplayed, even if higher points than newer
            // increase weight value to give more importance to time elapsed if so specified (> 1)

            // also give a bump for having more children; here we only do immediate children for efficiency and 
            // more direct relevancy but we might consider walking down and using count of all grandchildren
            var adjustedPoints = c.Points + Math.Pow(c.Children.Count + 1, _childCountWeight);
            var hoursElapsed = (DateTime.Now - c.PostedDate).TotalHours;
            var score = adjustedPoints / Math.Pow(hoursElapsed + 2, _timeWeight);
            return score;
        }
    }
}
