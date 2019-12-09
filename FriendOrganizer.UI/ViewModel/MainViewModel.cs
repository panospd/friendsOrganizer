using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using FriendOrganizer.Model;
using FriendOrganizer.UI.Annotations;
using FriendOrganizer.UI.Data;

namespace FriendOrganizer.UI.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        private readonly IFriendDataService _friendDataService;
        private Friend _selectedFriend;

        public MainViewModel(IFriendDataService friendDataService)
        {
            _friendDataService = friendDataService;
            Friends = new ObservableCollection<Friend>();
        }

        public async Task LoadAsync()
        {
            IEnumerable<Friend> friends = await _friendDataService.GetAllAsync();

            Friends.Clear();
            foreach (var friend in friends)
                Friends.Add(friend);
        }

        public Friend SelectedFriend
        {
            get => _selectedFriend;
            set
            {
                _selectedFriend = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<Friend> Friends { get; set; }
    }
}
