using System;

using Xamarin.Forms;

namespace NUSBusMap
{
	public class SvcInfoPage : ContentPage
	{
        public SvcInfoPage(string busSvcName)
        {
            Label header = new Label
            {
                Text = busSvcName + "Route Information",
                FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label)),
                HorizontalOptions = LayoutOptions.Center
            };

            var view = new TableView() {
                Intent = TableIntent.Settings
            };
            var root = new TableRoot();
            var section = new TableSection();

            foreach (string busStopCode in BusHelper.BusSvcs[busSvcName].stops)
            {
                section.Add(new BusStopCell(busStopCode));
            }

            root.Add(section);
            view.Root = root;

            Content = new StackLayout
            {
                Children =
                {
                    header,
                    view
                }
            };
        }
	}
}

