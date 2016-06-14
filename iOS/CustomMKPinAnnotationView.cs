﻿using MapKit;

namespace NUSBusMap.iOS
{
	public class CustomMKPinAnnotationView : MKPinAnnotationView
	{
		public string Id { get; set; }

		public string Url { get; set; }

		public CustomMKPinAnnotationView (IMKAnnotation annotation, string id)
			: base (annotation, id)
		{
		}
	}
}
