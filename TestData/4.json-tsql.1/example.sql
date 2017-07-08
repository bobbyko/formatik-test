DECLARE @myTable TABLE (
    [id] VARCHAR(MAX) NOT NULL PRIMARY KEY,
    [timestamp] DATETIME NOT NULL,
    [accountId] INT NOT NULL,
    [countryCode] VARCHAR(MAX) NOT NULL
)

INSERT INTO @myTable VALUES ("58e4a804dde973cb0034db4d", "2017-04-05T00:00:00.000Z", 1624, "US");
INSERT INTO @myTable VALUES ("58e433872f55e96fb7b83d66", "2017-04-05T00:00:07.133Z", 1770, "US");
INSERT INTO @myTable VALUES ("58e433877908d0543564a5ad", "2017-04-05T00:00:07.756Z", 1781, "US");

SELECT  countryCode,
        COUNT(*)
FROM @myTable T
GROUP BY T.countryCode
