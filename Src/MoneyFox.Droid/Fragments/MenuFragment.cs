﻿using System;
using System.Threading.Tasks;
using MoneyFox.Droid.Activities;
using MoneyFox.Shared.ViewModels;

namespace MoneyFox.Droid.Fragments
{
    [MvxFragment(typeof (MainViewModel), Resource.Id.navigation_frame)]
    [Register("moneyfox.droid.fragments.MenuFragment")]
    public class MenuFragment : MvxFragment<MenuViewModel>, NavigationView.IOnNavigationItemSelectedListener
    {
        private NavigationView navigationView;
        private IMenuItem previousMenuItem;

        public bool OnNavigationItemSelected(IMenuItem item)
        {
            item.SetCheckable(true);
            item.SetChecked(true);
            previousMenuItem?.SetChecked(false);
            previousMenuItem = item;

            Navigate(item.ItemId);

            return true;
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var ignore = base.OnCreateView(inflater, container, savedInstanceState);

            var view = this.BindingInflate(Resource.Layout.fragment_navigation, null);

            navigationView = view.FindViewById<NavigationView>(Resource.Id.navigation_view);
            navigationView.SetNavigationItemSelectedListener(this);
            navigationView.Menu.FindItem(Resource.Id.nav_accounts).SetChecked(true);

            return view;
        }

        private async Task Navigate(int itemId)
        {
            ((MainActivity) Activity).DrawerLayout.CloseDrawers();
            await Task.Delay(TimeSpan.FromMilliseconds(250));

            switch (itemId)
            {
                case Resource.Id.nav_accounts:
                    ViewModel.ShowViewModelByType(typeof (AccountListViewModel));
                    break;
                case Resource.Id.nav_statistics:
                    ViewModel.ShowViewModelByType(typeof (StatisticSelectorViewModel));
                    break;
                case Resource.Id.nav_categories:
                    ViewModel.ShowViewModelByType(typeof (CategoryListViewModel));
                    break;
                case Resource.Id.nav_backup:
                    ViewModel.ShowViewModelByType(typeof (BackupViewModel));
                    break;
                case Resource.Id.nav_settings:
                    ViewModel.ShowViewModelByType(typeof (SettingsViewModel));
                    break;
                case Resource.Id.nav_about:
                    ViewModel.ShowViewModelByType(typeof (AboutViewModel));
                    break;
            }
        }
    }
}