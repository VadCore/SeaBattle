USE SeaBattleDB
GO
INSERT INTO Boards(XAbsMax, YAbsMax, Turn, TurnPlayerId)
	VALUES
	(10, 10, 1, 1)

INSERT INTO CoordinateShips(BoardId, Quadrant, XAbs, YAbs, ShipId)
	VALUES
	(1, 1, 2, 2, NULL),
	(1, 1, 2, 3, NULL),
	(1, 1, 2, 4, NULL),
	(1, 1, 2, 5, NULL),
	(1, 2, 3, 2, NULL),
	(1, 2, 3, 3, NULL)

INSERT INTO Players(BoardId, Nick, ActiveUnitsCount)
	VALUES
	(1, 'VASYA', 2),
	(1, 'PETYA', 2)

INSERT INTO Sizes(Title, [Length], HealthMax, Speed, [Range], Reloading, DamageShot, HealShot)
	VALUES
	('SmallShip', 1, 1, 1, 3, 1, 1, 1),
	('MiddleShip', 3, 3, 3, 3, 2, 2, 2),
	('BigShip', 5, 5, 5, 6, 1, 3, 3),
	('HugeShip', 7, 7, 7, 4, 4, 4, 4)

INSERT INTO Ships(PlayerId, SizeId, CenterCoordinateShipId, Health, Rotation, NextTurn)
	VALUES
	(1, 1, 1, 1, 1, 1),
	(1, 2, 2, 3, 0, 2),
	(2, 1, 3, 1, 1, 1),
	(2, 2, 4, 3, 0, 2)

UPDATE CoordinateShips
	SET ShipId = 1
	WHERE Id IN (1, 2, 3)

UPDATE CoordinateShips
	SET ShipId = 2
	WHERE Id IN (4, 5, 6)

INSERT INTO BattleAbilities(ShipId, ReloadTurn)
	VALUES
	(1, 0),
	(2, 0)

INSERT INTO SupportAbilities(ShipId, ReloadTurn)
	VALUES
	(3, 0),
	(4, 0)