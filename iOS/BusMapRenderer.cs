﻿using System;
using System.Drawing;
using System.Collections.Generic;
using CoreGraphics;
using NUSBusMap;
using NUSBusMap.iOS;
using MapKit;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Xamarin.Forms.Maps.iOS;
using Xamarin.Forms.Platform.iOS;

[assembly:ExportRenderer (typeof(BusMap), typeof(BusMapRenderer))]
namespace NUSBusMap.iOS
{
	public class BusMapRenderer : MapRenderer
	{
		UIView customPinView;
		List<CustomPin> busPins;
		List<CustomPin> stopPins;

		protected override void OnElementChanged (ElementChangedEventArgs<View> e)
		{
			base.OnElementChanged (e);

			if (e.OldElement != null) {
				var nativeMap = Control as MKMapView;
				nativeMap.GetViewForAnnotation = null;
				nativeMap.DidSelectAnnotationView -= OnDidSelectAnnotationView;
				nativeMap.DidDeselectAnnotationView -= OnDidDeselectAnnotationView;
			}

			if (e.NewElement != null) {
				var formsMap = (BusMap)e.NewElement;
				var nativeMap = Control as MKMapView;
				busPins = formsMap.BusPins;
				stopPins = formsMap.StopPins;

				nativeMap.GetViewForAnnotation = GetViewForAnnotation;
				nativeMap.DidSelectAnnotationView += OnDidSelectAnnotationView;
				nativeMap.DidDeselectAnnotationView += OnDidDeselectAnnotationView;
			}
		}

		MKAnnotationView GetViewForAnnotation (MKMapView mapView, IMKAnnotation annotation)
		{
			MKAnnotationView annotationView = null;

			if (annotation is MKUserLocation)
				return null;
			
			var anno = annotation as MKPointAnnotation;
			var customPin = GetCustomPin (anno);
			if (customPin == null) {
				throw new Exception ("Custom pin not found");
			}

			// add custom image for map pin
			annotationView = mapView.DequeueReusableAnnotation (customPin.Id);
			if (annotationView == null) {
				annotationView = new MKAnnotationView (annotation, customPin.Id);
				annotationView.Image = UIImage.FromFile (customPin.Url).Scale(new SizeF() { Height=20, Width=20 });
			}
			// hide default callout
			annotationView.CanShowCallout = false;
			return annotationView;
		}

		void OnDidSelectAnnotationView (object sender, MKAnnotationViewEventArgs e)
		{
			// centralise map and freeze map updates
			MapPage.CentraliseMap (new Position(e.View.Annotation.Coordinate.Latitude, 
									e.View.Annotation.Coordinate.Longitude));
			MapPage.SetFreezeMap (true);

			// create custom callout with bus info
			// set background for callout
			var frame = new CGRect (0, 0, 200, 84);
			customPinView = new UIView { 
				Frame = frame,
				BackgroundColor = new UIColor(0.8f,0.8f,0.8f,0.3f),
				Center = new CGPoint (0, -(e.View.Frame.Height + 20))
			};
			customPinView.Layer.BorderColor = new CGColor(0,0,0,50);
			customPinView.Layer.BorderWidth = 1f;
			customPinView.Layer.CornerRadius = 2f;

			// add text info
			customPinView.Add(new UILabel { 
				Frame = frame,
				// title - label, subtitle - address (in xamarin.forms.maps)
				Text = e.View.Annotation.GetTitle() + "\n" + e.View.Annotation.GetSubtitle(), 
				Font = UIFont.FromName("Helvetica", 12f),
				TextAlignment = UITextAlignment.Left,
				AdjustsFontSizeToFitWidth = true,
				LineBreakMode = UILineBreakMode.WordWrap,
				Lines = 0
			});
			e.View.AddSubview (customPinView);
		}

		void OnDidDeselectAnnotationView (object sender, MKAnnotationViewEventArgs e)
		{
			if (!e.View.Selected) {
				customPinView.RemoveFromSuperview ();
				customPinView.Dispose ();
				customPinView = null;
				MapPage.SetFreezeMap (false);
			}
		}

		CustomPin GetCustomPin (MKPointAnnotation annotation)
		{
			var position = new Position (annotation.Coordinate.Latitude, annotation.Coordinate.Longitude);
			foreach (var pin in busPins) {
				if (pin.Pin.Position == position) {
					return pin;
				}
			}
			foreach (var pin in stopPins) {
				if (pin.Pin.Position == position) {
					return pin;
				}
			}
			return null;
		}
	}
}
