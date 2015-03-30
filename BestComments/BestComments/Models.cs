using System;
using System.Collections.Generic;

namespace BestComments
{
    class Topic
    {
        public Topic() { Comments = new List<Comment>(); }

        public List<Comment> Comments { get; set; }
    }

    class Comment
    {
        private static int _counter;

        public Comment()
        {
            Children = new List<Comment>();
            // IRL DB generated ids. simple counter here, no threading so no locking need
            Id = ++_counter;
        }

        public Comment(string text, int points, DateTime postedDate, params Comment[] children)
            : this()
        {
            this.Text = text;
            this.Points = points;
            this.PostedDate = postedDate;

            children.ForEach(c =>
            {
                c.Parent = this;
                this.Children.Add(c);
            });
        }

        public int Points { get; set; }

        public List<Comment> Children { get; set; }

        public Comment Parent { get; set; }


        // ----- additional identification props -----
        // To Identify Comment Contents
        public string Text { get; set; }

        // Date comment was posted / added, set when created/saved. To give time relevancy in score
        public DateTime PostedDate { get; set; }

        // to give uniqueness to comment
        public int Id { get; set; }


        // ----- additional calculated props -----
        // assigned in ranking process
        public double Score { get; set; }

        // depth level in tree, assigned during ranking. can be used to retrive by depth or for printing tree
        // not strictly needed currently but helpful if debugging
        public int Depth { get; set; }

        // id of the root level parent comment in the comment thread this is a part of. null if root already
        public int? RootParentId { get; set; }

        public override string ToString()
        {
            var root = RootParentId.HasValue ? ", Root: " + RootParentId : string.Empty;
            return string.Format("Points: {0}, Age: {1:##0.#}, Score: {2:##0.000}, Id: {3}{4}",
                this.Points, (DateTime.Now - this.PostedDate).TotalHours, this.Score, this.Id, root);
        }
    }
}
