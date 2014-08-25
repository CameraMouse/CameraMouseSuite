DROP TABLE UserInfo;
DROP TABLE LogEvent;
DROP TABLE PointerEvent;
DROP TABLE CursorEvent;
DROP TABLE ClickEvent;
DROP TABLE FrameEvent;

CREATE TABLE UserInfo (
	id BIGINT NOT NULL AUTO_INCREMENT PRIMARY KEY,
        firstName VARCHAR(200) NOT NULL,
	lastName VARCHAR(200) NOT NULL,
	ipAddress INT UNSIGNED NOT NULL, 
	ageGroup INT,
	email VARCHAR(200),
	city VARCHAR(200),
	state VARCHAR(200),
	country VARCHAR(200),
	disability VARCHAR(200),	
	consentAdultSignature VARCHAR(200),
	consentAdultWitness VARCHAR(200),
	consentAdultDate VARCHAR(200),
	consentChildWitness VARCHAR(200),
	consentChildWitnessRelationship VARCHAR(200),
	consentChildDate VARCHAR(200),
	consentParentSignature VARCHAR(200),
	consentParentDate VARCHAR(200)
        );

CREATE UNIQUE INDEX UserInfoNameAddressIndex ON UserInfo(firstName,lastName,ipAddress);


CREATE TABLE LogEvent (
	id BIGINT NOT NULL AUTO_INCREMENT PRIMARY KEY,
	userInfo BIGINT NOT NULL,
	session INT NOT NULL,
	clientId MEDIUMINT NOT NULL,
	timeInMillis MEDIUMINT NOT NULL,
	eventType VARCHAR(30) NOT NULL,	
	universalFileTime BIGINT,
	timeZone VARCHAR(100),	
	data MEDIUMTEXT,
        FOREIGN KEY (userInfo) REFERENCES UserInfo (id)
	);

CREATE UNIQUE INDEX UserSessionLocalIndex ON LogEvent (userInfo,session,clientId);


CREATE TABLE PointerEvent (
	logEvent BIGINT NOT NULL PRIMARY KEY,
	x INT NOT NULL,
	y INT NOT NULL,
        FOREIGN KEY (logEvent) REFERENCES LogEvent (id)
	);

CREATE TABLE CursorEvent (
	logEvent BIGINT NOT NULL PRIMARY KEY,
	x INT NOT NULL,
	y INT NOT NULL,
	screenWidth INT NOT NULL,
	screenHeight INT NOT NULL,
        FOREIGN KEY (logEvent) REFERENCES LogEvent (id)	
	);


CREATE TABLE ClickEvent (
	logEvent BIGINT NOT NULL PRIMARY KEY,
	x INT NOT NULL,
	y INT NOT NULL,
	clickType VARCHAR(20),
	screenWidth INT NOT NULL,
	screenHeight INT NOT NULL,	
        FOREIGN KEY (logEvent) REFERENCES LogEvent (id)	
	);

CREATE TABLE FrameEvent (
	logEvent BIGINT NOT NULL,
	cameraNum INT NOT NULL,
	width INT NOT NULL,
	height INT NOT NULL,
	format VARCHAR(20),
	img MEDIUMBLOB NOT NULL,
	FOREIGN KEY (logEvent) REFERENCES LogEvent (id)	
	);

CREATE UNIQUE INDEX FrameEventIndex ON FrameEvent (logEvent,cameraNum);