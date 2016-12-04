using System;

using Xamarin.Forms;

namespace PermissionTest
{
	public class MyPage : ContentPage
	{
		public MyPage()
		{
			Title = "Plugin.Permissions Test";
			Button testBtn = new Button { Text = "Test Map Permissions" };
			testBtn.Clicked += (sender, e) => Navigation.PushAsync(new PermissionTestPage());

			Content = new StackLayout
			{
				Children = {
					testBtn
				}
			};
		}
	}
}

