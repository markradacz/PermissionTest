using System;
using System.Threading.Tasks;
using Xamarin.Forms;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;

namespace PermissionTest
{
	public static class PermissionsHelper
	{
		public static async Task<bool> LocationPermissionGranted(Page page)
		{
			bool result = false; 

			try 
			{ 
				var status = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Location); 
				if (status != PermissionStatus.Granted) 
				{ 
					if(await CrossPermissions.Current.ShouldShowRequestPermissionRationaleAsync(Permission.Location)) 
					{ 
						await page.DisplayAlert(
							LocaleConstants.LOCATION_PERMISSION_MISSING_TITLE, 
							LocaleConstants.LOCATION_PERMISSION_MISSING,
							LocaleConstants.ALERT_OK); 
					} 
					var results = await CrossPermissions.Current.RequestPermissionsAsync(Permission.Location);
					if (results != null && results.Count > 0)
					{
						status = results[Permission.Location];
					}
					else 
					{
						status = PermissionStatus.Unknown;
					}
				} 

				if(status == PermissionStatus.Granted)
				{
					result = true;
				}
				else if(status != PermissionStatus.Unknown) 
				{ 
					await page.DisplayAlert(
						LocaleConstants.LOCATION_PERMISSION_DENIED_TITLE,
						LocaleConstants.LOCATION_PERMISSION_DENIED,
						LocaleConstants.ALERT_OK);
				} 
			} 
			catch (Exception ex) 
			{ 
				await page.DisplayAlert("Error", ex.Message, LocaleConstants.ALERT_OK);

				//catch and log to the insights
				//Xamarin.Insights.Report(ex);
			}

			return result;
		}

		public static async Task<bool> CalendarPermissionGranted(Page page)
		{
			bool result = false; 

			try 
			{ 
				var status = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Calendar); 
				if (status != PermissionStatus.Granted) 
				{ 
					if(await CrossPermissions.Current.ShouldShowRequestPermissionRationaleAsync(Permission.Calendar)) 
					{ 
						await page.DisplayAlert(
							LocaleConstants.CALENDAR_PERMISSION_MISSING_TITLE, 
							LocaleConstants.CALENDAR_PERMISSION_MISSING,
							LocaleConstants.ALERT_OK); 
					} 
					var results = await CrossPermissions.Current.RequestPermissionsAsync(Permission.Calendar); 
					if (results != null && results.Count > 0)
					{
						status = results[Permission.Calendar];
					}
					else
					{
						status = PermissionStatus.Unknown;
					}
				} 

				if(status == PermissionStatus.Granted)
				{
					result = true;
				}
				else if(status != PermissionStatus.Unknown) 
				{ 
					await page.DisplayAlert(
						LocaleConstants.CALENDAR_PERMISSION_DENIED_TITLE,
						LocaleConstants.CALENDAR_PERMISSION_DENIED,
						LocaleConstants.ALERT_OK);
				} 
			} 
			catch (Exception ex) 
			{ 
				await page.DisplayAlert("Error", ex.Message, LocaleConstants.ALERT_OK);

				//catch and log to the insights
				//Xamarin.Insights.Report(ex);
			}

			return result;
		}


		public static async Task<bool> PhonePermissionGranted(Page page)
		{
			bool result = false; 

			try 
			{ 
				var status = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Phone); 
				if (status != PermissionStatus.Granted) 
				{ 
					if(await CrossPermissions.Current.ShouldShowRequestPermissionRationaleAsync(Permission.Phone)) 
					{ 
						await page.DisplayAlert(
							LocaleConstants.PHONE_PERMISSION_MISSING_TITLE, 
							LocaleConstants.PHONE_PERMISSION_MISSING,
							LocaleConstants.ALERT_OK); 
					} 
					var results = await CrossPermissions.Current.RequestPermissionsAsync(Permission.Phone); 
					if (results != null && results.Count > 0)
					{
						status = results[Permission.Phone];
					}
					else
					{
						status = PermissionStatus.Unknown;
					}
				} 

				if(status == PermissionStatus.Granted)
				{
					result = true;
				}
				else if(status != PermissionStatus.Unknown) 
				{ 
					await page.DisplayAlert(
						LocaleConstants.PHONE_PERMISSION_DENIED_TITLE,
						LocaleConstants.PHONE_PERMISSION_DENIED,
						LocaleConstants.ALERT_OK);
				} 
			} 
			catch (Exception ex) 
			{ 
				await page.DisplayAlert("Error", ex.Message, LocaleConstants.ALERT_OK);
				
				//catch and log to the insights
				//Xamarin.Insights.Report(ex);
			}

			return result;
		}

	}
}

