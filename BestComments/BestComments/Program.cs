using System;
using System.Configuration;
using System.Linq;

namespace BestComments
{
    class Program
    {
        static void Main(string[] args)
        {
            var takeTop = Convert.ToInt32(ConfigurationManager.AppSettings["ScreenSpaceTopComments"]);
            var timeWeight = Convert.ToDouble(ConfigurationManager.AppSettings["TimeWeight"]);
            var childCountWeight = Convert.ToDouble(ConfigurationManager.AppSettings["ChildCountWeight"]);

            var ranker = new Ranker(new SampleData().Topic, takeTop, timeWeight, childCountWeight);
            var results = ranker.Rank().ToList();
            results.ForEach(r=> Console.WriteLine(r.ToString()));

            Console.WriteLine();
            Console.WriteLine("Press Enter to Exit");
            Console.ReadLine();
        }
    }
}
