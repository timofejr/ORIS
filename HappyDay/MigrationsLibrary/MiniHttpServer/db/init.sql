CREATE TABLE IF NOT EXISTS users  (
    Id SERIAL PRIMARY KEY,
    Username VARCHAR(255) NOT NULL,
    Email VARCHAR(255) NOT NULL
);

INSERT INTO users (username, email)
VALUES  ('Timur', 'timur@gmail.com'),
        ('Pasha228', 'pasha@gmail.com'),
        ('Abramskiy', 'abramskiy@gmail.com'),
        ('Timerkan', 'timerkhan@gmail.com'),
        ('Putin', 'putin@gmail.com');