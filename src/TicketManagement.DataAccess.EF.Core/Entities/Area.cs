﻿namespace TicketManagement.DataAccess.EF.Core.Entities
{
    /// <summary>
    /// Area entity.
    /// </summary>
    public class Area : IBaseEntity
    {
        public int Id { get; set; }
        public int LayoutId { get; set; }
        public string Description { get; set; }
        public int CoordX { get; set; }
        public int CoordY { get; set; }
    }
}