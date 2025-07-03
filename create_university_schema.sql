CREATE DATABASE IF NOT EXISTS university_db CHARACTER SET utf8mb4;
USE university_db;

CREATE TABLE Students (
    Id                INT AUTO_INCREMENT PRIMARY KEY,
    Matriculation     VARCHAR(20) NOT NULL UNIQUE,
    FirstName         VARCHAR(60) NOT NULL,
    LastName          VARCHAR(60) NOT NULL,
    Email             VARCHAR(100) NOT NULL UNIQUE,
    CreatedAt         DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP
);

CREATE TABLE Courses (
    Id            INT AUTO_INCREMENT PRIMARY KEY,
    Code          VARCHAR(10) NOT NULL UNIQUE,
    Name          VARCHAR(120) NOT NULL,
    CreditHours   TINYINT UNSIGNED NOT NULL CHECK (CreditHours BETWEEN 1 AND 9)
);

CREATE TABLE SemesterEnrollments (
    Id              INT AUTO_INCREMENT PRIMARY KEY,
    StudentId       INT NOT NULL,
    Year            SMALLINT NOT NULL,
    Term            ENUM('Spring','Summer','Fall') NOT NULL,
    MaxCreditHours  TINYINT UNSIGNED NOT NULL DEFAULT 21,
    CreatedAt       DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
    CONSTRAINT FK_Semester_Student FOREIGN KEY (StudentId)
        REFERENCES Students(Id) ON DELETE RESTRICT
);

CREATE TABLE EnrolledCourses (
    Id                   INT AUTO_INCREMENT PRIMARY KEY,
    SemesterEnrollmentId INT NOT NULL,
    CourseId             INT NOT NULL,
    CreditHours          TINYINT UNSIGNED NOT NULL,
    UNIQUE KEY UK_Semester_Course (SemesterEnrollmentId, CourseId),
    CONSTRAINT FK_Enrolled_Semester FOREIGN KEY (SemesterEnrollmentId)
        REFERENCES SemesterEnrollments(Id) ON DELETE CASCADE,
    CONSTRAINT FK_Enrolled_Course FOREIGN KEY (CourseId)
        REFERENCES Courses(Id)
);
