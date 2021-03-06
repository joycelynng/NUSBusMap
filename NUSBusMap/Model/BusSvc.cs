﻿using System;
using System.Collections.Generic;

namespace NUSBusMap
{
	public class BusSvc
	{
		public string routeName { get; set; }
		public List<string> firstBusTime { get; set; } // [weekday, sat, sun/ph] in "hh:mm" format
		public List<string> lastBusTime { get; set; } // [weekday, sat, sun/ph] in "hh:mm" format
		public List<int> freq { get; set; } // [high, mid, low] in mins
		public string firstStop { get; set; }
		public string lastStop { get; set; }
		public List<string> stops { get; set; }
		public List<double> distanceBetweenCheckpoints { get; set; }
		public List<double> checkpoints { get; set; } // data of long,lat of points the route passes by, for bus simulation (testing) and route drawing (in future)
		public bool showOnMap { get; set; }
	}
}

