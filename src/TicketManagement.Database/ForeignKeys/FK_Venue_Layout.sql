﻿ALTER TABLE dbo.Layout
ADD CONSTRAINT FK_Venue_Layout FOREIGN KEY (VenueId)     
    REFERENCES dbo.Venue (Id)
    ON DELETE CASCADE
    ON UPDATE CASCADE