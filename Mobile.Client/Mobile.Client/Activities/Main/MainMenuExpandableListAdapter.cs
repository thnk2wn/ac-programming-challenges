using System.Linq;
using Android.Content;
using Android.Views;
using Android.Widget;
using System.Collections.Generic;

namespace AvantCredit.Uploader.Activities.Main
{
    /// <summary>
    /// Handles setting up the expandable main menu list
    /// </summary>
    /// <remarks>
    /// For descriptions of built-in layouts and Text1, Text2 etc. resource ids, see:
    /// http://arteksoftware.com/androids-built-in-list-item-layouts/
    /// </remarks>
    class MainMenuExpandableListAdapter : BaseExpandableListAdapter
    {
        private readonly Context _context;

        private readonly List<MainMenuItem> _mainMenuItems;

        public MainMenuExpandableListAdapter(Context context, IEnumerable<MainMenuItem> mainMenuItems)
        {
            _context = context;
            _mainMenuItems = mainMenuItems.ToList();
        }

        public override bool HasStableIds
        {
            // indexes are used for ids
            get { return true; }
        }

        public override long GetGroupId(int groupPosition)
        {
            // index of the group is used as its id
            return groupPosition;
        }

        public override int GroupCount
        {
            get { return _mainMenuItems.Count; }
        }

        private View CreateView(int resource)
        {
            var inflater = (LayoutInflater)_context.GetSystemService(Context.LayoutInflaterService);
            var view = inflater.Inflate(resource, null);
            return view;
        }

        public override View GetGroupView(int groupPosition, bool isExpanded, View convertView, ViewGroup parent)
        {
            // recycle a previous view if provided, otherwise inflate a new one
            var view = convertView ?? CreateView(Android.Resource.Layout.SimpleExpandableListItem1);
            var mainMenuItem = _mainMenuItems[groupPosition];

            // get the built-in first text view and set main menu / top level header text
            var textView = Heading1TextView(view);
            textView.Text = mainMenuItem.Text;

            return view;
        }

        public override Java.Lang.Object GetGroup(int groupPosition)
        {
            return null;
        }

        public override long GetChildId(int groupPosition, int childPosition)
        {
            // index of the child is used as its id
            return childPosition;
        }

        public override int GetChildrenCount(int groupPosition)
        {
            var mainMenuItem = _mainMenuItems[groupPosition];
            return mainMenuItem.ChildMenuItems.Length;
        }

        public override View GetChildView(int groupPosition, int childPosition, bool isLastChild, View convertView, ViewGroup parent)
        {
            // recycle a previous view if provided, otherwise inflate a new one
            var view = convertView ?? CreateView(Android.Resource.Layout.SimpleExpandableListItem2);

            var mainMenuItem = _mainMenuItems[groupPosition];
            var childMenuItem = mainMenuItem.ChildMenuItems[childPosition];

            // set child menu item text
            var textView = Heading1TextView(view);
            textView.Text = childMenuItem.Text;

            // and the child submenu text
            textView = view.FindViewById<TextView>(Android.Resource.Id.Text2);
            textView.Text = childMenuItem.Subtitle;

            view.Click += (sender, args) => ChildMenuItemClicked(childMenuItem);

            return view;
        }

        private static void ChildMenuItemClicked(ChildMenuItem item)
        {
            System.Diagnostics.Trace.WriteLine(string.Format("Clicked: {0}", item.Text));

            if (null != item.Command)
                item.Command.Execute();
        }

        private static TextView Heading1TextView(View view)
        {
            return view.FindViewById<TextView>(Android.Resource.Id.Text1);
        }

        public override Java.Lang.Object GetChild(int groupPosition, int childPosition)
        {
            return null;
        }

        public override bool IsChildSelectable(int groupPosition, int childPosition)
        {
            return true;
        }
    }
}
