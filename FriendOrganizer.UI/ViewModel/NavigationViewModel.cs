﻿using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using FriendOrganizer.Model;
using FriendOrganizer.UI.Data;
using FriendOrganizer.UI.Event;
using Prism.Events;

namespace FriendOrganizer.UI.ViewModel
{
    public class NavigationViewModel : ViewModelBase, INavigationViewModel
    {
        private readonly IFriendLookupDataService _friendLookupDataService;
        private readonly IEventAggregator _eventAggregator;
        private NavigationItemViewModel _selectedFriend;

        public NavigationViewModel(IFriendLookupDataService friendLookupDataService, IEventAggregator eventAggregator)
        {
            _friendLookupDataService = friendLookupDataService;
            _eventAggregator = eventAggregator;

            _eventAggregator.GetEvent<AfterFriendSaveEvent>().Subscribe(AfterFriendSaved);

            Friends = new ObservableCollection<NavigationItemViewModel>();
        }

        public async Task LoadAsync()
        {
            IEnumerable<LookupItem> lookups = await _friendLookupDataService.GetFriendLookupAsync();
            Friends.Clear();

            foreach (var item in lookups)
                Friends.Add(new NavigationItemViewModel(item.Id, item.DisplayMember));
        }

        public ObservableCollection<NavigationItemViewModel> Friends { get; set; }

        public NavigationItemViewModel SelectedFriend
        {
            get => _selectedFriend;
            set
            {
                _selectedFriend = value;
                OnPropertyChanged();
            }
        }

        protected override void OnPropertyChanged(string propertyName = null)
        {
            base.OnPropertyChanged(propertyName);

            if (_selectedFriend != null)
                _eventAggregator.GetEvent<OpenFriendDetailViewEvent>().Publish(_selectedFriend.Id);
        }

        private void AfterFriendSaved(AfterFriendSaveEventArgs friendArgs)
        {
            NavigationItemViewModel friend = Friends.Single(f => f.Id == friendArgs.Id);

            friend.DisplayMember = friendArgs.DisplayMember;
        }
    }
}