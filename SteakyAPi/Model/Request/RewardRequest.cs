﻿namespace StreakyAPi.Model.Request
{
    public class RewardRequest
    {
        public int StreakId { get; set; }
        public string Description { get; set; }
        public int PointsClaimed { get; set; }
        public int BusinessId { get; set; }
    }
}