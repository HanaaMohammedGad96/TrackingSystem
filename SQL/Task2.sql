CREATE DATABASE TrackingSysDb;

Go
CREATE TABLE Employee (
  Id INT PRIMARY KEY,
  Name VARCHAR(50) NOT NULL,
  ManagerId INT,
  FOREIGN KEY (ManagerId) REFERENCES employee(Id)
);

Go
CREATE TABLE Project (
  Id INT PRIMARY KEY,
  Name VARCHAR(50) NOT NULL
);

Go
CREATE TABLE EmployeeProject (
  Id INT PRIMARY KEY,
  EmployeeId INT NOT NULL,
  ProjectId INT NOT NULL,
  TimeInMinutes INT NOT NULL,
  WorkDate DATE NOT NULL,
  TaskDescription NVARCHAR(500),
  FOREIGN KEY (EmployeeId) REFERENCES employee(Id),
  FOREIGN KEY (ProjectId) REFERENCES project(Id)
);

Go
CREATE PROCEDURE GetManagerProjectTime
@StartDate DATE,
@EndDate DATE,
@Threshold INT
AS
BEGIN
  SELECT
    m.Name AS ManagerName,
    p.Name AS ProjectName,
    SUM(ep.TimeInMinutes) AS TotalTime
  FROM
    Employee e
    JOIN Employee m ON e.ManagerId = m.Id
    JOIN EmployeeProject ep ON e.Id = ep.EmployeeId
    JOIN Project p ON ep.ProjectId = p.Id
  WHERE
    ep.WorkDate BETWEEN @StartDate AND @EndDate
  GROUP BY
    m.Name,
    p.Name
  HAVING
    SUM(ep.TimeInMinutes) >= @Threshold;
END;
Go
-- Insert data into Project table
INSERT INTO Project (Id, Name)
VALUES
  (1, 'Project A'),
  (2, 'Project B'),
  (3, 'Project C');
Go
-- Insert data into Employee table
INSERT INTO Employee (Id, Name, ManagerId)
VALUES
  (1, 'John Smith', NULL),
  (2, 'Jane Doe', 1),
  (3, 'Bob Johnson', 1),
  (4, 'Mary Williams', 2),
  (5, 'Tom Brown', 2),
  (6, 'Jane Dooe', 3),
  (7, 'Jane Doee', 3),
  (8, 'Jane Dooee', 1);

Go
-- Insert data into EmployeeProject table
INSERT INTO EmployeeProject (Id, EmployeeId, ProjectId, TimeInMinutes, WorkDate, TaskDescription)
VALUES
  (1, 1, 1, 1240, '2023-07-18', 'Task 1'),
  (2, 2, 1, 180, '2023-07-18', 'Task 2'),
  (3, 3, 1, 300, '2023-07-19', 'Task 3'),
  (4, 4, 2, 1120, '2023-07-19', 'Task 4'),
  (5, 5, 2, 180, '2023-07-20', 'Task 5'),
  (6, 1, 3, 240, '2023-07-20', 'Task 6'),
  (7, 8, 3, 240, '2023-07-20', 'Task 7'),
  (8, 7, 3, 740, '2023-07-20', 'Task 8'),
  (9, 1, 3, 20, '2023-07-20', 'Task 9');

Go
--//Write a query which get managerName and total time for its emplyees per project
SELECT
  m.Name AS ManagerName,
  p.Name AS ProjectName,
  SUM(ep.TimeInMinutes) AS TotalTime
FROM
  Employee e
  JOIN Employee m ON e.ManagerId = m.Id
  JOIN EmployeeProject ep ON e.Id = ep.EmployeeId
  JOIN Project p ON ep.ProjectId = p.Id
GROUP BY
  m.Name,
  p.Name;

--//Get managerName and total time which is >= 1000 for its employees per project according startDate and endDate
EXEC GetManagerProjectTime '2023-07-01', '2023-07-31', 1000;