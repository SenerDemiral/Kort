using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataLibrary;
using DataLibrary.Models;

namespace Kort.MyStates
{
    public class MyAppState
    {
        private string _usrName = "";
        private int _usrID = 0;
        private int _aaID = 0;
        private string _usrSkl = "";
        private bool _usrRrOK = false;
        private string _aaAd = "";
        readonly IDataAccess _dataAccess;

        public int curUuPageIndex = 0;
        public int curUuID = 0;

        public event EventHandler StateChanged;
        private void StateHasChanged()
        {
            // This will update any subscribers
            // that the counter state has changed
            // so they can update themselves
            // and show the current counter value
            StateChanged?.Invoke(this, EventArgs.Empty);
        }
        public MyAppState(IDataAccess dataAccess)
        {
            _dataAccess = dataAccess;
        }

        public bool LoginUser(MyAppStateModel appState)
        {
            //var aaa = _dataAccess.GetPK("UST");

            //var (ok, ad) = DataLibrary.DbAccess.Login(appState.UsrID, appState.UsrPwd);
            var (ok, usrAd, aaID, usrSkl, usrRrOK, aaAd) = _dataAccess.Login(appState.UsrID, appState.UsrPwd);
            if (ok)
            {
                _usrID = appState.UsrID;
                _usrName = usrAd;
                _usrSkl = usrSkl;
                _usrRrOK = usrRrOK;
                _aaID = aaID;
                _aaAd = aaAd;
                StateHasChanged();
                return true;
            }
            else if (appState == null)
            {
                _usrID = 0;
                _usrName = "";
                _aaID = 0;
                _usrSkl = "";
                _usrRrOK = false;
                _aaAd = "";

                StateHasChanged();
                return false;
            }
            /*
            // Authorize user
            if (appState.UsrPwd == appState.UsrID.ToString())
            {
                _usrID = appState.UsrID;
                _usrName = $"{appState.UsrID}Authorized";

                return true;
            }*/
            return false;
        }
        public void LogoutUser()
        {
            // use usrID LOG vs
            _usrID = 0;
            _usrName = "";
            StateHasChanged();

        }
        public int getUsrID()
        {
            return _usrID;
        }
        public int getAaID()
        {
            return _aaID;
        }
        public string getAaAd()
        {
            return _aaAd;
        }
        public string getUsrSkl()
        {
            return _usrSkl;
        }
        public bool getUsrRrOK()
        {
            return _usrRrOK;
        }
        public string getUsrName()
        {
            return _usrName;
        }
        public string getUsrFull()
        {
            return $"{_usrName} #{_usrID} @{_aaAd}";
        }
    }
}
