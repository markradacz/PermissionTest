using System;
using Xamarin.Forms;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using System.Windows.Input;
using System.Text.RegularExpressions;
using Plugin.Connectivity;
using Newtonsoft.Json;
using System.Reflection;
using Plugin.Messaging;

namespace PermissionTest
{
	public class ConvenientCaresViewModel : BaseViewModel
	{
        public ObservableCollection<ConvenientCare> ConvenientCares { get; set; }
        public ObservableCollection<ConvenientCare> OrderedConvenientCares { get; set; }

		private Page page; //bad way to display alerts was actually in BaseViewModel 

        public ConvenientCaresViewModel(Page page) : base ()
		{
			this.page = page; 
			Title = "Convenient Cares";
            ConvenientCares = new ObservableCollection<ConvenientCare>();
            OrderedConvenientCares = new ObservableCollection<ConvenientCare>();
            onCallTappedCommand = new Command(OnCallTapped);
            Latitude = 36.114647;
            Longitude = -115.172813;
        }

		public async Task<bool> GetConvenientCaresAsync()
		{
			if (IsBusy)
				return false;

			IsBusy = true;
			IsNotBusy = false;

			try
			{
				if (!CrossConnectivity.Current.IsConnected)
				{
					await page.DisplayAlert(LocaleConstants.CONNECTION_OFFLINE_TITLE,
						LocaleConstants.CONNECTION_OFFLINE_MESSAGE,
						LocaleConstants.ALERT_OK);
					return false;	
				}

				ConvenientCares.Clear();
				OrderedConvenientCares.Clear();

				var items = await GetConvenientCaresDataAsync_debug(); //instead of service call

				foreach (var item in items)
				{
					item.DistanceFromLocation = MapHelper.CalculateDistance(item.Latitude, item.Longitude, Latitude, Longitude, 'M');
					ConvenientCares.Add(item);
				}

				var orderedCC = ConvenientCares.OrderBy(l => l.DistanceFromLocation); 

				foreach (var orderedItem in orderedCC)
				{
					OrderedConvenientCares.Add(orderedItem);
				}

				return true;
			}
			catch (Exception ex)
			{
				//await page.DisplayAlert(LocaleConstants.ISSUE_TITLE,
				//	string.Format(LocaleConstants.ISSUE_MESSAGE_FORMAT, "load convenient care locations"),
				//	LocaleConstants.ALERT_OK);

				await page.DisplayAlert("Error", ex.ToString(), "Ok");

				//catch and log to the insights
				//Xamarin.Insights.Report(ex);
			}
			finally
			{
				IsBusy = false;
				IsNotBusy = true;
			}

			return false;
		}

        private double latitude;
        public double Latitude
        {
            get { return latitude; }
            set { SetProperty(ref latitude, value); }
        }

        private double longitude;
        public double Longitude
        {
            get { return longitude; }
            set { SetProperty(ref longitude, value); }
        }

        ICommand onCallTappedCommand;
        public ICommand OnCallTappedCommand
        {
            get { return onCallTappedCommand; }
        }

        async void OnCallTapped(object sender)
        {
            ConvenientCare tappedLocation = sender as ConvenientCare;
            if (tappedLocation != null && !string.IsNullOrWhiteSpace(tappedLocation.PhoneNumber))
            {
				var phoneToCall = tappedLocation.PhoneNumber.SafeTrim();
                if (await page.DisplayAlert(tappedLocation.Name, 
                    String.Format(LocaleConstants.CALL_LOCATION_MESSAGE, 
                    Regex.Replace(phoneToCall, @"(\d{3})(\d{3})(\d{4})", "($1) $2-$3")),
                    LocaleConstants.ALERT_YES, LocaleConstants.ALERT_NO))
                {
                    var phoneCallTask = MessagingPlugin.PhoneDialer;
					if (phoneCallTask.CanMakePhoneCall)
						phoneCallTask.MakePhoneCall(phoneToCall);
                }
            }
        }

        public void CalculateDistance()
        {
            foreach (var item in ConvenientCares)
            {
                item.DistanceFromLocation = MapHelper.CalculateDistance(item.Latitude, item.Longitude, Latitude, Longitude, 'M');
            }

            OrderedConvenientCares.Clear();
            foreach (var orderedItem in ConvenientCares.OrderBy(l => l.DistanceFromLocation))
            {
                OrderedConvenientCares.Add(orderedItem);
            }
        }


		//debug
		public Task<IEnumerable<ConvenientCare>> GetConvenientCaresDataAsync_debug()
		{
			var respJsonOK = ResourceManager.GetMockData(typeof(ConvenientCaresViewModel).GetTypeInfo().Assembly, "GetConvenientcares.json");

			string respJson = respJsonOK;

			var task = new Task<IEnumerable<ConvenientCare>>(() =>
			{
				var response = JsonConvert.DeserializeObject<IEnumerable<ConvenientCare>>(respJson);
				return response;
			});
			task.Start();
			return task;
		}


    }
}

