-- Execute inserts into the following order

-- 1) Insert users into the Users table
INSERT INTO Users (Id, Username, Email, ProfilePictureUrl, Bio, CreatedAt, IsAdmin)
VALUES
('A4D8E6F7-2A5D-4E6C-91A3-B123F6E789D1', 'cyberpunk_fan', 'cyberpunk_fan@example.com',
 'https://randomuser.me/api/portraits/lego/1.jpg',
 'Exploring digital worlds, one byte at a time.', '2025-01-01T12:00:00Z', 0),
('D8BCA0F9-77E6-4E1A-86CF-334F5C4E9C9D', 'android_dreamer', 'android_dreamer@example.com',
 'https://randomuser.me/api/portraits/lego/2.jpg',
 'Lover of sci-fi and artificial intelligence.', '2025-01-02T12:00:00Z', 0),
('C4E2F7A6-9D2E-49C9-B4E3-F69D3B1C28AF', 'neo_hacker', 'neo_hacker@example.com',
 'https://randomuser.me/api/portraits/lego/3.jpg',
 'Hacking the mainframe in style.', '2025-01-03T12:00:00Z', 1);

-- 2) Insert books into the Books table with valid AddedBy values
INSERT INTO Books (Id, AddedBy, PublishedDate, Title, Author, Description, CoverImageUrl, Rating)
VALUES
('96E08992-1D99-4D55-9344-A015E4AA9C63', 'A4D8E6F7-2A5D-4E6C-91A3-B123F6E789D1', '1984-07-01',
 'Neuromancer', 'William Gibson',
 'A groundbreaking cyberpunk novel about a washed-up computer hacker hired for one last job in cyberspace.',
 'https://example.com/neuromancer.jpg', 4.5),
('B8C29512-43DE-49B1-8D4F-CF9C2E23B654', 'D8BCA0F9-77E6-4E1A-86CF-334F5C4E9C9D', '1968-03-01',
 'Do Androids Dream of Electric Sheep?', 'Philip K. Dick',
 'A sci-fi classic exploring the ethical dilemmas of artificial intelligence and human identity.',
 'https://example.com/androids.jpg', 4.3),
('35A9D6F4-77D3-4F6B-8A7E-2B64D9F098C7', 'C4E2F7A6-9D2E-49C9-B4E3-F69D3B1C28AF', '1992-06-01',
 'Snow Crash', 'Neal Stephenson',
 'A fast-paced cyberpunk adventure blending virtual reality, ancient mythology, and corporate espionage.',
 'https://example.com/snowcrash.jpg', 4.4);
