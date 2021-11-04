USE SeaBattleDB

CREATE TABLE Roles(
	[Id] int IDENTITY (1, 1) NOT NULL PRIMARY KEY,
	[Name] nvarchar(40) NOT NULL,
	[NormalizedName] nvarchar(40) NOT NULL
)

CREATE TABLE Users(
	[Id] int IDENTITY (1, 1) NOT NULL PRIMARY KEY,
	[Email] nvarchar(40) NOT NULL,
	[Password] nvarchar(max) NOT NULL,
	[RoleId] int NOT NULL FOREIGN KEY REFERENCES [Roles](Id),
	[Name] nvarchar(40) NOT NULL,
	[NormalizedName] nvarchar(40) NOT NULL,
)

CREATE TABLE Boards(
	Id int IDENTITY (1,1) NOT NULL PRIMARY KEY,
	XAbsMax int NOT NULL,
	YAbsMax int NOT NULL,
	Turn int NOT NULL,
	TurnPlayerId int NOT NULL,
)

CREATE TABLE Players(
	Id int IDENTITY (1,1) NOT NULL PRIMARY KEY,
	BoardId int NOT NULL FOREIGN KEY REFERENCES [Boards](Id),
	ActiveUnitsCount int NOT NULL,
	UserId int NOT NULL FOREIGN KEY REFERENCES [Users](Id),
)

CREATE TABLE Sizes(
	Id int IDENTITY (1,1) NOT NULL PRIMARY KEY,
	Title nvarchar(30),
	[Length] int NOT NULL,
	HealthMax int NOT NULL,
	Speed int NOT NULL,
	[Range] int NOT NULL,
	Reloading int NOT NULL,
	DamageShot int NOT NULL,
	HealShot int NOT NULL,
)

CREATE TABLE Ships(
	Id int IDENTITY (1,1) NOT NULL PRIMARY KEY,
	PlayerId int NOT NULL FOREIGN KEY REFERENCES [Players](Id) ON DELETE CASCADE,
	SizeId int NOT NULL FOREIGN KEY REFERENCES [Sizes](Id),
	CenterCoordinateShipId int NOT NULL,
	Health int NOT NULL,
	Rotation int NOT NULL,
	NextTurn int NOT NULL
)

CREATE TABLE CoordinateShips(
	Id int IDENTITY (1,1) NOT NULL PRIMARY KEY,
	BoardId int NOT NULL FOREIGN KEY REFERENCES [Boards](Id) ON DELETE CASCADE,
	Quadrant int NOT NULL,
	XAbs int NOT NULL,
	YAbs int NOT NULL,
	ShipId int NULL FOREIGN KEY REFERENCES [Ships](Id) ON DELETE SET NULL,
)

ALTER TABLE Ships ADD CONSTRAINT FK_Ships_CenterCoordinateShipId
   FOREIGN KEY (CenterCoordinateShipId) REFERENCES CoordinateShips (Id);

CREATE TABLE BattleAbilities(
	Id int IDENTITY (1,1) NOT NULL PRIMARY KEY,
	ShipId int FOREIGN KEY REFERENCES [Ships](Id) ON DELETE CASCADE,
	ReloadTurn int NOT NULL,
)

CREATE TABLE SupportAbilities(
	Id int IDENTITY (1,1) NOT NULL PRIMARY KEY,
	ShipId int FOREIGN KEY REFERENCES [Ships](Id) ON DELETE CASCADE,
	ReloadTurn int NOT NULL,
)