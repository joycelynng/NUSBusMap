using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace NUSBusMap
{
    public class BusStopCell : ViewCell
    {
        public BusStopCell(string busStopCode)
        {
            var stack = new StackLayout
            {
                Orientation = StackOrientation.Horizontal,
                VerticalOptions = LayoutOptions.Center,
                Padding = new Thickness(10)
            };

            Label busStopName = new Label
            {
                Text = BusHelper.BusStops[busStopCode].name,
                TextColor = Color.Black,
                HorizontalOptions = LayoutOptions.StartAndExpand,
                VerticalOptions = LayoutOptions.Center
            };

            Label roadName = new Label
            {
                Text = BusHelper.BusStops[busStopCode].road,
                TextColor = Color.Gray,
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center
            };

            stack.Children.Add(busStopName);
            stack.Children.Add(roadName);
            View = stack;
        }
    }
}
