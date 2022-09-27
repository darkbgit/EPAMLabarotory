-- Venue
insert into dbo.Venue
	values 
	('Cinema holl', '84 Vasilevci street', '+375 29 678 90 12', 'UTC'),
	('Theatre holl', '46 Kolasa street', '+375 29 678 90 13', 'Morocco Standard Time')

-- Layout
insert into dbo.Layout
	values 
		(1, 'Big holl'),
		(1, 'Small holl'),
		(2, 'Holl')

-- Area
insert into dbo.Area
	values 
		(1, 'Sector A', 0, 0),
		(1, 'Sector B', 1, 1),
		(2, 'Sector A', 0, 0),
		(2, 'Sector B', 1, 1),
		(3, 'Sector A', 0, 0),
		(3, 'Sector B', 1, 1),
		(3, 'Launge zone', 2, 0)

-- Seat
insert into dbo.Seat
	values
		(1, 1, 1),
		(1, 1, 2),
		(1, 1, 3),
		(1, 2, 1),
		(1, 2, 2),
		(1, 2, 3),
		(2, 1, 1),
		(2, 1, 2),
		(2, 1, 3),
		(2, 2, 1),
		(2, 2, 2),
		(2, 2, 3),
		(3, 1, 1),
		(3, 1, 2),
		(3, 1, 3),
		(3, 2, 1),
		(3, 2, 2),
		(3, 2, 3),
		(4, 1, 1),
		(4, 1, 2),
		(4, 1, 3),
		(4, 2, 1),
		(4, 2, 2),
		(4, 2, 3),
		(5, 1, 1),
		(5, 1, 2),
		(6, 1, 1),
		(6, 1, 2),
		(6, 1, 3),
		(7, 1, 1)

-- Events
insert into dbo.Event
	values 
		('The Shawshank Redemption', 'Two imprisoned men bond over a number of years, finding solace and eventual redemption through acts of common decency.', 1, '2022-07-10 12:00:00', '2022-07-10 13:00:00', 'https://avatars.mds.yandex.net/get-kinopoisk-image/1946459/eae33fc1-bcb5-450e-89bf-9ba077b24cdf/orig'),
		('The green mile', 'The lives of guards on Death Row are affected by one of their charges: a black man accused of child murder and rape, yet who has a mysterious gift.', 1, '2022-07-10 14:00:00', '2022-07-10 16:00:00', 'https://avatars.mds.yandex.net/get-kinopoisk-image/1946459/acb932eb-c7d0-42de-92df-f5f306c4c48e/orig'),
		('Schindler''s List', 'In German-occupied Poland during World War II, industrialist Oskar Schindler gradually becomes concerned for his Jewish workforce after witnessing their persecution by the Nazis.', 2, '2022-07-10 12:30:00', '2022-07-10 15:00:00', 'https://avatars.mds.yandex.net/get-kinopoisk-image/1900788/ad682589-603d-40c1-b63a-fe01af9f3012/orig'),
		('Event2-2', 'Second Event on first layout', 2, '2022-07-10 18:00:00', '2022-07-10 19:00:00', null)

-- EventArea
insert into dbo.EventArea
	values 
		(1, 'Sector A', 0, 0, 10),
		(1, 'Sector B', 1, 1, 20),
		(2, 'Sector A', 0, 0, 15),
		(2, 'Sector B', 1, 1, 25),
		(3, 'Sector A', 0, 0, 15),
		(3, 'Sector B', 1, 1, 25),
		(4, 'Sector A', 0, 0, 15),
		(4, 'Sector B', 1, 1, 25)

-- EventSeat
insert into dbo.EventSeat
	values
		(1, 1, 1, 0),
		(1, 1, 2, 0),
		(1, 2, 1, 0),
		(1, 2, 2, 0),
		(2, 1, 1, 0),
		(2, 1, 2, 0),
		(2, 2, 1, 0),
		(2, 2, 2, 0),
		(3, 1, 1, 0),
		(3, 1, 2, 0),
		(3, 2, 1, 0),
		(3, 2, 2, 0),
		(4, 1, 1, 0),
		(4, 1, 2, 0),
		(4, 2, 1, 0),
		(4, 2, 2, 0),
		(5, 1, 1, 0),
		(5, 1, 2, 0),
		(5, 2, 1, 0),
		(5, 2, 2, 1),
		(5, 2, 3, 0),
		(6, 1, 1, 0),
		(6, 1, 2, 0),
		(6, 2, 1, 0),
		(6, 2, 2, 1),
		(7, 1, 1, 1),
		(7, 1, 2, 1),
		(7, 2, 1, 1),
		(7, 2, 2, 1),
		(8, 1, 1, 1),
		(8, 1, 2, 1),
		(8, 2, 1, 1),
		(8, 2, 2, 1)