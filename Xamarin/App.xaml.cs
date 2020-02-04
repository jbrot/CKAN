using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CKAN.Xamarin
{
    public partial class App : Application
    {
        public KSPManager Manager;

        public App (KSPManager mgr)
        {
            InitializeComponent();

            Manager = mgr ?? new KSPManager(new NullUser());

            MainPage = new View.MainPage();
        }

        protected override void OnStart ()
        {
            // Handle when your app starts
        }

        protected override void OnSleep ()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume ()
        {
            // Handle when your app resumes
        }
    }
}
