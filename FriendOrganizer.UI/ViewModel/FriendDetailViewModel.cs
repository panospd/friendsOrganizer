using System.Threading.Tasks;
using System.Windows.Input;
using FriendOrganizer.Model;
using FriendOrganizer.UI.Data;
using FriendOrganizer.UI.Event;
using FriendOrganizer.UI.Wrapper;
using Prism.Commands;
using Prism.Events;

namespace FriendOrganizer.UI.ViewModel
{
    public class FriendDetailViewModel : ViewModelBase, IFriendDetailViewModel
    {
        private readonly IFriendDataService _friendDataService;
        private readonly IEventAggregator _eventAggregator;
        private FriendWrapper _friend;

        public FriendDetailViewModel(IFriendDataService friendDataService, IEventAggregator eventAggregator)
        {
            _friendDataService = friendDataService;
            _eventAggregator = eventAggregator;

            SaveCommand = new DelegateCommand(OnSaveExecute, OnsaveCanExecute);

            _eventAggregator.GetEvent<OpenFriendDetailViewEvent>().Subscribe(OnOpenFriendDetailView);
        }

        public async Task LoadAsync(int friendId)
        {
            Friend friend = await _friendDataService.GetByIdAsync(friendId);
            Friend = new FriendWrapper(friend);

            Friend.PropertyChanged += (s, e) =>
            {
                if (e.PropertyName == nameof(Friend.HasErrors))
                    ((DelegateCommand) SaveCommand).RaiseCanExecuteChanged();
            };

            ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();
        }

        public FriendWrapper Friend
        {
            get => _friend;
            set
            {
                _friend = value;
                OnPropertyChanged();
            }
        }

        public ICommand SaveCommand { get; }

        private async void OnOpenFriendDetailView(int friendId)
        {
            await LoadAsync(friendId);
        }

        private bool OnsaveCanExecute()
        {
            if (Friend == null)
                return false;

            return !Friend.HasErrors;
        }

        private async void OnSaveExecute()
        {
            await _friendDataService.SaveAsync(Friend.Model);

            _eventAggregator.GetEvent<AfterFriendSaveEvent>().Publish(new AfterFriendSaveEventArgs
            {
                Id = Friend.Id,
                DisplayMember = $"{Friend.FirstName} {Friend.LastName}"
            });
        }
    }
}