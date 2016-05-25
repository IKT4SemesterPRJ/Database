using System.Linq;
using System.Security;
using System.Timers;
using System.Windows.Input;
using Administration;
using Pristjek220Data;
using SharedFunctionalities;

namespace Administration_GUI.User_Controls_Admin
{
    /// <summary>
    ///     AdminNewStoreModel is the User Control model for the AdminNewStore User Control
    ///     Its used to create a new store
    /// </summary>
    public class AdminNewStoreModel : ObservableObject, IPageViewModel
    {
        private readonly Timer _timer = new Timer(2500);
        private ICommand _enterPressedCommand;
        private string _error = string.Empty;
        private ICommand _illegalSignNewProductCommand;
        private readonly IAdmin _admin;

        private bool _isTextConfirm;


        private ICommand _newStoreCommand;

        private string _newStoreName;
        private string _oldtext;

        /// <summary>
        ///     AdminNewStoreModel constructor takes a UnitOfWork to create an Admin
        /// </summary>
        public AdminNewStoreModel(IAdmin admin)
        {
            _admin = admin;
        }

        /// <summary>
        ///     Get and Set method for NewStoreName. The set method, sets the old NewStoreName to an oldtext, and then
        ///     change the value to the new vaule and call OnPropertyChanged
        /// </summary>
        public string NewStoreName
        {
            get { return _newStoreName; }
            set
            {
                _oldtext = _newStoreName;
                _newStoreName = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        ///     Set and Get method for the Password
        /// </summary>
        public SecureString SecurePassword { private get; set; }

        /// <summary>
        ///     Set and Get method for the confirm Password
        /// </summary>
        public SecureString SecurePasswordConfirm { private get; set; }

        /// <summary>
        ///     The Error string that is written to a label on the GUI describes if the event has been successfully or if it has
        ///     failed, and why it failed
        /// </summary>
        public string Error
        {
            get { return _error; }
            set
            {
                _error = value;
                OnPropertyChanged();
                _timer.Stop();
                _timer.Start();
                _timer.Elapsed += delegate
                {
                    _error = "";
                    OnPropertyChanged();
                };
            }
        }

        /// <summary>
        ///     Is a bool that is used to set the color of a label to red if it's a fail and green if it's expected behaviour
        /// </summary>
        public bool IsTextConfirm
        {
            get { return _isTextConfirm; }
            set
            {
                _isTextConfirm = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        ///     Command that is used to create a new store, if anything goes wrong it will print the reason to why it
        ///     did not create a new store to a label
        /// </summary>
        public ICommand NewStoreCommand => _newStoreCommand ??
                                           (_newStoreCommand = new RelayCommand(NewStore));

        /// <summary>
        ///     Command that is used to see if Enter is pressed, if its pressed it calls the DeleteFromStoreDatabase
        /// </summary>
        public ICommand EnterKeyPressedCommand
            => _enterPressedCommand ?? (_enterPressedCommand = new GalaSoft.MvvmLight.Command.RelayCommand<KeyEventArgs>(EnterKeyPressed));

        /// <summary>
        ///     Command that is used whenever there is an TextChanged event to see if the text entered contains illegal signs
        /// </summary>
        public ICommand IllegalSignNewStoreCommand
            =>
                _illegalSignNewProductCommand ??
                (_illegalSignNewProductCommand = new RelayCommand(IllegalSignDeleteStore));

        private void IllegalSignDeleteStore()
        {
            if (NewStoreName == null) return;
            if (NewStoreName.All(chr => char.IsLetter(chr) || char.IsNumber(chr) || char.IsWhiteSpace(chr))) return;
            IsTextConfirm = false;
            Error = $"Der kan kun skrives bogstaverne fra a til å og tallene fra 0 til 9.";
            NewStoreName = _oldtext;
        }

        private void NewStore()
        {
            if (string.IsNullOrEmpty(NewStoreName))
            {
                IsTextConfirm = false;
                Error = "Udfyld venligst alle felter.";
                return;
            }
            if (_admin.CheckPasswords(SecurePassword, SecurePasswordConfirm) != 0)
            {
                switch (_admin.CreateLogin(NewStoreName, SecurePassword, NewStoreName))
                {
                    case -1:
                        IsTextConfirm = false;
                        Error = "Forretningen findes allerede.";
                        break;
                    case -2:
                        IsTextConfirm = false;
                        Error = "Udfyld venligst alle felter.";
                        break;
                    default:
                        IsTextConfirm = true;
                        Error = $"Forretning oprettet med forretningsnavnet \"{NewStoreName}\".";
                        break;
                }
            }
            else
            {
                IsTextConfirm = false;
                Error = "Kodeordene matcher ikke.";
            }
        }

        private void EnterKeyPressed(KeyEventArgs e)
        {
            if ((e.Key == Key.Enter) || (e.Key == Key.Return))
            {
                NewStore();
            }
        }
    }
}