DROP DATABASE IF EXISTS PedrosCantina;

CREATE DATABASE PedrosCantina;

USE PedrosCantina;

CREATE TABLE Employee
(
    EmployeeId INT AUTO_INCREMENT PRIMARY KEY,
    FirstName VARCHAR(50) NOT NULL,
    LastName VARCHAR(50) NOT NULL,
    Phone VARCHAR(20),
    Email VARCHAR(100),
    CanLead BOOLEAN NOT NULL DEFAULT FALSE
);

CREATE TABLE WorkShift
(
    ShiftId INT AUTO_INCREMENT PRIMARY KEY,
    ShiftDate DATE NOT NULL,
    StartTime TIME NOT NULL,
    EndTime TIME NOT NULL
);

CREATE TABLE ShiftAssignment
(
    ShiftId INT NOT NULL,
    EmployeeId INT NOT NULL,

    PRIMARY KEY (ShiftId, EmployeeId),

    FOREIGN KEY (ShiftId)
        REFERENCES WorkShift(ShiftId),

    FOREIGN KEY (EmployeeId)
        REFERENCES Employee(EmployeeId)
);

INSERT INTO Employee
    (FirstName, LastName, Phone, Email, CanLead)
VALUES
    ('Pedro', 'Gonzalez', '11111111', 'pedro@cantina.dk', TRUE),
    ('Jacoby', 'Jensen', '22222222', 'jacoby@cantina.dk', TRUE),
    ('Anna', 'Andersen', '33333333', 'anna@cantina.dk', FALSE),
    ('Martin', 'Nielsen', '44444444', 'martin@cantina.dk', FALSE),
    ('Sara', 'Hansen', '55555555', 'sara@cantina.dk', FALSE),
    ('Peter', 'Larsen', '66666666', 'peter@cantina.dk', FALSE);

SELECT * FROM Employee;

INSERT INTO WorkShift
    (ShiftDate, StartTime, EndTime)
VALUES
    ('2026-10-01', '09:00:00', '14:00:00'),
    ('2026-10-01', '14:00:00', '19:00:00'),

    ('2026-10-02', '09:00:00', '14:00:00'),
    ('2026-10-02', '14:00:00', '19:00:00'),

    ('2026-10-03', '09:00:00', '14:00:00'),
    ('2026-10-03', '14:00:00', '19:00:00');

SELECT * FROM WorkShift;

INSERT INTO ShiftAssignment
    (ShiftId, EmployeeId)
VALUES

    -- October 1 morning
    (1, 1),
    (1, 3),
    (1, 4),

    -- October 1 afternoon
    (2, 2),
    (2, 5),
    (2, 6),

    -- October 2 morning
    (3, 1),
    (3, 4),

    -- October 2 afternoon
    (4, 2),
    (4, 3),
    (4, 5),

    -- October 3 morning
    (5, 1),
    (5, 5),
    (5, 6),

    -- October 3 afternoon
    (6, 2),
    (6, 3),
    (6, 4);

SELECT * FROM ShiftAssignment;

SELECT
    ws.ShiftDate,
    ws.StartTime,
    ws.EndTime,
    e.FirstName,
    e.LastName,
    e.CanLead
FROM WorkShift ws

INNER JOIN ShiftAssignment sa
    ON ws.ShiftId = sa.ShiftId

INNER JOIN Employee e
    ON sa.EmployeeId = e.EmployeeId

WHERE YEAR(ws.ShiftDate) = 2026
  AND MONTH(ws.ShiftDate) = 10

ORDER BY
    ws.ShiftDate,
    ws.StartTime,
    e.CanLead DESC,
    e.FirstName;

SELECT
    e.EmployeeId,
    e.FirstName,
    e.LastName,
    COUNT(ws.ShiftId) AS NumberOfShifts,
    COUNT(ws.ShiftId) * 5 AS HoursWorked
FROM Employee e

LEFT JOIN ShiftAssignment sa
    ON e.EmployeeId = sa.EmployeeId

LEFT JOIN WorkShift ws
    ON sa.ShiftId = ws.ShiftId
    AND YEAR(ws.ShiftDate) = 2026
    AND MONTH(ws.ShiftDate) = 10

GROUP BY
    e.EmployeeId,
    e.FirstName,
    e.LastName

ORDER BY
    HoursWorked DESC;

SELECT
    e.EmployeeId,
    e.FirstName,
    e.LastName,
    e.Phone,
    e.Email,
    e.CanLead
FROM Employee e

WHERE e.EmployeeId NOT IN
(
    SELECT sa.EmployeeId
    FROM ShiftAssignment sa
    WHERE sa.ShiftId = 1
)

ORDER BY
    e.CanLead DESC,
    e.FirstName;

SELECT
    e.EmployeeId,
    e.FirstName,
    e.LastName,
    MONTH(ws.ShiftDate) AS WorkMonth,
    COUNT(ws.ShiftId) AS NumberOfShifts,
    COUNT(ws.ShiftId) * 5 AS HoursWorked
FROM Employee e

INNER JOIN ShiftAssignment sa
    ON e.EmployeeId = sa.EmployeeId

INNER JOIN WorkShift ws
    ON sa.ShiftId = ws.ShiftId

WHERE YEAR(ws.ShiftDate) = 2026

GROUP BY
    e.EmployeeId,
    e.FirstName,
    e.LastName,
    MONTH(ws.ShiftDate)

ORDER BY
    e.EmployeeId,
    WorkMonth;

INSERT INTO WorkShift
    (ShiftDate, StartTime, EndTime)
VALUES
    ('2026-10-04', '09:00:00', '14:00:00'),
    ('2026-10-04', '14:00:00', '19:00:00');

SELECT * FROM WorkShift;

INSERT INTO ShiftAssignment
    (ShiftId, EmployeeId)
VALUES
    (7, 1),
    (7, 3),
    (7, 6),

    (8, 2),
    (8, 4),
    (8, 5);

SELECT * FROM ShiftAssignment
WHERE ShiftId IN (7, 8);

DELETE FROM ShiftAssignment
WHERE ShiftId = 8
AND EmployeeId = 4;

INSERT INTO ShiftAssignment
    (ShiftId, EmployeeId)
VALUES
    (8, 6);

SELECT
    ws.ShiftDate,
    ws.StartTime,
    ws.EndTime,
    e.FirstName,
    e.LastName
FROM WorkShift ws

INNER JOIN ShiftAssignment sa
    ON ws.ShiftId = sa.ShiftId

INNER JOIN Employee e
    ON sa.EmployeeId = e.EmployeeId

WHERE ws.ShiftId = 8;