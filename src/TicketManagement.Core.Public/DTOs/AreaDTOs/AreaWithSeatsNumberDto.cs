﻿namespace TicketManagement.Core.Public.DTOs.AreaDTOs
{
    public class AreaWithSeatsNumberDto
    {
        public int Id { get; set; }
        public int LayoutId { get; set; }
        public string Description { get; set; }
        public int CoordX { get; set; }
        public int CoordY { get; set; }
        public int SeatsNumber { get; set; }
    }
}