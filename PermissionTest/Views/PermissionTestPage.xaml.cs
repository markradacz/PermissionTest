using System;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Plugin.Geolocator;

namespace PermissionTest
{
	public partial class PermissionTestPage : ContentPage
	{
		readonly string pageName = "PermissionTestPage";
		private ConvenientCaresViewModel viewModel;

		public PermissionTestPage()
		{
			InitializeComponent();
			AutomationId = pageName;
			Title = "Convenient Cares";
			NavigationPage.SetBackButtonTitle(this, "");

			BindingContext = viewModel = new ConvenientCaresViewModel(this);

			ConvenientCareListView.ItemSelected += (sender, e) =>
			{
				if (ConvenientCareListView.SelectedItem == null)
					return;

				ConvenientCare cCare = ConvenientCareListView.SelectedItem as ConvenientCare;
				if (cCare != null && !string.IsNullOrWhiteSpace(cCare.Address))
					MapHelper.NavigateToLocation(cCare.Address);

				ConvenientCareListView.SelectedItem = null;
			};

			AddressSearchBar.TextChanged += async (sender, e) =>
			{
				if (string.IsNullOrWhiteSpace(e.NewTextValue))
				{
					AddressSearchBar.Unfocus();

					if (await PermissionsHelper.LocationPermissionGranted(this))
					{
						await CalculateCurrentLocation();
					}
					viewModel.CalculateDistance();
				}
			};

			AddressSearchBar.SearchButtonPressed += async (sender, e) =>
			{
				SearchBar searchBar = (SearchBar)sender;
				if (searchBar == null)
					return;

				string address = searchBar.Text;
				if (string.IsNullOrWhiteSpace(address))
					return;

				await CalculateLocationByAddress(address);
			};
		}

		public async Task CalculateCurrentLocation()
		{
			try
			{
				if (await PermissionsHelper.LocationPermissionGranted(this))
				{
					CrossGeolocator.Current.DesiredAccuracy = 1000;

					var location = await CrossGeolocator.Current.GetPositionAsync(timeoutMilliseconds: 10000);
					viewModel.Latitude = location.Latitude;
					viewModel.Longitude = location.Longitude;
				}
			}
			catch (Exception ex)
			{
				await DisplayAlert("Error", ex.Message, LocaleConstants.ALERT_OK);

				//DisplayAlert(LocaleConstants.ISSUE_TITLE,
				//	LocaleConstants.UNABLE_TO_GET_CURRENT_LOCATION, LocaleConstants.ALERT_OK);
				//Xamarin.Insights.Report(ex);
			}
		}


		public async Task CalculateLocationByAddress(string address)
		{
			var geocoder = new Geocoder();
			var positions = await geocoder.GetPositionsForAddressAsync(address);

			if (positions != null && positions.Count() > 0)
			{
				var position = positions.FirstOrDefault();
				viewModel.Latitude = position.Latitude;
				viewModel.Longitude = position.Longitude;

				MoveToLocation(viewModel.Latitude, viewModel.Longitude, 5);
				viewModel.CalculateDistance();
			}
		}

		private void MoveToLocation(double Latitude, double Longitude, int fromMiles)
		{
			ConvenientCareMap.MoveToRegion(
							MapSpan.FromCenterAndRadius(
								new Position(Latitude, Longitude), Distance.FromMiles(fromMiles)));
		}

		protected override async void OnAppearing()
		{
			base.OnAppearing();
			try
			{
				if (await PermissionsHelper.LocationPermissionGranted(this))
				{
					ConvenientCareMap.IsShowingUser = true; 
					MoveToLocation(viewModel.Latitude, viewModel.Longitude, 10);
					await CalculateCurrentLocation();
					MoveToLocation(viewModel.Latitude, viewModel.Longitude, 5);
				}

				if (viewModel.ConvenientCares.Count > 0 || viewModel.IsBusy)
					return;

				viewModel.ConvenientCares.Clear();
				if (await viewModel.GetConvenientCaresAsync())
				{
					foreach (var urgentCare in viewModel.ConvenientCares)
					{
						var position = new Position(urgentCare.Latitude, urgentCare.Longitude);
						var pin = new Pin
						{
							Type = PinType.Place,
							Position = position,
							Label = urgentCare.Name,
							Address = urgentCare.Address
						};
						ConvenientCareMap.Pins.Add(pin);
					}
				}
			}
			catch (Exception ex)
			{
				await DisplayAlert("Error", ex.Message, "Ok");
				//Xamarin.Insights.Report(ex);
			}
		}

	}
}

