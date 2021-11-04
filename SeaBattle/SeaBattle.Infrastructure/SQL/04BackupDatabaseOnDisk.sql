USE SeaBattleDB;
GO
BACKUP DATABASE SeaBattleDB
TO DISK = 'SeaBattleDB_Korobov.bak'
   WITH FORMAT,
      MEDIANAME = 'SeaBattleDBKorobovBackups',
      NAME = 'Full Backup of SeaBattleDB';
GO