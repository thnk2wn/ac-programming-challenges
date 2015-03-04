
using AvantCredit.Uploader.Core.Command;

namespace AvantCredit.Uploader.Activities.Main
{
    class MainMenuItem
    {
        public string Text { get; set; }

        public ChildMenuItem[] ChildMenuItems { get; set; }
    }

    class ChildMenuItem
    {
        public string Text { get; set; }

        public string Subtitle { get; set; }

        public ICommand Command { get; set; }
    }
}