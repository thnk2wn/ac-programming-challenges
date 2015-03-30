using System;

namespace BestComments
{
    class SampleData
    {
        public SampleData()
        {
            PopulateData();
        }

        private void PopulateData()
        {
            this.Topic = new Topic();
            Topic.Comments.Add(
                new Comment ("Comment", 100, DateTime.Now.AddHours(-8),
                    new Comment("Child Comment", 90, DateTime.Now.AddHours(-7.5),
                        new Comment("Child of Child Comment", 50, DateTime.Now.AddHours(-7.0))),
                    new Comment("Child Comment", 10, DateTime.Now.AddHours(-6)))
             );
            Topic.Comments.Add(
                new Comment("Comment", 80, DateTime.Now.AddHours(-6),
                    new Comment("Child Comment", 70, DateTime.Now.AddHours(-5.9),
                        new Comment("Child of Child Comment", 60, DateTime.Now.AddHours(-3.8)),
                        new Comment("Child of Child Comment", 10, DateTime.Now.AddHours(-3.8))
                        ),
                    new Comment("Child Comment", 10, DateTime.Now.AddHours(-5.7)))
                );
            Topic.Comments.Add(
                new Comment("Comment", 70, DateTime.Now.AddHours(-3),
                    new Comment("Child Comment", 50, DateTime.Now.AddHours(-2.5),
                        new Comment("Child of Child Comment", 30, DateTime.Now.AddHours(-2)))
                ));

            //TODO: loop on up to 1,000 generating better sample size for testing
        }

        public Topic Topic { get; private set; }
    }
}
