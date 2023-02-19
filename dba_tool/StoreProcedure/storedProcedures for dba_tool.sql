use master
go
------------------------------------------------------------------------------------------------
--- 1. Gets Total Count of Tables in Selected Database ---
------------------------------------------------------------------------------------------------
Create or ALter proc udp_getTableCount
@dbname varchar(30)
as 
begin
exec('use ' + @dbname + '; select count(*) from INFORMATION_SCHEMA.tables')
end
GO
/*-------------------------------------------------------------------------------------------------*/


------------------------------------------------------------------------------------------------
--- 2. Gets Total Count of Views in Selected Database ---
------------------------------------------------------------------------------------------------
Create or ALter proc udp_getViewsCount
@dbname varchar(30)
as 
begin
exec('use ' + @dbname + '; select count(*) from INFORMATION_SCHEMA.VIEWS')
end
Go
/*-------------------------------------------------------------------------------------------------*/


------------------------------------------------------------------------------------------------
--- 3. Gets Total Count of Indexes in Selected Database ---
------------------------------------------------------------------------------------------------
Create or ALter proc udp_getIndexesCount
@dbname varchar(30)
as 
begin
exec('use ' + @dbname + '; select count(*) from sys.indexes where name like ''PK%'' or name like ''pk%''')
end
GO
/*-------------------------------------------------------------------------------------------------*/


------------------------------------------------------------------------------------------------
--- 4. Gets Index Physical Statistics in Selected Database ---
------------------------------------------------------------------------------------------------
Create or ALter proc udp_getIndexPhysicalStat
@dbname varchar(20)
as
begin
exec('use ' + @dbname + '; SELECT	TOP 50
		object_schema_name(ips.object_id)	AS [Schema_Name],
		object_name (ips.object_id)		AS [Object_Name],
		i.name					AS [Index_Name],
		ips.avg_fragmentation_in_percent	AS [Avg_Fragmentation_Percent]
FROM	sys.dm_db_index_physical_stats(db_id(), null, null, null, null) ips
INNER JOIN sys.indexes i ON i.object_id = ips.object_id 
		   AND i.index_id = ips.index_id
WHERE	ips.index_id > 0
ORDER BY avg_fragmentation_in_percent DESC')
end
GO
/*-------------------------------------------------------------------------------------------------*/


------------------------------------------------------------------------------------------------
--- 5. Gets Disk Usage By Tables in Selected Database ---
------------------------------------------------------------------------------------------------
Create or ALter  proc udp_getTop20TableByDiskUsage
@dbname varchar(30)
as
begin
exec('use '+@dbname+'; SELECT
        a3.name AS SchemaName,
        a2.name AS TableName,
        a1.rows as Records,
        (a1.reserved )* 8.0 / 1024 AS reserved_mb,
        a1.data * 8.0 / 1024 AS data_mb,
        (CASE WHEN (a1.used ) > a1.data THEN (a1.used ) - a1.data ELSE 0 END) * 8.0 / 1024 AS index_size_mb,
        (CASE WHEN (a1.reserved ) > a1.used THEN (a1.reserved ) - a1.used ELSE 0 END) * 8.0 / 1024 AS unused_mb

    FROM    (   SELECT
                ps.object_id,
                SUM ( CASE WHEN (ps.index_id < 2) THEN row_count    ELSE 0 END ) AS [rows],
                SUM (ps.reserved_page_count) AS reserved,
                SUM (CASE   WHEN (ps.index_id < 2) THEN (ps.in_row_data_page_count + ps.lob_used_page_count + ps.row_overflow_used_page_count)
                            ELSE (ps.lob_used_page_count + ps.row_overflow_used_page_count) END
                    ) AS data,
                SUM (ps.used_page_count) AS used
                FROM sys.dm_db_partition_stats ps
                GROUP BY ps.object_id
            ) AS a1

    INNER JOIN sys.all_objects a2  ON ( a1.object_id = a2.object_id )

    INNER JOIN sys.schemas a3 ON (a2.schema_id = a3.schema_id)

    WHERE a2.type <> N''S'' and a2.type <> N''IT''   
    order by data_mb desc') 
	end
	GO
/*-------------------------------------------------------------------------------------------------*/


------------------------------------------------------------------------------------------------
--- 6. Gets Login Error Log in Server ---
------------------------------------------------------------------------------------------------
Create or ALter proc udp_getLoginErrorLog
as
begin
DECLARE @A VARCHAR(10), @B VARCHAR(10);
SELECT @A = CONVERT(VARCHAR(20),GETDATE()-1,112);
SELECT @B = CONVERT(VARCHAR(20),GETDATE()+1,112);
EXEC XP_READERRORLOG 0, 1,N'Login', N'Failed', @A,@B,'DESC';
end
GO
/*-------------------------------------------------------------------------------------------------*/


------------------------------------------------------------------------------------------------
--- 7. Gets database Files Information Of Selected Database ---
------------------------------------------------------------------------------------------------
Create or ALter proc udp_getDatabaseFiles
@dbname varchar(30)
as
begin
exec('use '+@dbname+'; select name, physical_name, type_desc, state_desc, size, max_size, growth from sys.database_files')
end
GO
/*-------------------------------------------------------------------------------------------------*/


------------------------------------------------------------------------------------------------
--- 8. Gets Database Files Location of Selected Database ---
------------------------------------------------------------------------------------------------
Create or ALter proc udp_FetchDBFileLocations
@dbname varchar(30)
as
begin
exec('use '+ @dbname + '; select name, physical_name from sys.database_files')
					end
					GO
/*-------------------------------------------------------------------------------------------------*/


------------------------------------------------------------------------------------------------
--- 9. Gets Log Usage Information of Selected Database ---
------------------------------------------------------------------------------------------------
Create or ALter proc udp_FetchLogUsage
@dbname varchar(30)
as
begin
exec('use '+ @dbname + '; select database_id, total_log_size_in_bytes, used_log_space_in_bytes, used_log_space_in_percent from sys.dm_db_log_space_usage')
					end
					GO
/*-------------------------------------------------------------------------------------------------*/


------------------------------------------------------------------------------------------------
--- 10. Gets CPU Usage Information of Server ---
------------------------------------------------------------------------------------------------
Create or ALter proc udp_getCpuUsage
as
begin

Create table #CPU(                           
EventTime2 datetime,                            
SQLProcessUtilization varchar(50),                           
SystemIdle varchar(50),  
OtherProcessUtilization varchar(50),  
load_date datetime                            
)      
DECLARE @ts BIGINT;  DECLARE @lastNmin TINYINT;  
SET @lastNmin = 240;  
SELECT @ts =(SELECT cpu_ticks/(cpu_ticks/ms_ticks) FROM sys.dm_os_sys_info);   
insert into #CPU  
SELECT TOP 10 * FROM (  
SELECT TOP(@lastNmin)  
  DATEADD(ms,-1 *(@ts - [timestamp]),GETDATE())AS [Event_Time],   
  SQLProcessUtilization AS [SQLServer_CPU_Utilization],   
  SystemIdle AS [System_Idle_Process],   
  100 - SystemIdle - SQLProcessUtilization AS [Other_Process_CPU_Utilization],  
  GETDATE() AS 'LoadDate'  
FROM (SELECT record.value('(./Record/@id)[1]','int')AS record_id,   
record.value('(./Record/SchedulerMonitorEvent/SystemHealth/SystemIdle)[1]','int')AS [SystemIdle],   
record.value('(./Record/SchedulerMonitorEvent/SystemHealth/ProcessUtilization)[1]','int')AS [SQLProcessUtilization],   
[timestamp]        
FROM (SELECT[timestamp], convert(xml, record) AS [record]               
FROM sys.dm_os_ring_buffers               
WHERE ring_buffer_type =N'RING_BUFFER_SCHEDULER_MONITOR'AND record LIKE'%%')AS x )AS y   
ORDER BY SystemIdle ASC) d
select * from #CPU
drop table #CPU
end
GO
/*-------------------------------------------------------------------------------------------------*/


------------------------------------------------------------------------------------------------
--- 11. Gets TempDB Usage Information of Server ---
------------------------------------------------------------------------------------------------
Create or ALter proc udp_getTempdbUsage
as
begin 
exec('USE Tempdb;  
SELECT  CONVERT(VARCHAR(100), @@SERVERNAME) AS [server_name]  
                ,db.name AS [database_name]  
                ,mf.[name] AS [file_logical_name]  
                ,mf.[filename] AS[file_physical_name]  
                ,convert(FLOAT, mf.[size]/128) AS [file_size_mb]               
                ,convert(FLOAT, (mf.[size]/128 - (CAST(FILEPROPERTY(mf.[name], ''SpaceUsed'') AS int)/128))) as [available_space_mb]  
                ,convert(DECIMAL(38,2), (CAST(FILEPROPERTY(mf.[name], ''SpaceUsed'') AS int)/128.0)/(mf.[size]/128.0))*100 as [percent_full]      
FROM   tempdb.dbo.sysfiles mf  
JOIN      master..sysdatabases db  
ON         db.dbid = db_id()')
end
GO
/*-------------------------------------------------------------------------------------------------*/


------------------------------------------------------------------------------------------------
--- 12. Takes Snapshot of Selected Database ---
------------------------------------------------------------------------------------------------
Create or ALter proc [dbo].[udp_createSnapshot]
@dbname varchar(30),
@snapshot varchar(50)
as
begin
exec('use '+@dbname+'; create database '+@snapshot+' 
on 
(name = '+@dbname+', filename = ''F:\Snapshots\'+@snapshot+'.ss'') 
as snapshot of '+@dbname+'')
end
GO
/*-------------------------------------------------------------------------------------------------*/


------------------------------------------------------------------------------------------------
--- 13. Gives backup details of all database ---
------------------------------------------------------------------------------------------------
Create or ALter proc [dbo].[udp_getBackupsDetails]
as
begin
WITH CTE_Backup AS
(
SELECT  database_name,backup_start_date,type,physical_device_name
       ,Row_Number() OVER(PARTITION BY database_name,BS.type
        ORDER BY backup_start_date DESC) AS RowNum
FROM    msdb..backupset BS
JOIN    msdb.dbo.backupmediafamily BMF
ON      BS.media_set_id=BMF.media_set_id
)
SELECT      D.name
           ,ISNULL(CONVERT(VARCHAR,backup_start_date),'No backups') AS last_backup_time
           ,D.recovery_model_desc
           ,state_desc,
            CASE WHEN type ='D' THEN 'Full database'
            WHEN type ='I' THEN 'Differential database'
            WHEN type ='L' THEN 'Log'
            WHEN type ='F' THEN 'File or filegroup'
            WHEN type ='G' THEN 'Differential file'
            WHEN type ='P' THEN 'Partial'
            WHEN type ='Q' THEN 'Differential partial'
            ELSE 'Unknown' END AS backup_type
           ,physical_device_name
FROM        sys.databases D
LEFT JOIN   CTE_Backup CTE
ON          D.name = CTE.database_name
AND         RowNum = 1
ORDER BY    D.name,type
end
GO
/*-------------------------------------------------------------------------------------------------*/


------------------------------------------------------------------------------------------------
--- 14. Gives Data Files Details ---
------------------------------------------------------------------------------------------------
Create or ALter procedure [dbo].[udp_getDataFileUsage]
@name varchar(100)
as
begin
exec('use '+@name+'
select DbName, FileName, type_desc, CurrentSizeMB, (CurrentSizeMB - FreeSpaceMB) as UsedSizeMB, FreeSpaceMB from (SELECT DB_NAME() AS DbName, 
    name AS FileName, 
    type_desc,
    size/128.0 AS CurrentSizeMB,  
    size/128.0 - CAST(FILEPROPERTY(name, ''SpaceUsed'') AS INT)/128.0 AS FreeSpaceMB
FROM sys.database_files
WHERE type IN (0,1) and type_desc = ''ROWS'') as datafileDetails')
		end
GO
/*-------------------------------------------------------------------------------------------------*/


------------------------------------------------------------------------------------------------
--- 15. Takes Database Full Backup ---
------------------------------------------------------------------------------------------------
Create or ALter proc [dbo].[udp_getFullBackup]
@dbname varchar(50),
@backupname varchar(50)
as
begin
exec('backup database '+@dbname+'
to disk =''E:\Backups\fullBak\'+@backupname+'.bak''')
end
GO
/*-------------------------------------------------------------------------------------------------*/


------------------------------------------------------------------------------------------------
--- 16. Gives Snapshot Details of all databases ---
------------------------------------------------------------------------------------------------
Create or ALter proc [dbo].[udp_getSnapshotsDetails]
as
begin
select s2.name as snapshot_name, s1.name as source_database, s1.create_date, s1.state_desc, s1.snapshot_isolation_state_desc,
s1.recovery_model_desc from sys.databases s1, sys.databases s2
where s1.database_id = s2.source_database_id
end
GO
/*-------------------------------------------------------------------------------------------------*/


------------------------------------------------------------------------------------------------
--- 17. Rebuilds the index ---
------------------------------------------------------------------------------------------------
Create or ALter proc [dbo].[udp_rebuildIndex]
@schemaname varchar(100),
@dbname varchar(100),
@tablename varchar(100),
@indexname varchar(100)
as
begin
exec('use '+@dbname+'; Alter Index '+@indexname+' on '+@schemaname+'.'+@tablename+' Rebuild;')
end

GO
/*-------------------------------------------------------------------------------------------------*/


------------------------------------------------------------------------------------------------
--- 18. Reograinzes the index ---
------------------------------------------------------------------------------------------------
Create or ALter proc [dbo].[udp_reOrganizeIndex]
@schemaname varchar(100),
@dbname varchar(100),
@tablename varchar(100),
@indexname varchar(100)
as
begin
exec('use '+@dbname+'; Alter Index '+@indexname+' on '+@schemaname+'.'+@tablename+' Reorganize;')
end
GO
/*-------------------------------------------------------------------------------------------------*/


------------------------------------------------------------------------------------------------
--- 19. Takes Database Differential Backup ---
------------------------------------------------------------------------------------------------
Create or ALter proc [dbo].[upd_getDifferentialBackup]
@dbname varchar(50),
@backupname varchar(50)
as
Begin
declare @recover_model varchar(50)
declare @path varchar(100)
create table #temp_backupDetails(
name varchar(50),
last_backup_time varchar(50),
recovery_model_desc varchar(20),
state_desc varchar(10),
backup_type varchar(30),
physical_device_name varchar(100)
)
insert into #temp_backupDetails
exec udp_getBackupsDetails


select @recover_model = recovery_model_desc, @path = physical_device_name from #temp_backupDetails
where name = @dbname

if @recover_model <> 'SIMPLE' and @path is not null
begin
exec('backup database '+@dbname+'
to disk =''E:\Backups\differentialBak'+@backupname+'.dif''')
end
else
print('Can''t take differential backup')
select * from #temp_backupDetails;
drop table #temp_backupDetails
ENd
GO
/*-------------------------------------------------------------------------------------------------*/


------------------------------------------------------------------------------------------------
--- 20. Takes Database Transactional Backup ---
------------------------------------------------------------------------------------------------
Create or Alter proc [dbo].[upd_getTransactionalBackup]
@dbname varchar(50),
@backupname varchar(50)
as
Begin
declare @recover_model varchar(50)
declare @path varchar(100)
create table #temp_backupDets(
name varchar(50),
last_backup_time varchar(50),
recovery_model_desc varchar(20),
state_desc varchar(10),
backup_type varchar(30),
physical_device_name varchar(100)
)
insert into #temp_backupDets
exec udp_getBackupsDetails


select @recover_model = recovery_model_desc, @path = physical_device_name from #temp_backupDets
where name = @dbname

if @recover_model <> 'SIMPLE' and @path is not null
begin
exec('backup log '+@dbname+'
to disk =''E:\Backups\transactionalBak\'+@backupname+'.trn''')
end
else
print('Can''t take transactional backup')
select * from #temp_backupDets;
drop table #temp_backupDets
ENd
GO
/*-------------------------------------------------------------------------------------------------*/


------------------------------------------------------------------------------------------------
--- 21. Returns BackupInfo about single database ---
------------------------------------------------------------------------------------------------
Create or Alter proc [dbo].[upd_specificDatabaseBackupInfo]
@dbname varchar(50)
as
Begin
declare @recover_model varchar(50)
declare @path varchar(100)
create table #temp_backupDetail(
name varchar(50),
last_backup_time varchar(50),
recovery_model_desc varchar(20),
state_desc varchar(10),
backup_type varchar(30),
physical_device_name varchar(100)
)
insert into #temp_backupDetail
exec udp_getBackupsDetails

select recovery_model_desc,physical_device_name from #temp_backupDetail where name = @dbname;
drop table #temp_backupDetail
ENd
GO
/*-------------------------------------------------------------------------------------------------*/


------------------------------------------------------------------------------------------------
--- 22. Gives Log Usage Info ABout ALl Databases---
------------------------------------------------------------------------------------------------
create or alter proc udp_getOverallLogUsage
as
begin
CREATE TABLE #LogSpace(  
DBName VARCHAR(100),  
LogSize VARCHAR(50),  
LogSpaceUsed_Percent VARCHAR(100),   
LStatus CHAR(1));  
  
INSERT INTO #LogSpace  
EXEC ('DBCC SQLPERF(LOGSPACE) WITH NO_INFOMSGS;'); 
select * from #LogSpace;
drop table #LogSpace
end
GO
/*-------------------------------------------------------------------------------------------------*/


------------------------------------------------------------------------------------------------
--- 23. Gives Overall Memory Usage of Server ---
------------------------------------------------------------------------------------------------
create or alter proc udp_getOverallMemoryUsage
as
begin
CREATE TABLE #Memory_BPool (  
BPool_Committed_MB VARCHAR(50),  
BPool_Commit_Tgt_MB VARCHAR(50),  
BPool_Visible_MB VARCHAR(50));  
  
INSERT INTO #Memory_BPool   
SELECT  
      (committed_kb)/1024.0 as BPool_Committed_MB,  
      (committed_target_kb)/1024.0 as BPool_Commit_Tgt_MB,  
      (visible_target_kb)/1024.0 as BPool_Visible_MB  
FROM  sys.dm_os_sys_info;  

CREATE TABLE #Memory_sys (  
total_physical_memory_mb VARCHAR(50),  
available_physical_memory_mb VARCHAR(50),  
total_page_file_mb VARCHAR(50),  
available_page_file_mb VARCHAR(50),  
Percentage_Used VARCHAR(50),  
system_memory_state_desc VARCHAR(50));  
  
INSERT INTO #Memory_sys  
select  
      total_physical_memory_kb/1024 AS total_physical_memory_mb,  
      available_physical_memory_kb/1024 AS available_physical_memory_mb,  
      total_page_file_kb/1024 AS total_page_file_mb,  
      available_page_file_kb/1024 AS available_page_file_mb,  
      100 - (100 * CAST(available_physical_memory_kb AS DECIMAL(18,3))/CAST(total_physical_memory_kb AS DECIMAL(18,3)))   
      AS 'Percentage_Used',  
      system_memory_state_desc  
from  sys.dm_os_sys_memory;  
  
  
CREATE TABLE #Memory_process(  
physical_memory_in_use_GB VARCHAR(50),  
locked_page_allocations_GB VARCHAR(50),  
virtual_address_space_committed_GB VARCHAR(50),  
available_commit_limit_GB VARCHAR(50),  
page_fault_count VARCHAR(50))  
  
INSERT INTO #Memory_process  
select  
      physical_memory_in_use_kb/1048576.0 AS 'Physical_Memory_In_Use(GB)',  
      locked_page_allocations_kb/1048576.0 AS 'Locked_Page_Allocations(GB)',  
      virtual_address_space_committed_kb/1048576.0 AS 'Virtual_Address_Space_Committed(GB)',  
      available_commit_limit_kb/1048576.0 AS 'Available_Commit_Limit(GB)',  
      page_fault_count as 'Page_Fault_Count'  
from  sys.dm_os_process_memory;  
  
  
CREATE TABLE #Memory(  
ID INT IDENTITY NOT NULL,
Parameter VARCHAR(200),  
Value VARCHAR(100));  
  
INSERT INTO #Memory   
SELECT 'BPool_Committed_MB',BPool_Committed_MB FROM #Memory_BPool  
UNION  
SELECT 'BPool_Commit_Tgt_MB', BPool_Commit_Tgt_MB FROM #Memory_BPool  
UNION   
SELECT 'BPool_Visible_MB', BPool_Visible_MB FROM #Memory_BPool  
UNION  
SELECT 'Total_Physical_Memory_MB',total_physical_memory_mb FROM #Memory_sys  
UNION  
SELECT 'Available_Physical_Memory_MB',available_physical_memory_mb FROM #Memory_sys
UNION  
SELECT 'Percentage_Used',Percentage_Used FROM #Memory_sys  
UNION
SELECT 'System_memory_state_desc',system_memory_state_desc FROM #Memory_sys  
UNION  
SELECT 'Total_page_file_mb',total_page_file_mb FROM #Memory_sys  
UNION  
SELECT 'Available_page_file_mb',available_page_file_mb FROM #Memory_sys  
UNION  
SELECT 'Physical_memory_in_use_GB',physical_memory_in_use_GB FROM #Memory_process  
UNION  
SELECT 'Locked_page_allocations_GB',locked_page_allocations_GB FROM #Memory_process  
UNION  
SELECT 'Virtual_Address_Space_Committed_GB',virtual_address_space_committed_GB FROM #Memory_process  
UNION  
SELECT 'Available_Commit_Limit_GB',available_commit_limit_GB FROM #Memory_process  
UNION  
SELECT 'Page_Fault_Count',page_fault_count FROM #Memory_process;
select * from #Memory;
drop table #Memory;
drop table #Memory_BPool;
drop table #Memory_process;
drop table #Memory_sys;
end
GO
/*-------------------------------------------------------------------------------------------------*/


------------------------------------------------------------------------------------------------
--- 24. Gives View Details of Specific Database ---
------------------------------------------------------------------------------------------------
create or alter proc udp_getAllViews
@dbname varchar(100)
as 
begin
exec('use '+@dbname+';
select sv.name as view_name, ss.name as schema_name, sv.create_date, sv.modify_date from sys.views sv
inner join sys.schemas ss
on sv.schema_id = ss.schema_id;')
end
go
/*-------------------------------------------------------------------------------------------------*/


------------------------------------------------------------------------------------------------
--- 25. Returns All Backup Details of Selected Database ---
------------------------------------------------------------------------------------------------
create or alter  proc [dbo].[udp_getUserInputDatabaseBackupDetails]
@dbname varchar(100)
as
begin
SELECT 
   CONVERT(CHAR(100), SERVERPROPERTY('Servername')) AS Server, 
   msdb.dbo.backupset.database_name, 
   msdb.dbo.backupset.backup_start_date, 
   msdb.dbo.backupset.backup_finish_date, 
   msdb.dbo.backupset.expiration_date, 
   CASE msdb..backupset.type 
      WHEN 'D' THEN 'Full database'
            WHEN 'I' THEN 'Differential database'
            WHEN 'L' THEN 'Log'
            WHEN 'F' THEN 'File or filegroup'
            WHEN 'G' THEN 'Differential file'
            WHEN 'P' THEN 'Partial'
            WHEN 'Q' THEN 'Differential partial'
            ELSE 'Unknown' END AS backup_type, 
   msdb.dbo.backupset.backup_size, 
   msdb.dbo.backupmediafamily.logical_device_name, 
   msdb.dbo.backupmediafamily.physical_device_name, 
   msdb.dbo.backupset.name AS backupset_name, 
   msdb.dbo.backupset.description 
FROM 
   msdb.dbo.backupmediafamily 
   INNER JOIN msdb.dbo.backupset ON msdb.dbo.backupmediafamily.media_set_id = msdb.dbo.backupset.media_set_id 
WHERE 
   (CONVERT(datetime, msdb.dbo.backupset.backup_start_date, 102) >= GETDATE() - 10000) and database_name = @dbname
ORDER BY 
   msdb.dbo.backupset.database_name, 
   msdb.dbo.backupset.backup_finish_date 
   end
GO
/*-------------------------------------------------------------------------------------------------*/