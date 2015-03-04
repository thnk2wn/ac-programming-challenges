using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Android.App;
using Android.OS;
using AvantCredit.Uploader.Cloud;
using AvantCredit.Uploader.Commands;
using AvantCredit.Uploader.Core.Activity;
using AvantCredit.Uploader.Core.DI;
using AvantCredit.Uploader.Core.Security;
using AvantCredit.Uploader.Core.Window;

namespace AvantCredit.Uploader.Activities.Main
{
    [Activity(Label = "AvantCredit Uploader", Icon = "@drawable/avant_icon_logo")]
    class MainActivity : ExpandableListActivity
    {
        private readonly IUserAuthService _userAuthService;
        private readonly IDocumentUploadService _documentUploadService;
        private readonly IMessageBoxService _messageBoxService;

        private User User { get; set; }

        public MainActivity()
        {
            _userAuthService = IoC.Get<IUserAuthService>();
            _documentUploadService = IoC.Get<IDocumentUploadService>();
            _messageBoxService = IoC.Get<IMessageBoxService>();
        }

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            User = this.Intent.GetAsJson<User>();

            this.MenuItems = BuildMenuItems().ToList();
            var adapter = new MainMenuExpandableListAdapter(this, MenuItems);
            SetListAdapter(adapter);

            this.ExpandableListView.ExpandGroup(1);

            GetDocuments();
        }

        private List<MainMenuItem> MenuItems { get; set; } 

        private void GetDocuments()
        {
            //var progressDialog = ProgressDialog.Show(this, "Checking your account", "One moment please...", true);
            Exception error = null;

            new Thread(new ThreadStart(delegate
            {
                IList<DocumentInfo> docs = null;
                try
                {
                    docs = _documentUploadService.GetUploadedImages(_userAuthService.CurrentUser.UserId);
                }
                catch (Exception threadEx)
                {
                    error = threadEx;
                    System.Diagnostics.Trace.WriteLine(error);
                }

                RunOnUiThread(() =>
                {
                    //progressDialog.Hide();
                    DocsMenuItem.Subtitle = "View and manage your documents";

                    if (null != error)
                    {
                        DocsMenuItem.Text = "Your Documents";
                        _messageBoxService.ShowAlert("Cannot retrieve document details at this time. Please try again in a few minutes.", this);
                    }
                    else
                    {
                        //TODO: consider listing each doc here if sufficient text name w/direct link to details or link to thumbnails page
                        DocsMenuItem.Text = string.Format("Your Documents ({0})", null != docs ? docs.Count : 0);
                    }

                    //TODO: less brute force method of refreshing the list
                    var adapter = new MainMenuExpandableListAdapter(this, MenuItems);
                    SetListAdapter(adapter);
                    this.ExpandableListView.ExpandGroup(1);
                });
            })).Start();
        }

        private ChildMenuItem DocsMenuItem { get; set; }

        public IEnumerable<MainMenuItem> BuildMenuItems()
        {
            var items = new List<MainMenuItem>();

            //TODO: consider listing each doc here if sufficient text name w/direct link to details or link to thumbnails page
            //TODO: consider enabled state ability here and/or diff formatting
            DocsMenuItem = new ChildMenuItem
                {
                    Text = "Your Documents...",
                    Subtitle = "Checking your account documents. One moment please..."
                };

            items.Add(new MainMenuItem
            {
                Text = string.Format("Welcome {0}", User.FirstName),
                ChildMenuItems = new[] 
                { 
                    new ChildMenuItem { Text= "Logout", Command = new LogoutCommand(_userAuthService, this)}
                }
            });

            items.Add(new MainMenuItem()
            {
                Text = "Documents",
                ChildMenuItems = new[] 
                { 
                    new ChildMenuItem
                        {
                            Text="Upload New Document", 
                            Subtitle = "Add a new document for verification",
                            Command = new UploadNewDocCommand(this)
                        },
                    
                    DocsMenuItem
                }
            });

            return items;
        }
    }
}

