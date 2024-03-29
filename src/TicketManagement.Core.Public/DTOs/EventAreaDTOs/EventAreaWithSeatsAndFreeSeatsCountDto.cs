﻿namespace TicketManagement.Core.Public.DTOs.EventAreaDTOs
{
    public class EventAreaWithSeatsAndFreeSeatsCountDto
    {
        public int Id { get; set; }
        public int EventId { get; set; }
        public string Description { get; set; }
        public int CoordX { get; set; }
        public int CoordY { get; set; }
        public decimal Price { get; set; }
        public int TotalSeats { get; set; }
        public int FreeSeats { get; set; }
    }
}