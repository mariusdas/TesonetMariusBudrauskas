using Caliburn.Micro;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using TesonetMariusBudrauskas.Models;

namespace TesonetMariusBudrauskas.ViewModels
{
    public class ServersViewModel : Screen
    {
        private string _token;

        public string Token
        {
            get { return _token; }
            set { _token = value; }
        }


        public ServersViewModel(string token)
        {
            Token = token;
            Servers = new ObservableCollection<Servers>();
            FillData();
        }

        private void FillData()
        {
            try
            {
                var request = System.Net.HttpWebRequest.Create("http://playground.tesonet.lt/v1/servers");
                request.Method = "GET";
                request.Headers.Add("Authorization", "Bearer " + Token);
                using (System.Net.WebResponse response = request.GetResponse())
                {
                    using (System.IO.StreamReader streamReader = new System.IO.StreamReader(response.GetResponseStream()))
                    {
                        dynamic jsonResponseText = streamReader.ReadToEnd();

                        var allServers = JsonConvert.DeserializeObject<List<Servers>>(jsonResponseText);

                        foreach (Servers server in allServers)
                        {
                            var cServer = server;
                            cServer.Distance = cServer.Distance + " km";
                            Servers.Add(cServer);
                        }
                    }
                }
            }
            catch(Exception)
            {
                //log exception.
                return;
            }

        }

        private ObservableCollection<Servers> _servers;

        public ObservableCollection<Servers> Servers
        {
            get { return _servers; }
            set
            {
                _servers = value;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}