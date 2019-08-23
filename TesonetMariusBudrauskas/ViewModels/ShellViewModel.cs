using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using TesonetMariusBudrauskas.Views;

namespace TesonetMariusBudrauskas.ViewModels
{
    public class ShellViewModel : Screen
    {
        private string _Username;

        public string Username
        {
            get { return _Username; }
            set { _Username = value; }
        }

        private string _password;
        public string UserPassword
        {
            get { return _password; }
            set
            {
                _password = value;
                NotifyOfPropertyChange(() => UserPassword);
            }
        }

        public void Login()
        {
            try
            {
                var httpWebRequest = (HttpWebRequest)WebRequest.Create("http://playground.tesonet.lt/v1/tokens");
                httpWebRequest.ContentType = "application/json";
                httpWebRequest.Method = "POST";

                using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                {
                    string json = "{\"username\":\"" + Username + "\"," +
                                    "\"password\":\"" + UserPassword + "\"}";

                    streamWriter.Write(json);
                }

                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    var result = streamReader.ReadToEnd();

                    result = result.Split(':')[1].Remove(0, 1);
                    result = result.Remove(result.Length - 2);

                    var windowManager = new WindowManager();
                    ServersViewModel viewModel = Activator.CreateInstance(typeof(ServersViewModel), result) as ServersViewModel;

                    windowManager.ShowWindow(viewModel);
                }
            }
            catch (Exception)
            {
                //log exception
                return;
            }
        }
    }
}
