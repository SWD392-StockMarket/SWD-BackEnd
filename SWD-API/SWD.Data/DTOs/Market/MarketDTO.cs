﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWD.Data.DTOs.Market
{
    public class MarketDTO
    {
        public int MarketId { get; set; }
        public string MarketName { get; set; } = null!;
        public string? Address { get; set; }
        public DateTime? EstablishedDate { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Website { get; set; }
    }
}
