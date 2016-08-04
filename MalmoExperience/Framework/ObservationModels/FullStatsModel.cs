﻿namespace RunMission.Framework.ObservationModels {
    public class FullStatsModel {
        public int DistanceTravelled { get; set; }
        public int TimeAlive { get; set; }
        public int MobsKilled { get; set; }
        public int DamageTaken { get; set; }
        public decimal Life { get; set; }
        public int Score { get; set; }
        public int Food { get; set; }
        public int XP { get; set; }
        public bool IsAlive { get; set; }
        public int Air { get; set; }
        public double XPos { get; set; }
        public double YPos { get; set; }
        public double ZPos { get; set; }
        public double Pitch { get; set; }
        public double Yaw { get; set; }
    }
}