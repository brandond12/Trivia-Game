CREATE DATABASE TriviaGame;
USE TriviaGame;

CREATE TABLE Users (
Name VARCHAR(50),
IsActive BOOL,
PRIMARY KEY (Name)
);

CREATE TABLE UserGames (
GameNumber INT,
Name VARCHAR(50),
GameScore INT,
PRIMARY KEY (GameNumber, Name),
FOREIGN KEY (Name) REFERENCES Users(Name)
);

CREATE TABLE TestQuestions (
QuestionNumber INT,
Question VARCHAR(300),
PRIMARY KEY (QuestionNumber)
);

CREATE TABLE TestAnswers (
QuestionNumber INT,
Answer VARCHAR(100),
IsCorrect BOOL,
PRIMARY KEY (QuestionNumber, Answer),
FOREIGN KEY (QuestionNumber) REFERENCES TestQuestions(QuestionNumber),
INDEX (Answer)
);

CREATE TABLE UserAnswer (
GameNumber INT,
Name VARCHAR(50),
GameQuestion INT,
UserAnswer VARCHAR(100),
AnswerScore INT,
PRIMARY KEY (GameNumber, Name, GameQuestion),
FOREIGN KEY (GameNumber, Name) REFERENCES UserGames(GameNumber, Name),
FOREIGN KEY (GameQuestion) REFERENCES TestQuestions(QuestionNumber),
FOREIGN KEY (UserAnswer) REFERENCES TestAnswers(Answer)
);