SELECT
    s.Id AS SuggestionId,
    s.BookId,
    b.Title AS BookTitle,
    b.Author AS BookAuthor,
    b.CoverImageUrl AS BookCoverImageUrl,
    b.Description AS BookDescription,
    s.SuggestedBookId,
    sb.Title AS SuggestedBookTitle,
    sb.Author AS SuggestedBookAuthor,
    sb.CoverImageUrl AS SuggestedBookCoverImageUrl,
    sb.Description AS SuggestedBookDescription,
    s.SuggestedBy AS SuggestedByUserId,
    su.Username AS SuggestedByUserName,
    s.CreatedAt AS SuggestionCreatedAt,
    s.Reason AS SuggestionReason,
    (SELECT COUNT(*) FROM Upvotes WHERE SuggestionId = s.Id) AS UpvotesCount,
    (SELECT COUNT(*) FROM Comments WHERE SuggestionId = s.Id) AS CommentsCount
FROM Suggestions s
INNER JOIN Books b ON s.BookId = b.Id
INNER JOIN Books sb ON s.SuggestedBookId = sb.Id
INNER JOIN Users su ON s.SuggestedBy = su.Id
LEFT JOIN Users u ON b.AddedBy = u.Id;